using RaidClears.Features.Raids.Models;
using RaidClears.Features.Shared.Controls;
using RaidClears.Features.Shared.Enums;
using RaidClears.Features.Strikes.Services;
using RaidClears.Utils;


namespace RaidClears.Features.Strikes.Models;


public static class StrikeFactory
{
    public static Strike[] Create(StrikesPanel panel)
    {
        var settings = Module.ModuleInstance.SettingsService;
        var strikes = GetStrikeMetaData();
        foreach(var strike in strikes)
        {
            var group = new GridGroup(
                panel,
                settings.StrikePanelLayout
            );
            group.VisiblityChanged(StrikeSettingService.GetStrikeGroupVisibleSettingByIndex(strike.index));
            strike.SetGridGroupReference(group);


            var labelBox = new GridBox(
                group,
                strike.shortName, strike.name,
                settings.StrikePanelLabelOpacity, settings.StrikePanelFontSize
            );
            strike.SetGroupLabelReference(labelBox);
            labelBox.LayoutChange(settings.StrikePanelLayout);
            labelBox.LabelDisplayChange(settings.StrikePanelLabelDisplay, strike.shortName, strike.shortName);
            
            foreach (var encounter in strike.boxes)
            {
                var encounterBox = new GridBox(
                    group,
                    encounter.short_name, encounter.name,
                    settings.StrikePanelGridOpacity, settings.StrikePanelFontSize
                );
                encounterBox.VisiblityChanged(StrikeSettingService.GetStrikeVisibleFromEncounterId(encounter.id)) ;
                encounterBox.TextColorSetting(settings.StrikePanelColorText);
                encounter.SetGridBoxReference(encounterBox);
                encounter.WatchColorSettings(settings.StrikePanelColorCleared, settings.StrikePanelColorNotCleared);
                
            }
                            
        }

        return strikes;
    }
    public static Strike[] GetStrikeMetaData()
    {
        return new[] {
            new Strike("Icebrood Saga", 8, "IBS",
                new[] {
                    new Encounter(Encounters.StrikeMission.ColdWar),
                    new Encounter(Encounters.StrikeMission.Fraenir),
                    new Encounter(Encounters.StrikeMission.ShiverpeaksPass),
                    new Encounter(Encounters.StrikeMission.VoiceAndClaw),
                    new Encounter(Encounters.StrikeMission.Whisper),
                    new Encounter(Encounters.StrikeMission.Boneskinner),
                }),
            new Strike("End of Dragons", 9, "EoD",
                new[] {
                    new Encounter(Encounters.StrikeMission.AetherbladeHideout),
                    new Encounter(Encounters.StrikeMission.Junkyard),
                    new Encounter(Encounters.StrikeMission.Overlook),
                    new Encounter(Encounters.StrikeMission.HarvestTemple),
                    new Encounter(Encounters.StrikeMission.OldLionsCourt),
                }),
            new Strike("Priority", 10, "PS",
                new Encounter[] {
                    
                }),
           
        };
    }
}
public class Strike : Wing
{ 
    public Strike(string name, int index, string shortName, Encounter[] boxes) : base(name, index, shortName, boxes)
    {
    }

    
}
