using Blish_HUD.Settings;
using RaidClears.Features.Raids.Models;
using RaidClears.Features.Shared.Controls;
using RaidClears.Features.Shared.Enums;
using RaidClears.Features.Shared.Models;
using RaidClears.Localization;
using RaidClears.Settings.Models;
using RaidClears.Utils;
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
        var strikeIndex = 0;
        foreach (var strike in strikes)
        {
            var group = new GridGroup(
                panel,
                settings.Style.Layout
            );
            group.VisiblityChanged(GetStrikeGroupVisibleSettingByIndex(strike.index));
            strike.SetGridGroupReference(group);

            var labelBox = new GridBox(
                group,
                strike.shortName, strike.name,
                settings.Style.LabelOpacity, settings.Style.FontSize
            );
            strike.SetGroupLabelReference(labelBox);
            labelBox.LayoutChange(settings.Style.Layout);
            labelBox.LabelDisplayChange(settings.Style.LabelDisplay, strike.shortName, strike.shortName);

            var allStrikes = settings.IbsMissions.Concat(settings.EodMissions).ToArray();

            foreach (var index in Enumerable.Range(0, strike.boxes.Count()))
            {
                var encounter = strike.boxes.ToArray()[index];

                var encounterBox = new GridBox(
                    group,
                    encounter.shortName, encounter.name,
                    settings.Style.GridOpacity, settings.Style.FontSize
                );
                if (strikeIndex < allStrikes.Length)
                {
                    encounterBox.VisiblityChanged(allStrikes[strikeIndex++]);
                }
                encounterBox.TextColorSetting(settings.Style.Color.Text);
                encounter.SetGridBoxReference(encounterBox);
                encounter.WatchColorSettings(settings.Style.Color.Cleared, settings.Style.Color.NotCleared);
            }
        }

        strikes.Add(new PriorityStrikes(Strings.StrikeGroup_Priority, 10, Strings.StrikeGroup_Priority_abbr, new List<BoxModel>() { }, panel));


        return strikes;
    }

    private static IEnumerable<Strike> GetStrikeMetaData()
    {
        return new List<Strike>() {
            new Strike(Strings.StrikeGroup_Icebrood, 8, Strings.StrikeGroup_Icebrood_abbr,
                new List<BoxModel>() {
                    new Encounter(Encounters.StrikeMission.ShiverpeaksPass),
                    new Encounter(Encounters.StrikeMission.Fraenir),
                    new Encounter(Encounters.StrikeMission.VoiceAndClaw),
                    new Encounter(Encounters.StrikeMission.Whisper),
                    new Encounter(Encounters.StrikeMission.Boneskinner),
                    new Encounter(Encounters.StrikeMission.ColdWar),
                }),
            new Strike(Strings.StrikeGroup_EoD, 9, Strings.StrikeGroup_Eod_abbr,
                new List<BoxModel>() {
                    new Encounter(Encounters.StrikeMission.AetherbladeHideout),
                    new Encounter(Encounters.StrikeMission.Junkyard),
                    new Encounter(Encounters.StrikeMission.Overlook),
                    new Encounter(Encounters.StrikeMission.HarvestTemple),
                    new Encounter(Encounters.StrikeMission.OldLionsCourt),
                }),
           // new PriorityStrikes("Priority Strike Missions (Daily)", 10, "PS", new List<BoxModel>() { }),
        };
    }

    public static SettingEntry<bool> GetStrikeGroupVisibleSettingByIndex(int id)
    {
        return id switch
        {
            8 => Settings.StrikeVisibleIbs,
            9 => Settings.StrikeVisibleEod,
            10 => Settings.StrikeVisiblePriority,
            _ => Settings.StrikeVisiblePriority
        };
    }
}