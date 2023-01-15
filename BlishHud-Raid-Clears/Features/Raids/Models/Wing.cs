
using Blish_HUD.Controls;
using Blish_HUD.Settings;
using RaidClears.Features.Shared.Controls;
using RaidClears.Raids.Services;
using RaidClears.Settings;
using RaidClears.Utils;
using System.Net.NetworkInformation;

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
                    settings.RaidPanelLabelOpacity, settings.RaidPanelLayout, settings.RaidPanelFontSize
                );
                wing.SetGroupLabelReference(labelBox);
                ApplyConditionalTextColoring(labelBox, wing.index, weekly, settings);
                labelBox.LabelDisplayChange(settings.RaidPanelLabelDisplay, (wing.index + 1).ToString(), wing.shortName);
                
                foreach (var encounter in wing.boxes)
                {
                    GridBox encounterBox = new GridBox(
                        group,
                        encounter.short_name, encounter.name,
                        settings.RaidPanelGridOpacity, settings.RaidPanelLayout, settings.RaidPanelFontSize
                    );
                    encounter.SetGridBoxReference(encounterBox);
                    encounter.WatchColorSettings(settings.RaidPanelColorCleared, settings.RaidPanelColorNotCleared);
                    ApplyConditionalTextColoring(encounterBox, wing.index, weekly, settings);
                    
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
                new Wing("Spirit Vale", 0, "SV",
                    new Encounter[] {
                        new Encounter("vale_guardian", "Vale Guardian", "VG"),
                        new Encounter("spirit_woods", "Spirit Run", "SR"),
                        new Encounter("gorseval", "Gorseval", "G"),
                        new Encounter("sabetha", "Sabetha", "S"),
                    }),
                new Wing("Salvation Pass", 1, "SP",
                    new Encounter[] {
                        new Encounter("slothasor", "Slothasor", "S"),
                        new Encounter("bandit_trio", "Bandit Trio", "B3"),
                        new Encounter("matthias", "Matthias Gabrel", "M"),
                    }),
                new Wing("Stronghold of the Faithful", 2, "SotF",
                    new Encounter[] {
                        new Encounter("escort", "Escort", "E"),
                        new Encounter("keep_construct", "Keep Construct", "KC"),
                        new Encounter("twisted_castle", "Twisted Castel", "TC"),
                        new Encounter("xera", "Xera", "X"),
                    }),
                new Wing("Bastion of the Penitent", 3, "BotP",
                    new Encounter[] {
                        new Encounter("cairn", "Cairn the Indominable", "C"),
                        new Encounter("mursaat_overseer", "Mursaat Overseer", "MO"),
                        new Encounter("samarog", "Samarog", "S"),
                        new Encounter("deimos", "Deimos", "D"),
                    }),
                new Wing("Hall of Chains", 4, "HoC",
                    new Encounter[] {
                        new Encounter("soulless_horror", "Soulless Horror", "SH"),
                        new Encounter("river_of_souls", "River of Souls", "R"),
                        new Encounter("statues_of_grenth", "Statues of Grenth", "S"),
                        new Encounter("voice_in_the_void", "Dhuum", "D"),
                    }),
                new Wing("Mythwright Gambit", 5, "MG",
                    new Encounter[] {
                        new Encounter("conjured_amalgamate", "Conjured Amalgamate", "CA"),
                        new Encounter("twin_largos", "Twin Largos", "TL"),
                        new Encounter("qadim", "Qadim", "Q1"),
                    }),
                new Wing("The Key of Ahdashim", 6, "TKoA",
                    new Encounter[] {
                        new Encounter("gate", "Gate", "G"),
                        new Encounter("adina", "Cardinal Adina", "A"),
                        new Encounter("sabir", "Cardinal Sabir", "S"),
                        new Encounter("qadim_the_peerless", "Qadim the Peerless", "Q2"),
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
