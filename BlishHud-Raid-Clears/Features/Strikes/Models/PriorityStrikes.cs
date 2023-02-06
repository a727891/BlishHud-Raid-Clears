using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Settings;
using RaidClears.Features.Raids.Models;
using RaidClears.Features.Shared.Controls;
using RaidClears.Features.Shared.Enums;
using RaidClears.Features.Shared.Models;
using RaidClears.Features.Strikes.Services;
using RaidClears.Settings.Models;
using RaidClears.Utils;

namespace RaidClears.Features.Strikes.Models;

public class PriorityStrikes : Strike
{
    private readonly StrikeSettings settings = Service.Settings.StrikeSettings;
    private static StrikeSettings Settings => Service.Settings.StrikeSettings;
    public PriorityStrikes(string name, int index, string shortName, IEnumerable<BoxModel> boxes, Container panel) : base(name, index, shortName, boxes)
    {
        Service.ResetWatcher.DailyReset += ResetWatcher_DailyReset;
        InitGroup(panel);
        InitPriorityStrikes();


    }
    protected void InitGroup(Container panel)
    {
        var group = new GridGroup(
                panel,
                settings.Style.Layout
            );
        group.VisiblityChanged(settings.StrikeVisiblePriority);
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
    protected void InitPriorityStrikes()
    {

        var dailies = PriorityRotationService.GetPriorityStrikes();
        var newList = new List<BoxModel>();
        foreach (var daily in dailies)
        {
            var encounter = daily.Encounter;
            var encounterBox = new GridBox(
                this.GridGroup,
                encounter.shortName, encounter.name,
                Settings.Style.GridOpacity, Settings.Style.FontSize
            );

            encounterBox.TextColorSetting(Settings.Style.Color.Text);
            encounter.SetGridBoxReference(encounterBox);
            encounter.WatchColorSettings(Settings.Style.Color.Cleared, Settings.Style.Color.NotCleared);
            newList.Add(encounter);
        }
        boxes = newList;
    }

    private void ResetWatcher_DailyReset(object sender, System.DateTime e)
    {
        foreach (var model in boxes)
        {
            model.Box.Dispose();
        }
        InitPriorityStrikes();
    }

    public override void Dispose()
    {
        Service.ResetWatcher.DailyReset -= ResetWatcher_DailyReset;
    }


}