using System;
using System.Collections.Generic;
using Blish_HUD.Settings;
using RaidClears.Features.Shared.Controls;
using RaidClears.Features.Shared.Models;
using RaidClears.Settings.Models;
using RaidClears.Utils;

namespace RaidClears.Features.Raids.Models;

public static class WingFactory
{
    public static IEnumerable<Wing> Create(RaidPanel panel)
    {
        var settings = Service.Settings.RaidSettings;
        var wings = GetWingMetaData();
        foreach (var wing in wings)
        {
            var group = new GridGroup(
                panel,
                settings.Style.Layout
            );
            var weeklyModifier = new WeeklyModifier(Service.RaidData.GetRaidWingByZeroIndex(wing.index));
            group.VisiblityChanged(GetWingSelectionByIndex(wing.index));
            wing.SetGridGroupReference(group);

            var labelBox = new GridBox(
                group,
                wing.shortName, wing.name,
                settings.Style.LabelOpacity,
                settings.Style.FontSize
            );
            

            wing.SetGroupLabelReference(labelBox);
            labelBox.LayoutChange(settings.Style.Layout);
            ApplyConditionalTextColoring(labelBox, weeklyModifier, settings);
            labelBox.LabelDisplayChange(settings.Style.LabelDisplay, (wing.index + 1).ToString(), wing.shortName);

            foreach (var encounter in wing.boxes)
            {
                var encounterBox = new GridBox(
                    group,
                    encounter.shortName, encounter.name,
                    settings.Style.GridOpacity, settings.Style.FontSize
                );
                encounterBox.VisiblityChanged(Service.RaidSettings.GetEncounterVisibleByApiId(encounter.id));
                encounter.SetGridBoxReference(encounterBox);
                encounter.WatchColorSettings(settings.Style.Color.Cleared, settings.Style.Color.NotCleared);
                ApplyConditionalTextColoring(encounterBox, weeklyModifier, settings);
            }
        }

        return wings;
    }

    private static void ApplyConditionalTextColoring(GridBox box, WeeklyModifier weekly, RaidSettings settings)
    {
        if (weekly.Emboldened)
        {
            box.ConditionalTextColorSetting(settings.RaidPanelHighlightEmbolden, settings.RaidPanelColorEmbolden, settings.Style.Color.Text);
        }
        else if (weekly.CallOfTheMist)
        {
            box.ConditionalTextColorSetting(settings.RaidPanelHighlightCotM, settings.RaidPanelColorCotm, settings.Style.Color.Text);
        }
        else
        {
            box.TextColorSetting(settings.Style.Color.Text);
        }
    }

    private static SettingEntry<bool> GetWingSelectionByIndex(int index) {
        var raidWing = Service.RaidData.GetRaidWingByZeroIndex(index);
        return Service.RaidSettings.GetWingVisible(raidWing);
    }

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
                var wingModel = new Wing(wing.Name, wing.Id, wing.Number-1, wing.Abbriviation, encounters.ToArray());
                raids.Add(wingModel);
            }
            
        }
        return raids;
    }
}

