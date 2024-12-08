using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Settings;
using RaidClears.Features.Raids.Services;
using RaidClears.Features.Shared.Controls;
using RaidClears.Features.Shared.Enums;
using RaidClears.Features.Shared.Models;
using RaidClears.Features.Strikes.Models;
using RaidClears.Localization;
using RaidClears.Settings.Models;
using RaidClears.Utils;

namespace RaidClears.Features.Raids.Models;

public static class WingFactory
{
    public static IEnumerable<Wing> Create(RaidPanel panel, WeeklyWings weekly)
    {
        var settings = Service.Settings.RaidSettings;
        var wings = GetWingMetaData();
        foreach(var wing in wings)
        {
            var group = new GridGroup(
                panel,
                settings.Style.Layout
            );
            group.VisiblityChanged(GetWingSelectionByIndex(wing.index, settings));
            wing.SetGridGroupReference(group);

            var labelBox = new GridBox(
                group,
                wing.shortName, wing.name,
                settings.Style.LabelOpacity, 
                settings.Style.FontSize
            );
            
            wing.SetGroupLabelReference(labelBox);
            labelBox.LayoutChange(settings.Style.Layout);
            ApplyConditionalTextColoring(labelBox, wing.index, weekly, settings);
            labelBox.LabelDisplayChange(settings.Style.LabelDisplay, (wing.index + 1).ToString(), wing.shortName);
            
            foreach (var encounter in wing.boxes)
            {
                var encounterBox = new GridBox(
                    group,
                    encounter.shortName, encounter.name,
                    settings.Style.GridOpacity, settings.Style.FontSize
                );
                encounter.SetGridBoxReference(encounterBox);
                encounter.WatchColorSettings(settings.Style.Color.Cleared, settings.Style.Color.NotCleared);
                ApplyConditionalTextColoring(encounterBox, wing.index, weekly, settings);
            }
        }

        return wings;
    }

    private static void ApplyConditionalTextColoring(GridBox box,int index, WeeklyWings weekly, RaidSettings settings)
    {
        if (index == weekly.Emboldened)
        {
            box.ConditionalTextColorSetting(settings.RaidPanelHighlightEmbolden, settings.RaidPanelColorEmbolden, settings.Style.Color.Text);
        }else if(index == weekly.CallOfTheMist || index == weekly.LatestRelease)
        {
            box.ConditionalTextColorSetting(settings.RaidPanelHighlightCotM, settings.RaidPanelColorCotm, settings.Style.Color.Text);
        }
        else
        {
            box.TextColorSetting(settings.Style.Color.Text);
        }
    }
    
    private static SettingEntry<bool> GetWingSelectionByIndex(int index, RaidSettings settings) => settings.RaidWings.ElementAt(index);

    private static List<Wing> GetWingMetaData()
    {

        List<Wing> raids = new List<Wing>();
        foreach (var expansion in Service.RaidData.Expansions)
        {
            foreach(var wing in expansion.Wings)
            {
                var encounters = new List<BoxModel>();
                foreach (var encounter in wing.Encounters)
                {
                    encounters.Add(new Encounter(encounter));
                }
                var wingModel = new Wing(wing.Name, wing.Number-1, wing.Abbriviation, encounters.ToArray());
                raids.Add(wingModel);
            }
            
        }
        return raids;
        return new List<Wing> {
            new Wing(Strings.Raid_Wing_1, 0, Strings.Raid_Wing_1_Short,
                new BoxModel[] {
                    new Encounter(Encounters.RaidBosses.ValeGuardian),
                    new Encounter(Encounters.RaidBosses.SpiritWoods),
                    new Encounter(Encounters.RaidBosses.Gorseval),
                    new Encounter(Encounters.RaidBosses.Sabetha),
                }),
            new Wing(Strings.Raid_Wing_2, 1, Strings.Raid_Wing_2_Short,
                new BoxModel[] {
                    new Encounter(Encounters.RaidBosses.Slothasor),
                    new Encounter(Encounters.RaidBosses.BanditTrio),
                    new Encounter(Encounters.RaidBosses.Matthias),
                }),
            new Wing(Strings.Raid_Wing_3, 2, Strings.Raid_Wing_3_Short,
                new BoxModel[] {
                    new Encounter(Encounters.RaidBosses.Escort),
                    new Encounter(Encounters.RaidBosses.KeepConstruct),
                    new Encounter(Encounters.RaidBosses.TwistedCastle),
                    new Encounter(Encounters.RaidBosses.Xera),
                }),
            new Wing(Strings.Raid_Wing_4, 3, Strings.Raid_Wing_4_Short,
                new BoxModel[] {
                    new Encounter(Encounters.RaidBosses.Cairn),
                    new Encounter(Encounters.RaidBosses.MursaatOverseer),
                    new Encounter(Encounters.RaidBosses.Samarog),
                    new Encounter(Encounters.RaidBosses.Deimos),
                }),
            new Wing(Strings.Raid_Wing_5, 4, Strings.Raid_Wing_5_Short,
                new BoxModel[] {
                    new Encounter(Encounters.RaidBosses.SoulessHorror),
                    new Encounter(Encounters.RaidBosses.RiverOfSouls),
                    new Encounter(Encounters.RaidBosses.StatuesOfGrenth),
                    new Encounter(Encounters.RaidBosses.VoiceInTheVoid),
                }),
            new Wing(Strings.Raid_Wing_6, 5, Strings.Raid_Wing_6_Short,
                new BoxModel[] {
                    new Encounter(Encounters.RaidBosses.ConjuredAmalgamate),
                    new Encounter(Encounters.RaidBosses.TwinLargos),
                    new Encounter(Encounters.RaidBosses.Qadim),
                }),
            new Wing(Strings.Raid_Wing_7, 6, Strings.Raid_Wing_7_Short,
                new BoxModel[] {
                    new Encounter(Encounters.RaidBosses.Gate),
                    new Encounter(Encounters.RaidBosses.Adina),
                    new Encounter(Encounters.RaidBosses.Sabir),
                    new Encounter(Encounters.RaidBosses.QadimThePeerless),
                }),
            new Wing(Strings.Raid_Wing_8, 7, Strings.Raid_Wing_8_Short,
                new BoxModel[] {
                    new Encounter(Encounters.RaidBosses.Camp),
                    new Encounter(Encounters.RaidBosses.Decima),
                    new Encounter(Encounters.RaidBosses.Greer),
                    new Encounter(Encounters.RaidBosses.Ura),
                })
        };
    }
}
public class Wing : GroupModel
{ 
    public Wing(string name, int index, string shortName, IEnumerable<BoxModel> boxes) : base(name, index, shortName, boxes)
    {
    }
}
