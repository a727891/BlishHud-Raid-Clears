using Blish_HUD.Controls;
using Blish_HUD.Settings;
using RaidClears.Features.Raids.Models;
using RaidClears.Features.Shared.Controls;
using RaidClears.Features.Shared.Enums;
using RaidClears.Features.Shared.Models;
using RaidClears.Localization;
using RaidClears.Settings.Models;
using RaidClears.Utils;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaidClears.Features.Strikes.Models;

public static class StrikeMetaData
{
    private static StrikeSettings Settings => Service.Settings.StrikeSettings;

    public static IEnumerable<Strike> Create(StrikesPanel panel)
    {
        var settings = Service.Settings.StrikeSettings;
        var strikes = GetStrikeMetaData().ToList();
        foreach (var strike in strikes)
        {
            var group = new GridGroup(
                panel,
                settings.Style.Layout
            );
            //group.VisiblityChanged(Service.StrikeSettings.Expansions[strike.shortName]);
            group.VisiblityChanged(Service.StrikeData.GetExpansionVisible(Service.StrikeData.GetExpansionStrikesByName(strike.name)));
            strike.SetGridGroupReference(group);

            var labelBox = new GridBox(
                group,
                strike.shortName, strike.name,
                settings.Style.LabelOpacity, settings.Style.FontSize
            );
            strike.SetGroupLabelReference(labelBox);
            labelBox.LayoutChange(settings.Style.Layout);
            labelBox.LabelDisplayChange(settings.Style.LabelDisplay, strike.shortName, strike.shortName);

            //var allStrikes = settings.IbsMissions.Concat(settings.EodMissions).Concat(settings.SotOMissions).ToArray();

            foreach (var index in Enumerable.Range(0, strike.boxes.Count()))
            {
                var encounter = strike.boxes.ToArray()[index];

                var encounterBox = new GridBox(
                    group,
                    encounter.shortName, encounter.name,
                    settings.Style.GridOpacity, settings.Style.FontSize
                );
                /*if (strikeIndex < allStrikes.Length)
                {
                    encounterBox.VisiblityChanged(allStrikes[strikeIndex++]);
                }*/
                encounterBox.VisiblityChanged(Service.StrikeData.GetMissionVisible(Service.StrikeData.GetStrikeMissionByName(encounter.name)));
                encounterBox.TextColorSetting(settings.Style.Color.Text);
                encounter.SetGridBoxReference(encounterBox);
                encounter.WatchColorSettings(settings.Style.Color.Cleared, settings.Style.Color.NotCleared);
            }
        }

        strikes.Add(new PriorityStrikes(Strings.StrikeGroup_Priority, Strings.StrikeGroup_Priority_abbr, 11, Strings.StrikeGroup_Priority_abbr, new List<BoxModel>() { }, panel));


        return strikes;
    }

    private static IEnumerable<Strike> GetStrikeMetaData()
    {
        List<Strike> strikes = new List<Strike>();
        foreach(var expansion in Service.StrikeData.Expansions)
        {
            strikes.Add(new Strike(expansion));
        }
        return strikes;
    }
}