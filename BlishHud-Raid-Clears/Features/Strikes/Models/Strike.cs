using RaidClears.Features.Raids.Models;
using RaidClears.Features.Shared.Controls;
using RaidClears.Features.Strikes.Services;
using RaidClears.Settings.Services;
using RaidClears.Utils;


namespace RaidClears.Features.Strikes.Models
{

    public static class StrikeFactory
    {
        public static Strike[] Create(StrikesPanel panel)
        {
            SettingService settings = Module.ModuleInstance.SettingsService;
            Strike[] strikes = GetStrikeMetaData();
            foreach(var strike in strikes)
            {
                GridGroup group = new GridGroup(
                    panel,
                    settings.StrikePanelLayout
                );
                group.VisiblityChanged(StrikeSettingService.GetStrikeGroupVisibleSettingByIndex(strike.index));
                strike.SetGridGroupReference(group);


                GridBox labelBox = new GridBox(
                    group,
                    strike.shortName, strike.name,
                    settings.StrikePanelLabelOpacity, settings.StrikePanelFontSize
                );
                strike.SetGroupLabelReference(labelBox);
                labelBox.LayoutChange(settings.StrikePanelLayout);
                labelBox.LabelDisplayChange(settings.StrikePanelLabelDisplay, strike.shortName, strike.shortName);
                
                foreach (var encounter in strike.boxes)
                {
                    GridBox encounterBox = new GridBox(
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
            return new Strike[] {
                new Strike("Icebrood Saga", 8, "IBS",
                    new Encounter[] {
                        new Encounter("cold_war","Cold War", "CW"),
                        new Encounter("fraenir_of_jormag", "Fraenir of Jormag", "FoJ"),
                        new Encounter("shiverpeak_pass", "Shiverpeaks Pass", "SP"),
                        new Encounter("voice_and_claw", "Voice and Claw of the Fallen", "Bear"),
                        new Encounter("whisper_of_jormag", "Whisper of Jormag", "WoJ"),
                        new Encounter("boneskinner", "Boneskinner", "BS"),
                    }),
                new Strike("End of Dragons", 9, "EoD",
                    new Encounter[] {
                        new Encounter("aetherblade_hideout","Aetherblade Hideout", "AH"),
                        new Encounter("xunlai_jade_junkyard", "Xunlai Jade Junkyard", "XJJ"),
                        new Encounter("kaineng_overlook", "Kaineng Overlook", "KO"),
                        new Encounter("harvest_temple", "Harvest Temple", "HT"),
                        new Encounter("old_lion_court", "Old Lion's Court", "OLC"),
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
}
