using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Settings;

namespace RaidClears.Settings.Models;

public class DungeonSettings
{
    public SettingEntry<bool> DungeonHighlightFrequenter { get; set; }
    public IEnumerable<SettingEntry<bool>> DungeonPaths { get; set; }
    public SettingEntry<bool> DungeonFrequenterVisible { get; set; }
    public SettingEntry<string> DungeonPanelColorFreq { get; set; }
    public DisplayStyle Style { get; set; }
    public GenericSettings Generic { get; set; }

    public DungeonSettings(SettingCollection settings)
    {
        Generic = new GenericSettings
        {
            Location = settings.DefineSetting(Settings.Dungeons.General.location),
            Enabled = settings.DefineSetting(Settings.Dungeons.General.enable),
            Tooltips = settings.DefineSetting(Settings.Dungeons.General.tooltips),
            ToolbarIcon = settings.DefineSetting(Settings.Dungeons.General.toolbarIcon),
            Visible = settings.DefineSetting(Settings.Dungeons.General.visible),
            PositionLock = settings.DefineSetting(Settings.Dungeons.General.positionLock),
            ShowHideKeyBind = settings.DefineSetting(Settings.Dungeons.General.keyBind),
        };

        DungeonHighlightFrequenter = settings.DefineSetting(Settings.Dungeons.Module.highlightFrequenter);
        DungeonPaths = Settings.Dungeons.Module.dungeonPaths.Select(settings.DefineSetting);
        DungeonFrequenterVisible = settings.DefineSetting(Settings.Dungeons.Module.dungeonFrequenterEnabled);
        
        Style = new DisplayStyle
        {
            Color = new DisplayColor
            {
                Background = settings.DefineSetting(Settings.Dungeons.Style.Color.background),
                Cleared = settings.DefineSetting(Settings.Dungeons.Style.Color.cleared),
                Text = settings.DefineSetting(Settings.Dungeons.Style.Color.text),
                NotCleared = settings.DefineSetting(Settings.Dungeons.Style.Color.uncleared),
            },
            
            FontSize = settings.DefineSetting(Settings.Dungeons.Style.fontSize),
            Layout = settings.DefineSetting(Settings.Dungeons.Style.layout),
            LabelOpacity = settings.DefineSetting(Settings.Dungeons.Style.labelOpacity),
            BgOpacity = settings.DefineSetting(Settings.Dungeons.Style.backgroundOpacity),
            GridOpacity = settings.DefineSetting(Settings.Dungeons.Style.gridOpacity),
            LabelDisplay = settings.DefineSetting(Settings.Dungeons.Style.labelDisplay),
        };
        Style.GridOpacity.SetRange(0.1f, 1.0f);
        Style.LabelOpacity.SetRange(0.1f, 1.0f);
        Style.BgOpacity.SetRange(0.0f, 1.0f);
        Style.LabelDisplay.SetExcluded(Enums.LabelDisplay.WingNumber);

        DungeonPanelColorFreq = settings.DefineSetting(Settings.Dungeons.Style.Color.frequenter);
    }
}