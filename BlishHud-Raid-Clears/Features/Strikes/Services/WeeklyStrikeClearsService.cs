using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blish_HUD;
using RaidClears.Features.Shared.Models;
using Gw2Sharp.WebApi.V2.Models;
using System.Linq;

namespace RaidClears.Features.Strikes.Services;

/// <summary>
/// Syncs weekly strike clears from the GW2 account achievement (e.g. 9125 Weekly Raid Encounters).
/// On API refresh, fetches the achievement bits and updates StrikePersistance; dispatch is left to the caller.
/// </summary>
public static class WeeklyStrikeClearsService
{
    private static readonly List<TokenPermission> NecessaryPermissions = new()
    {
        TokenPermission.Account,
        TokenPermission.Progression
    };

    /// <summary>
    /// Fetches the weekly strike achievement from the API and updates StrikePersistance for the current account.
    /// Only runs if StrikeData has WeeklyAchievementId set and WeeklyAchievementBitStrikeIds populated.
    /// Caller should then invoke MapWatcher.DispatchCurrentStrikeClears() (e.g. on main thread) to update the UI.
    /// </summary>
    public static async Task RefreshFromApiAsync()
    {
        var strikeData = Service.StrikeData;
        if (strikeData.WeeklyAchievementId == 0 || strikeData.WeeklyAchievementBitStrikeIds == null || strikeData.WeeklyAchievementBitStrikeIds.Count == 0)
            return;

        var gw2ApiManager = Service.Gw2ApiManager;
        var logger = Logger.GetLogger<Module>();

        if (gw2ApiManager == null || !gw2ApiManager.HasPermissions(NecessaryPermissions))
            return;

        var account = Service.CurrentAccountName;
        if (string.IsNullOrEmpty(account))
            return;

        try
        {
            var allAchievements = await gw2ApiManager.Gw2ApiClient.V2.Account.Achievements.GetAsync().ConfigureAwait(false);
            var list = allAchievements?.ToList() ?? new List<AccountAchievement>();
            var achievement = list.Find(x => x.Id == strikeData.WeeklyAchievementId);
            if (achievement == null)
                return;

            var completedBits = new HashSet<int>(achievement.Bits ?? Array.Empty<int>());
            var mapping = strikeData.WeeklyAchievementBitStrikeIds;
            var persistence = Service.StrikePersistance;

            for (var i = 0; i < mapping.Count; i++)
            {
                var strikeId = mapping[i];
                var mission = strikeData.GetBossEncounterById(strikeId);
                if (completedBits.Contains(i))
                    persistence.SaveClear(account, mission);
                else
                    persistence.RemoveClear(account, mission);
            }
        }
        catch (Exception e)
        {
            logger.Warn(e, "Could not refresh weekly strike clears from API");
        }
    }
}
