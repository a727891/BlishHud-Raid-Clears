using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Settings;
using RaidClears.Features.Raids.Models;
using RaidClears.Features.Shared.Controls;
using RaidClears.Features.Shared.Enums;
using RaidClears.Features.Shared.Models;
using RaidClears.Features.Fractals.Services;
using RaidClears.Settings.Models;
using RaidClears.Utils;

namespace RaidClears.Features.Fractals.Models;

public class TierNTomorrow : Fractal
{
    private readonly FractalSettings settings = Service.Settings.FractalSettings;
    private static FractalSettings Settings => Service.Settings.FractalSettings;
    public TierNTomorrow(string name, int index, string shortName, IEnumerable<BoxModel> boxes, Container panel) : base(name, index, shortName, boxes)
    {
        Service.ResetWatcher.DailyReset += ResetWatcher_DailyReset;
        InitGroup(panel);
        InitTierNFractals();


    }
    protected void InitGroup(Container panel)
    {
        var group = new GridGroup(
                panel,
                settings.Style.Layout
            );
        group.VisiblityChanged(settings.TomorrowTierN);
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
    protected void InitTierNFractals()
    {

        var dailies = DailyTierNFractalService.GetTomorrowTierN();
        var newList = new List<BoxModel>();
        foreach (var encounter in dailies)
        {
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
        InitTierNFractals();
    }

    public override void Dispose()
    {
        Service.ResetWatcher.DailyReset -= ResetWatcher_DailyReset;
    }


}