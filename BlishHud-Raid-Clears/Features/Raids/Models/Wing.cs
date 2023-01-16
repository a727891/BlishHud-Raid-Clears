using Blish_HUD.Settings;
using RaidClears.Features.Shared.Controls;
using RaidClears.Features.Shared.Models;
using RaidClears.Localization;
using RaidClears.Raids.Services;
using RaidClears.Settings;
using RaidClears.Utils;


namespace RaidClears.Features.Raids.Models
{

    public static class WingFactory
    {
        public static Wing[] Create(RaidPanel panel, WeeklyWings weekly)
        {
            SettingService settings = Module.ModuleInstance.SettingsService;
            Wing[] wings = GetWingMetaData();
            foreach(var wing in wings)
            {
                GridGroup group = new GridGroup(
                    panel,
                    settings.RaidPanelLayout
                );
                group.VisiblityChanged(GetWingSelectionByIndex(wing.index, settings));
                wing.SetGridGroupReference(group);


                GridBox labelBox = new GridBox(
                    group,
                    wing.shortName, wing.name,
                    settings.RaidPanelLabelOpacity, settings.RaidPanelFontSize
                );
                wing.SetGroupLabelReference(labelBox);
                labelBox.LayoutChange(settings.RaidPanelLayout);
                ApplyConditionalTextColoring(labelBox, wing.index, weekly, settings);
                labelBox.LabelDisplayChange(settings.RaidPanelLabelDisplay, (wing.index + 1).ToString(), wing.shortName);
                
                foreach (var encounter in wing.boxes)
                {
                    GridBox encounterBox = new GridBox(
                        group,
                        encounter.short_name, encounter.name,
                        settings.RaidPanelGridOpacity, settings.RaidPanelFontSize
                    );
                    encounter.SetGridBoxReference(encounterBox);
                    encounter.WatchColorSettings(settings.RaidPanelColorCleared, settings.RaidPanelColorNotCleared);
                    ApplyConditionalTextColoring(encounterBox, wing.index, weekly, settings);
                    
                }
                                
            }

            return wings;
        }
        public static Wing[] CreateStrikes(RaidPanel panel)
        {
            SettingService settings = Module.ModuleInstance.SettingsService;
            Wing[] wings = GetWingMetaData();
            foreach (var wing in wings)
            {
                GridGroup group = new GridGroup(
                    panel,
                    settings.RaidPanelLayout
                );
                group.VisiblityChanged(GetWingSelectionByIndex(wing.index, settings));
                wing.SetGridGroupReference(group);


                GridBox labelBox = new GridBox(
                    group,
                    wing.shortName, wing.name,
                    settings.RaidPanelLabelOpacity, settings.RaidPanelFontSize
                );
                wing.SetGroupLabelReference(labelBox);
                labelBox.LayoutChange(settings.RaidPanelLayout);
                labelBox.LabelDisplayChange(settings.RaidPanelLabelDisplay, (wing.index + 1).ToString(), wing.shortName);

                foreach (var encounter in wing.boxes)
                {
                    GridBox encounterBox = new GridBox(
                        group,
                        encounter.short_name, encounter.name,
                        settings.RaidPanelGridOpacity, settings.RaidPanelFontSize
                    );
                    encounter.SetGridBoxReference(encounterBox);
                    encounter.WatchColorSettings(settings.RaidPanelColorCleared, settings.RaidPanelColorNotCleared);

                }

            }

            return wings;
        }

        public static void ApplyConditionalTextColoring(GridBox box,int index, WeeklyWings weekly, SettingService settings)
        {
            if (index == weekly.Emboldened)
            {
                box.ConditionalTextColorSetting(settings.RaidPanelHighlightEmbolden, settings.RaidPanelColorEmbolden, settings.RaidPanelColorText);
            }else if(index == weekly.CallOfTheMist)
            {
                box.ConditionalTextColorSetting(settings.RaidPanelHighlightCotM, settings.RaidPanelColorCotm, settings.RaidPanelColorText);
            }
            else
            {
                box.TextColorSetting(settings.RaidPanelColorText);
            }
        }
        public static SettingEntry<bool> GetWingSelectionByIndex(int index, SettingService settings)
        {
            switch (index)
            {
                case 0: return settings.W1IsVisible;
                case 1: return settings.W2IsVisible;
                case 2: return settings.W3IsVisible;
                case 3: return settings.W4IsVisible;
                case 4: return settings.W5IsVisible;
                case 5: return settings.W6IsVisible;
                case 6: return settings.W7IsVisible;
                default : return settings.W1IsVisible;
            }
        }

        public static Wing[] GetWingMetaData()
        {
            return new Wing[] {
                new Wing(Strings.Raid_Wing_1, 0, Strings.Raid_Wing_1_Short,
                    new Encounter[] {
                        new Encounter("vale_guardian",Strings.Raid_Wing_1_1_Name, Strings.Raid_Wing_1_1_Short),
                        new Encounter("spirit_woods", Strings.Raid_Wing_1_2_Name, Strings.Raid_Wing_1_2_Short),
                        new Encounter("gorseval", Strings.Raid_Wing_1_3_Name, Strings.Raid_Wing_1_3_Short),
                        new Encounter("sabetha", Strings.Raid_Wing_1_4_Name, Strings.Raid_Wing_1_4_Short),
                    }),
                new Wing(Strings.Raid_Wing_2, 1, Strings.Raid_Wing_2_Short,
                    new Encounter[] {
                        new Encounter("slothasor", Strings.Raid_Wing_2_1_Name, Strings.Raid_Wing_2_1_Short),
                        new Encounter("bandit_trio", Strings.Raid_Wing_2_2_Name, Strings.Raid_Wing_2_2_Short),
                        new Encounter("matthias", Strings.Raid_Wing_2_3_Name, Strings.Raid_Wing_2_3_Short),
                    }),
                new Wing(Strings.Raid_Wing_3, 2, Strings.Raid_Wing_3_Short,
                    new Encounter[] {
                        new Encounter("escort", Strings.Raid_Wing_3_1_Name, Strings.Raid_Wing_3_1_Short),
                        new Encounter("keep_construct", Strings.Raid_Wing_3_2_Name, Strings.Raid_Wing_3_2_Short),
                        new Encounter("twisted_castle", Strings.Raid_Wing_3_3_Name, Strings.Raid_Wing_3_3_Short),
                        new Encounter("xera", Strings.Raid_Wing_3_4_Name, Strings.Raid_Wing_3_4_Short),
                    }),
                new Wing(Strings.Raid_Wing_4, 3, Strings.Raid_Wing_4_Short,
                    new Encounter[] {
                        new Encounter("cairn", Strings.Raid_Wing_4_1_Name, Strings.Raid_Wing_4_1_Short),
                        new Encounter("mursaat_overseer", Strings.Raid_Wing_4_2_Name, Strings.Raid_Wing_4_2_Short),
                        new Encounter("samarog", Strings.Raid_Wing_4_3_Name, Strings.Raid_Wing_4_3_Short),
                        new Encounter("deimos", Strings.Raid_Wing_4_4_Name, Strings.Raid_Wing_4_4_Short),
                    }),
                new Wing(Strings.Raid_Wing_5, 4, Strings.Raid_Wing_5_Short,
                    new Encounter[] {
                        new Encounter("soulless_horror", Strings.Raid_Wing_5_1_Name, Strings.Raid_Wing_5_1_Short),
                        new Encounter("river_of_souls", Strings.Raid_Wing_5_2_Name, Strings.Raid_Wing_5_2_Short),
                        new Encounter("statues_of_grenth", Strings.Raid_Wing_5_3_Name, Strings.Raid_Wing_5_3_Short),
                        new Encounter("voice_in_the_void", Strings.Raid_Wing_5_4_Name, Strings.Raid_Wing_5_4_Short),
                    }),
                new Wing(Strings.Raid_Wing_6, 5, Strings.Raid_Wing_6_Short,
                    new Encounter[] {
                        new Encounter("conjured_amalgamate", Strings.Raid_Wing_6_1_Name, Strings.Raid_Wing_6_1_Short),
                        new Encounter("twin_largos", Strings.Raid_Wing_6_2_Name, Strings.Raid_Wing_6_2_Short),
                        new Encounter("qadim", Strings.Raid_Wing_6_3_Name, Strings.Raid_Wing_6_3_Short),
                    }),
                new Wing(Strings.Raid_Wing_7, 6, Strings.Raid_Wing_7_Short,
                    new Encounter[] {
                        new Encounter("gate", Strings.Raid_Wing_7_1_Name, Strings.Raid_Wing_7_1_Short),
                        new Encounter("adina", Strings.Raid_Wing_7_2_Name, Strings.Raid_Wing_7_2_Short),
                        new Encounter("sabir", Strings.Raid_Wing_7_3_Name, Strings.Raid_Wing_7_3_Short),
                        new Encounter("qadim_the_peerless", Strings.Raid_Wing_7_4_Name, Strings.Raid_Wing_7_4_Short),
                    })
            };
        }
    }
    public class Wing : GroupModel
    { 
        public Wing(string name, int index, string shortName, BoxModel[] boxes) : base(name, index, shortName, boxes)
        {
        }

        
    }
}
