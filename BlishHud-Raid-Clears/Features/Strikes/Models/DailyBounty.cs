using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD.Controls;
using RaidClears.Features.Shared.Controls;
using RaidClears.Features.Shared.Models;
using RaidClears.Features.Shared.Services;
using RaidClears.Features.Raids.Models;
using RaidClears.Settings.Models;
using RaidClears;
using RaidClears.Utils;

namespace RaidClears.Features.Strikes.Models;

public class DailyBounty : Strike
{
    private const string BountyIdPrefix = "priority_";

    private readonly StrikeSettings settings = Service.Settings.StrikeSettings;
    private static StrikeSettings Settings => Service.Settings.StrikeSettings;

    public DailyBounty(string name, string id, int index, string shortName, IEnumerable<BoxModel> boxes, Container panel) 
        : base(name, id, index, shortName, boxes)
    {
        Service.ResetWatcher.DailyReset += ResetWatcher_DailyReset;
        if (Service.ApiPollingService != null)
            Service.ApiPollingService.ApiPollingTrigger += OnApiPollingTrigger;
        InitGroup(panel);
        InitBountyEncounters();
    }

    private void OnApiPollingTrigger(object sender, bool e)
    {
        Task.Run(async () =>
        {
            if (Service.DailyBountyProgress == null) return;
            await Service.DailyBountyProgress.RefreshFromApiAsync();
            var completed = Service.DailyBountyProgress.CompletedDailyBountyAchievementIds;
            foreach (var model in boxes)
            {
                var encounterId = model.id;
                var apiId = encounterId.StartsWith(BountyIdPrefix, System.StringComparison.Ordinal)
                    ? encounterId.Substring(BountyIdPrefix.Length)
                    : encounterId;
                var enc = Service.RaidData.GetRaidEncounterByApiId(apiId);
                var bountyAchievementId = enc.DailyBountyAchievementId;
                var cleared = bountyAchievementId.HasValue && completed.Contains(bountyAchievementId.Value);
                model.SetCleared(cleared);
            }
        });
    }

    protected void InitGroup(Container panel)
    {
        var group = new GridGroup(
            panel,
            settings.Style.Layout
        );
        group.VisiblityChanged(Service.StrikeData.GetPriorityVisible());
        SetGridGroupReference(group);

        var labelBox = new GridBox(
            group,
            shortName, name,
            settings.Style.LabelOpacity, settings.Style.FontSize
        );
        SetGroupLabelReference(labelBox);
        labelBox.LayoutChange(settings.Style.Layout);
        labelBox.LabelDisplayChange(settings.Style.LabelDisplay, shortName, shortName);
    }

    protected void InitBountyEncounters()
    {
        var bounties = DailyBountyService.GetDailyBounties();
        var newList = new List<BoxModel>();

        foreach (var encounter in bounties)
        {
            var encounterBox = new GridBox(
                this.GridGroup,
                encounter.shortName, encounter.name,
                Settings.Style.GridOpacity, Settings.Style.FontSize
            );

            encounterBox.TextColorSetting(Settings.Style.Color.Text);
            encounter.SetGridBoxReference(encounterBox);
            encounter.WatchColorSettings(
                Settings.Style.Color.Cleared, 
                Settings.Style.Color.NotCleared
            );
            newList.Add(encounter);
        }
        boxes = newList;
    }

    private void ResetWatcher_DailyReset(object sender, System.DateTime e)
    {
        foreach (var model in boxes)
        {
            model.Box?.Dispose();
        }
        InitBountyEncounters();
    }

    public override void Dispose()
    {
        if (Service.ApiPollingService != null)
            Service.ApiPollingService.ApiPollingTrigger -= OnApiPollingTrigger;
        Service.ResetWatcher.DailyReset -= ResetWatcher_DailyReset;
        base.Dispose();
    }
}
