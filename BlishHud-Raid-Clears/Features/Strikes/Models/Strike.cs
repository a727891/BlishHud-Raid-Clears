using System.Linq;
using RaidClears.Features.Raids.Models;
using RaidClears.Features.Shared.Controls;
using RaidClears.Features.Shared.Enums;
using RaidClears.Features.Shared.Models;
using RaidClears.Features.Strikes.Services;
using RaidClears.Utils;

namespace RaidClears.Features.Strikes.Models;

public static class StrikeFactory
{
    public static Strike[] Create(StrikesPanel panel)
    {
        var settings = Module.moduleInstance.SettingsService.StrikeSettings;
        var strikes = GetStrikeMetaData();
        foreach(var strike in strikes)
        {
            var group = new GridGroup(
                panel,
                settings.Style.Layout
            );
            group.VisiblityChanged(StrikeSettingService.GetStrikeGroupVisibleSettingByIndex(strike.index));
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

            foreach (var index in Enumerable.Range(0, strike.boxes.Length))
            {
                var encounter = strike.boxes[index];
                
                var encounterBox = new GridBox(
                    group,
                    encounter.shortName, encounter.name,
                    settings.Style.GridOpacity, settings.Style.FontSize
                );
                
                encounterBox.VisiblityChanged(allStrikes[index]);
                encounterBox.TextColorSetting(settings.Style.Color.Text);
                encounter.SetGridBoxReference(encounterBox);
                encounter.WatchColorSettings(settings.Style.Color.Cleared, settings.Style.Color.NotCleared);
            }
        }
        
        return strikes;
    }

    private static Strike[] GetStrikeMetaData()
    {
        return new[] {
            new Strike("Icebrood Saga", 8, "IBS",
                new BoxModel[] {
                    new Encounter(Encounters.StrikeMission.ColdWar),
                    new Encounter(Encounters.StrikeMission.Fraenir),
                    new Encounter(Encounters.StrikeMission.ShiverpeaksPass),
                    new Encounter(Encounters.StrikeMission.VoiceAndClaw),
                    new Encounter(Encounters.StrikeMission.Whisper),
                    new Encounter(Encounters.StrikeMission.Boneskinner),
                }),
            new Strike("End of Dragons", 9, "EoD",
                new BoxModel[] {
                    new Encounter(Encounters.StrikeMission.AetherbladeHideout),
                    new Encounter(Encounters.StrikeMission.Junkyard),
                    new Encounter(Encounters.StrikeMission.Overlook),
                    new Encounter(Encounters.StrikeMission.HarvestTemple),
                    new Encounter(Encounters.StrikeMission.OldLionsCourt),
                }),
            new Strike("Priority", 10, "PS",
                new BoxModel[] {
                    
                }),
        };
    }
}
public class Strike : Wing
{ 
    public Strike(string name, int index, string shortName, BoxModel[] boxes) : base(name, index, shortName, boxes)
    {
        
    }
}
