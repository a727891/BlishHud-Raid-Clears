using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blish_HUD;
using Newtonsoft.Json;
using RaidClears.Features.Raids.Models;
using RaidClears.Features.Shared.Models;
using RaidClears;
using Gw2Sharp.WebApi.V2.Models;

namespace RaidClears.Features.Raids.Services;

/// <summary>
/// Collects mentor achievement IDs from raid data, fetches progress from the GW2 API,
/// caches progress locally, and notifies when progress changes.
/// </summary>
public class MentorAchievementProgressService
{
    private const string CacheFilename = "mentor_achievement_progress.json";
    public const int DefaultMentorAchievementMax = 1000;

    private static readonly List<TokenPermission> NecessaryPermissions = new()
    {
        TokenPermission.Account,
        TokenPermission.Progression
    };

    private readonly object _progressLock = new();
    private Dictionary<int, MentorAchievementProgressEntry> _progress = new();
    private readonly Dictionary<int, int> _mentorAchievementMax = new();
    private readonly RaidData _raidData;

    public MentorAchievementProgressService(RaidData raidData)
    {
        _raidData = raidData ?? throw new ArgumentNullException(nameof(raidData));
        LoadMentorAchievementMax();
    }

    private void LoadMentorAchievementMax()
    {
        _mentorAchievementMax.Clear();

        foreach (var expansion in _raidData.Expansions)
        {
            foreach (var wing in expansion.Wings)
            {
                foreach (var encounter in wing.Encounters)
                {
                    if (!encounter.MentorAchievementId.HasValue)
                        continue;

                    var id = encounter.MentorAchievementId.Value;
                    var max = encounter.MentorAchievementMax ?? DefaultMentorAchievementMax;
                    if (max <= 0)
                        max = DefaultMentorAchievementMax;

                    _mentorAchievementMax[id] = max;
                }
            }
        }
    }

    /// <summary>
    /// Current mentor achievement progress by achievement ID. Empty until first successful fetch or load from cache.
    /// </summary>
    public IReadOnlyDictionary<int, MentorAchievementProgressEntry> Progress
    {
        get
        {
            lock (_progressLock)
            {
                return new Dictionary<int, MentorAchievementProgressEntry>(_progress);
            }
        }
    }

    /// <summary>
    /// Raised when progress has been updated (from API or cache) and at least one mentor achievement gained progress (current increased).
    /// </summary>
    public event EventHandler<MentorProgressUpdatedEventArgs>? ProgressUpdated;

    /// <summary>
    /// Collects all mentor achievement IDs from raid encounters.
    /// </summary>
    public IReadOnlyCollection<int> GetMentorAchievementIds()
    {
        return _mentorAchievementMax.Keys.ToList();
    }

    /// <summary>
    /// Loads cached progress from disk. Call during module load so progress is available before first API poll.
    /// </summary>
    public void LoadCache()
    {
        var path = GetCacheFilePath();
        if (!System.IO.File.Exists(path))
            return;

        try
        {
            using var reader = new System.IO.StreamReader(path, Encoding.UTF8);
            var json = reader.ReadToEnd();
            var cache = JsonConvert.DeserializeObject<MentorAchievementProgressCache>(json);
            if (cache?.Achievements == null || cache.Achievements.Count == 0)
                return;

            var dict = new Dictionary<int, MentorAchievementProgressEntry>();
            foreach (var entry in cache.Achievements)
            {
                if (_mentorAchievementMax.TryGetValue(entry.Id, out var max))
                {
                    entry.Max = max;
                    dict[entry.Id] = entry;
                }
            }

            lock (_progressLock)
            {
                _progress = dict;
            }
        }
        catch (Exception ex)
        {
            Logger.GetLogger<Module>().Warn(ex, "Failed to load mentor achievement progress cache");
        }
    }

    /// <summary>
    /// Fetches progress from the GW2 API, updates cache if data changed, and raises ProgressUpdated.
    /// </summary>
    public async Task RefreshFromApiAsync()
    {
        if (Service.Settings?.RaidSettings?.RaidPanelMentorProgress?.Value != true)
            return;

        var gw2ApiManager = Service.Gw2ApiManager;
        var logger = Logger.GetLogger<Module>();

        if (gw2ApiManager == null || !gw2ApiManager.HasPermissions(NecessaryPermissions))
            return;

        var mentorIds = GetMentorAchievementIds();
        if (mentorIds.Count == 0)
            return;

        try
        {
            var allAchievements = await gw2ApiManager.Gw2ApiClient.V2.Account.Achievements.GetAsync();
            var list = allAchievements?.ToList() ?? new List<Gw2Sharp.WebApi.V2.Models.AccountAchievement>();

            var newProgress = new Dictionary<int, MentorAchievementProgressEntry>();
            foreach (var ach in list)
            {
                if (!mentorIds.Contains(ach.Id))
                    continue;

                // The static raid data defines the mentor achievement max tier/points.
                var maxFromStatic = _mentorAchievementMax.TryGetValue(ach.Id, out var defMax) && defMax > 0 ? defMax : ach.Max;

                newProgress[ach.Id] = new MentorAchievementProgressEntry
                {
                    Id = ach.Id,
                    Current = ach.Current,
                    Max = maxFromStatic,
                    Done = ach.Done
                };
            }

            List<MentorProgressChange>? increases = null;
            bool changed = false;
            lock (_progressLock)
            {
                if (!ProgressEquals(_progress, newProgress))
                {
                    increases = new List<MentorProgressChange>();
                    foreach (var kv in newProgress)
                    {
                        if (_progress.TryGetValue(kv.Key, out var old) && kv.Value.Current > old.Current)
                            increases.Add(new MentorProgressChange { AchievementId = kv.Key, PreviousCurrent = old.Current, NewCurrent = kv.Value.Current });
                    }
                    _progress = newProgress;
                    changed = true;
                }
            }

            if (changed)
            {
                SaveCache(newProgress);
                if (increases != null && increases.Count > 0)
                    ProgressUpdated?.Invoke(this, new MentorProgressUpdatedEventArgs { Changes = increases });
            }
        }
        catch (Exception ex)
        {
            logger.Warn(ex, "Failed to fetch mentor achievement progress from API");
        }
    }

    private static bool ProgressEquals(
        Dictionary<int, MentorAchievementProgressEntry> a,
        Dictionary<int, MentorAchievementProgressEntry> b)
    {
        if (a.Count != b.Count) return false;
        foreach (var kv in a)
        {
            if (!b.TryGetValue(kv.Key, out var entry) || !kv.Value.Equals(entry))
                return false;
        }
        return true;
    }

    private void SaveCache(Dictionary<int, MentorAchievementProgressEntry> progress)
    {
        try
        {
            var cache = new MentorAchievementProgressCache
            {
                UpdatedUtc = DateTime.UtcNow.ToString("o"),
                Achievements = progress.Values.ToList()
            };
            var path = GetCacheFilePath();
            var dir = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(dir) && !System.IO.Directory.Exists(dir))
                System.IO.Directory.CreateDirectory(dir);

            using var writer = new System.IO.StreamWriter(path, false, Encoding.UTF8);
            writer.Write(JsonConvert.SerializeObject(cache, Formatting.Indented));
        }
        catch (Exception ex)
        {
            Logger.GetLogger<Module>().Warn(ex, "Failed to save mentor achievement progress cache");
        }
    }

    private string GetCacheFilePath()
    {
        var dir = Service.DirectoriesManager.GetFullDirectoryPath(Module.DIRECTORY_PATH);
        return Path.Combine(dir, CacheFilename);
    }
}
