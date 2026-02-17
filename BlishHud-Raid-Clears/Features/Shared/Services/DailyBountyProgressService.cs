using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Blish_HUD;
using Gw2Sharp.WebApi.V2.Models;
using Newtonsoft.Json;
using RaidClears;

namespace RaidClears.Features.Shared.Services;

/// <summary>
/// Fetches the Daily Raid Bounties achievement category from the official API and the account's
/// completion state for those achievements, so the Daily Bounty panel can mark clears.
/// Category URL is read from DailyBountyData (remotely updatable via JSON).
/// </summary>
public class DailyBountyProgressService
{
    private static readonly List<TokenPermission> NecessaryPermissions = new()
    {
        TokenPermission.Account,
        TokenPermission.Progression
    };

    private readonly object _lock = new();
    private HashSet<int> _completedBountyAchievementIds = new();

    /// <summary>
    /// Achievement IDs that the user has completed today (daily bounty progress from the official API).
    /// Empty if API is unavailable or no bounties are completed.
    /// </summary>
    public IReadOnlyCollection<int> CompletedDailyBountyAchievementIds
    {
        get
        {
            lock (_lock)
            {
                return new List<int>(_completedBountyAchievementIds);
            }
        }
    }

    public bool IsBountyCompleted(int achievementId)
    {
        lock (_lock)
        {
            return _completedBountyAchievementIds.Contains(achievementId);
        }
    }

    /// <summary>
    /// Fetches the category URL from DailyBountyData, gets achievement IDs from the API,
    /// then fetches account progress for those IDs and updates CompletedDailyBountyAchievementIds.
    /// Call on API poll (e.g. same trigger as raid clears).
    /// </summary>
    public async Task RefreshFromApiAsync()
    {
        var bountyData = Service.DailyBountyData;
        if (bountyData == null || !bountyData.Enabled || string.IsNullOrWhiteSpace(bountyData.DailyBountyCategoryUrl))
        {
            lock (_lock) { _completedBountyAchievementIds.Clear(); }
            return;
        }

        var bountyAchievementIds = await FetchCategoryAchievementIdsAsync(bountyData.DailyBountyCategoryUrl).ConfigureAwait(false);
        if (bountyAchievementIds == null || bountyAchievementIds.Count == 0)
        {
            lock (_lock) { _completedBountyAchievementIds.Clear(); }
            return;
        }

        var gw2ApiManager = Service.Gw2ApiManager;
        var logger = Logger.GetLogger<Module>();

        if (gw2ApiManager == null || !gw2ApiManager.HasPermissions(NecessaryPermissions))
        {
            lock (_lock) { _completedBountyAchievementIds.Clear(); }
            return;
        }

        try
        {
            var allAchievements = await gw2ApiManager.Gw2ApiClient.V2.Account.Achievements.GetAsync().ConfigureAwait(false);
            var list = allAchievements?.ToList() ?? new List<Gw2Sharp.WebApi.V2.Models.AccountAchievement>();
            var bountyIdsSet = new HashSet<int>(bountyAchievementIds);
            var completed = new HashSet<int>();

            foreach (var ach in list)
            {
                if (bountyIdsSet.Contains(ach.Id) && ach.Done)
                    completed.Add(ach.Id);
            }

            lock (_lock)
            {
                _completedBountyAchievementIds = completed;
            }
        }
        catch (Exception ex)
        {
            logger.Warn(ex, "Could not fetch daily bounty progress from API");
            lock (_lock) { _completedBountyAchievementIds.Clear(); }
        }
    }

    /// <summary>
    /// Fetches the achievement category JSON from the given URL and returns the achievement IDs array.
    /// </summary>
    private static async Task<List<int>?> FetchCategoryAchievementIdsAsync(string categoryUrl)
    {
        if (string.IsNullOrWhiteSpace(categoryUrl))
            return null;

        try
        {
            using var webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;
            var json = await webClient.DownloadStringTaskAsync(new Uri(categoryUrl)).ConfigureAwait(false);
            var category = JsonConvert.DeserializeObject<AchievementCategoryDto>(json);
            return category?.Achievements ?? new List<int>();
        }
        catch (Exception ex)
        {
            Logger.GetLogger<Module>().Warn(ex, "Could not fetch daily bounty category from {Url}", categoryUrl);
            return null;
        }
    }

    private sealed class AchievementCategoryDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; } = "";

        [JsonProperty("achievements")]
        public List<int> Achievements { get; set; } = new();
    }
}
