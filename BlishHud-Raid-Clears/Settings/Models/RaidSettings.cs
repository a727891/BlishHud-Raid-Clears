using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using RaidClears.Features.Raids.Services;

namespace RaidClears.Settings.Models;

public class RaidSettings
{
    public IEnumerable<SettingEntry<bool>> RaidWings { get; set; }
    public DisplayStyle Style { get; set; }
    public GenericSettings Generic { get; set; }
    
    public SettingEntry<bool> RaidPanelHighlightEmbolden { get; set; }
    public SettingEntry<bool> RaidPanelHighlightCotM { get; set; }
    public SettingEntry<bool> RaidPanelMentorProgress { get; set; }
    public SettingEntry<bool> RaidPanelMentorProgressPopup { get; set; }
    public SettingEntry<bool> RaidPanelMentorProgressPopupReposition { get; set; }
    public SettingEntry<Point> RaidPanelMentorProgressPopupPosition { get; set; }
    public SettingEntry<string> RaidPanelColorEmbolden { get; set; }
    public SettingEntry<string> RaidPanelColorCotm { get; set; }
    public SettingEntry<string> RaidPanelColorNonWeeklyBounty { get; set; }
    public SettingEntry<bool> RaidPanelHighlightNonWeeklyBounty { get; set; }
    public SettingEntry<bool> RaidPanelOmitEventEncounters { get; set; }

    public RaidSettings(SettingCollection settings)
    {
        Generic = new GenericSettings
        {
            Enabled = settings.DefineSetting(Settings.Raids.General.enabled),
            PositionLock = settings.DefineSetting(Settings.Raids.General.positionLock),
            Location = settings.DefineSetting(Settings.Raids.General.location),
            Tooltips = settings.DefineSetting(Settings.Raids.General.tooltips),
            ToolbarIcon = settings.DefineSetting(Settings.Raids.General.toolbarIcon),
            ShowHideKeyBind = settings.DefineSetting(Settings.Raids.General.keyBind),
            Visible = settings.DefineSetting(Settings.Raids.General.visible),
        };
        
        // RaidWings = Settings.Raids.Module.raidWings.Select(settings.DefineSetting).ToArray();
        
        RaidPanelHighlightEmbolden = settings.DefineSetting(Settings.Raids.Module.highlightEmbolden);
        RaidPanelHighlightCotM = settings.DefineSetting(Settings.Raids.Module.highlightCotm);
        RaidPanelHighlightNonWeeklyBounty = settings.DefineSetting(Settings.Raids.Module.highlightNonWeeklyBounty);
        RaidPanelOmitEventEncounters = settings.DefineSetting(Settings.Raids.Module.omitEventEncounters);
        RaidPanelMentorProgress = settings.DefineSetting(Settings.Raids.Module.mentorProgress);
        RaidPanelMentorProgressPopup = settings.DefineSetting(Settings.Raids.Module.mentorProgressPopup);
        RaidPanelMentorProgressPopupReposition = settings.DefineSetting(Settings.Raids.Module.mentorProgressPopupReposition);
        RaidPanelMentorProgressPopupPosition = settings.DefineSetting(Settings.Raids.Module.mentorProgressPopupPosition);
        
        Style = new DisplayStyle
        {
            Color = new DisplayColor
            {
                Background = settings.DefineSetting(Settings.Raids.Style.Color.background),
                Cleared = settings.DefineSetting(Settings.Raids.Style.Color.cleared),
                Text = settings.DefineSetting(Settings.Raids.Style.Color.text),
                NotCleared = settings.DefineSetting(Settings.Raids.Style.Color.uncleared),
            },
            
            Layout = settings.DefineSetting(Settings.Raids.Style.layout),
            FontSize = settings.DefineSetting(Settings.Raids.Style.fontSize),
            BgOpacity = settings.DefineSetting(Settings.Raids.Style.backgroundOpacity),
            GridOpacity = settings.DefineSetting(Settings.Raids.Style.gridOpacity),
            LabelDisplay = settings.DefineSetting(Settings.Raids.Style.labelDisplay),
            LabelOpacity = settings.DefineSetting(Settings.Raids.Style.labelOpacity),
        };
        Style.GridOpacity.SetRange(0.1f, 1.0f);
        Style.LabelOpacity.SetRange(0.1f, 1.0f);
        Style.BgOpacity.SetRange(0.0f, 1.0f);

        RaidPanelColorEmbolden = settings.DefineSetting(Settings.Raids.Style.Color.embolden);
        RaidPanelColorCotm = settings.DefineSetting(Settings.Raids.Style.Color.cotm);
        RaidPanelColorNonWeeklyBounty = settings.DefineSetting(Settings.Raids.Style.Color.nonWeeklyBounty);

        CleanUpOldSettings(settings);
    }

    public void CleanUpOldSettings(SettingCollection settings){
        settings.UndefineSetting("RCw1");
        settings.UndefineSetting("RCw2");
        settings.UndefineSetting("RCw3");
        settings.UndefineSetting("RCw4");
        settings.UndefineSetting("RCw5");
        settings.UndefineSetting("RCw6");
        settings.UndefineSetting("RCw7");
        settings.UndefineSetting("RCw8");
    }

    // /**
    //  * Used to convert existing settings to new settings JSON file
    //  */
    // public void ConvertToJsonFile(RaidSettingsPersistance json, RaidData raidData)
    // {
    //     foreach(var expansion in raidData.Expansions)
    //     {
    //         SetExpansionValue(json, expansion.Id, true);
    //         foreach(var wing in expansion.Wings)
    //         {
    //             if (RaidWings.Count() >= wing.Number)
    //             {
    //                 SetWingValue(json, wing.Id, RaidWings.ToArray()[wing.Number-1].Value);
    //             }
    //             else
    //             {
    //                 SetWingValue(json, wing.Id, true);
    //             }
    //             foreach(var encounter in wing.Encounters)
    //             {
    //                 SetEncounterValue(json, encounter.ApiId, true);
    //             }
    //         }
    //     }
       
    // }
    // private void SetExpansionValue(RaidSettingsPersistance json, string id, bool value)
    // {
    //     if (json.Expansions.ContainsKey(id))
    //     {
    //         json.Expansions[id] = value;
    //     }
    // }
    // private void SetWingValue(RaidSettingsPersistance json, string id, bool value)
    // {
    //     if (json.Wings.ContainsKey(id))
    //     {
    //         json.Wings[id] = value;
    //     }
    // }
    // private void SetEncounterValue(RaidSettingsPersistance json, string id, bool value)
    // {
    //     if (json.Encounters.ContainsKey(id))
    //     {
    //         json.Encounters[id] = value;
    //     }
    // }
}