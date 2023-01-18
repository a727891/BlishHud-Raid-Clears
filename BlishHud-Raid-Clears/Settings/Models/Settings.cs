using System;
using Blish_HUD;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using RaidClears.Features.Shared.Enums;
using RaidClears.Features.Shared.Enums.Extensions;
using RaidClears.Localization;
using RaidClears.Settings.Enums;
using Colors = RaidClears.Utils.Colors;

namespace RaidClears.Settings.Models;

public record Setting<T>(string Key, T DefaultValue, Func<string>? Name = null, Func<string>? Description = null)
{
    public string Key { get; } = Key;
    public T DefaultValue { get; } = DefaultValue;
    public Func<string>? Name { get; } = Name;
    public Func<string>? Description { get; } = Description;
}

public record DungeonSetting<T>(string Key, T DefaultValue, Func<string>? Name = null) :
    Setting<T>(Key, DefaultValue, Name, () => $"Enable {Name?.Invoke()} on the dungeon display");

public record StrikeSetting<T>(string Key, T DefaultValue, Func<string>? Name = null) :
    Setting<T>(Key, DefaultValue, Name, () => $"Enable {Name?.Invoke()} on the strike display");

public static class Settings
{
    public static class Dungeons
    {
        public static class General
        {
            public static Setting<bool> enable = new("RCDungeonEnable", true, () => Strings.Setting_Dun_Enabled, () => Strings.Setting_Dun_Enabled_Tooltip);
            public static Setting<Point> location = new("RCDungeonLoc", new Point(250, 410));
            public static Setting<bool> positionLock = new("RCDunDrag", true, () => Strings.Setting_Dun_Drag_Label, () => Strings.Setting_Dun_Drag_Tooltip);
            public static Setting<bool> tooltips = new("RCDuntooltips", true, () => Strings.Setting_Dun_Tooltips_Label, () => Strings.Setting_Dun_Tooltips_Tooltip);
            public static Setting<bool> toolbarIcon = new("RCDungeonCornerIcon", true, () => Strings.Setting_Dun_Icon_Label, () => Strings.Setting_Dun_Icon_Tooltip);
            public static Setting<bool> visible = new("RCDungeonActive", true, () => Strings.Setting_Dun_Visible_Label, () => Strings.Setting_Dun_Visible_Tooltip);
            public static Setting<KeyBinding> keyBind = new("RCDungeonkeybind", new KeyBinding(Keys.None), () => Strings.Setting_Dun_Keybind_Label, () => Strings.Setting_Dun_Keybind_Tooltip);
        }
        
        public static class Style
        {
            public static class Color
            {
                public static Setting<string> uncleared = new("DunColNotCleared", Colors.SoftRed, () => Strings.Setting_Raid_ColNotClear_Label, () => Strings.Setting_Raid_ColNotClear_Tooltip);
                public static Setting<string> cleared = new("DunColCleared", Colors.Green, () => Strings.Setting_Raid_ColClear_Label, () => Strings.Setting_Raid_ColClear_Tooltip);
                public static Setting<string> text = new("DunColText", Colors.White, () => Strings.Setting_Raid_ColText_Label, () => Strings.Setting_Raid_ColText_Tooltip); 
                public static Setting<string> frequenter = new("DunColFreq", Colors.Yellow, () => Strings.Setting_Dun_ColFreqText_Label, () => Strings.Setting_Dun_ColFreqText_Tooltip); 
                public static Setting<string> background = new("DunColBG", Colors.Black, () => Strings.Setting_Raid_ColBG_Label, () => Strings.Setting_Raid_ColBG_Tooltip);
            }

            public static Setting<ContentService.FontSize> fontSize = new("RCDungeonFontSize", ContentService.FontSize.Size18, () => Strings.Setting_Raid_Font_Label, () => Strings.Setting_Raid_Font_Tooptip);
            public static Setting<LabelDisplay> labelDisplay = new("RCDungeonLabelDisplay", LabelDisplay.Abbreviation, () => Strings.Setting_Raid_LabelDisplay_Label, () => Strings.Setting_Raid_LabelDisplay_Tooltip);
            public static Setting<Layout> layout = new("RCDungeonOrientation", Layout.Vertical, () => Strings.Setting_Raid_Layout_Label);
            public static Setting<float> labelOpacity = new("RCDungeonOpacity", 1f, () => Strings.Setting_Raid_LabelOpacity_Label, () => Strings.Setting_Raid_LabelOpacity_Tooltip);
            public static Setting<float> gridOpacity = new("RCPathOpacity", 0.8f, () => Strings.Setting_Raid_GridOpacity_Label, () => Strings.Setting_Raid_GridOpactiy_Tooltip);
            public static Setting<float> backgroundOpacity = new("RCDunBGOpacity", 0.0f, () => Strings.Setting_Raid_PanelOpacity_Label, () => Strings.Setting_Raid_PanelOpacity_Tooltip);
        }

        public static class Module
        {
            public static Setting<bool> highlightFrequenter = new("RCDunFreqHighlight", true, () => "Highlight Frequenter Paths");

            public static Setting<bool>[] dungeonPaths = {
                new DungeonSetting<bool>("RCd1", true, () => Encounters.Dungeons.AscalonianCatacombs.GetLabel()),
                new DungeonSetting<bool>("RCd2", true, () => Encounters.Dungeons.CaudecusManor.GetLabel()),
                new DungeonSetting<bool>("RCd3", true, () => Encounters.Dungeons.TwilightArbor.GetLabel()),
                new DungeonSetting<bool>("RCd4", true, () => Encounters.Dungeons.SorrowsEmbrace.GetLabel()),
                new DungeonSetting<bool>("RCd5", true, () => Encounters.Dungeons.CitadelOfFlame.GetLabel()),
                new DungeonSetting<bool>("RCd6", true, () => Encounters.Dungeons.HonorOfTheWaves.GetLabel()),
                new DungeonSetting<bool>("RCd7", true, () => Encounters.Dungeons.CrucibleOfEternity.GetLabel()),
                new DungeonSetting<bool>("RCd8", true, () => Encounters.Dungeons.RuinedCityOfArah.GetLabel()),
            };

            public static Setting<bool> dungeonFrequenterEnabled = new("RCdf", true, () => "Dungeon Frequenter Summary", () => "Enable a dungeon frequenter achievement summary");
        }
    }

    public static class Raids
    {
        public static class General
        {
            public static Setting<bool> enabled = new("RCRaidEnabled", true, () => "Enable Raids Feature");
            public static Setting<Point> location = new("RCLocation", new Point(250, 250));
            public static Setting<bool> positionLock = new("RCDrag", true, () => Strings.Setting_Raid_Drag_Label, () => Strings.Setting_Raid_Drag_Tooltip);
            public static Setting<bool> tooltips = new("RCtooltips", true, () => Strings.Setting_Raid_Tooltips_Label, () => Strings.Setting_Raid_Tooltips_Tooltip);
            public static Setting<bool> toolbarIcon = new("RCCornerIcon", true, () => Strings.Setting_Raid_Icon_Label, () => Strings.Setting_Raid_Icon_Tooltip);
            public static Setting<bool> visible = new("RCActive", true, () => Strings.Setting_Raid_Visible_Label, () => Strings.Setting_Raid_Visible_Tooltip);
            public static Setting<KeyBinding> keyBind = new("RCkeybind", new KeyBinding(Keys.None), () => Strings.Setting_Raid_Keybind_Label, () => Strings.Setting_Raid_Keybind_Tooltip);
        }

        public static class Style
        {
            public static class Color
            {
                public static Setting<string> uncleared = new("colNotCleared", Colors.SoftRed, () => Strings.Setting_Raid_ColNotClear_Label, () => Strings.Setting_Raid_ColNotClear_Tooltip);
                public static Setting<string> cleared = new("colCleared", Colors.Green, () => Strings.Setting_Raid_ColClear_Label, () => Strings.Setting_Raid_ColClear_Tooltip);
                public static Setting<string> text = new("colText", Colors.White, () => Strings.Setting_Raid_ColText_Label, () => Strings.Setting_Raid_ColText_Tooltip);
                public static Setting<string> cotm = new("colCotm", Colors.Yellow, () => Strings.Setting_Raid_ColCotm_Label, () => Strings.Setting_Raid_ColCotm_Tooltip);
                public static Setting<string> embolden = new("colEmbolden", Colors.Blue, () => Strings.Setting_Raid_ColEmbolden_Label, () => Strings.Setting_Raid_ColEmbolden_Tooltip);
                public static Setting<string> background = new("colRaidBG", Colors.Black, () => Strings.Setting_Raid_ColBG_Label, () => Strings.Setting_Raid_ColBG_Tooltip);
            }
            
            public static Setting<ContentService.FontSize> fontSize = new("RCFontSize", ContentService.FontSize.Size18, () => Strings.Setting_Raid_Font_Label, () => Strings.Setting_Raid_Font_Tooptip);
            public static Setting<LabelDisplay> labelDisplay = new("RCLabelDisplay", LabelDisplay.Abbreviation, () => Strings.Setting_Raid_LabelDisplay_Label, () => Strings.Setting_Raid_LabelDisplay_Tooltip);
            public static Setting<Layout> layout = new("RCOrientation", Layout.Vertical, () => Strings.Setting_Raid_Layout_Label, () => Strings.Setting_Raid_Layout_Tooltip);
            public static Setting<float> labelOpacity = new("RCWingOpacity", 1f, () => Strings.Setting_Raid_LabelOpacity_Label, () => Strings.Setting_Raid_LabelOpacity_Tooltip);
            public static Setting<float> gridOpacity = new("RCEncOpacity", 0.8f, () => Strings.Setting_Raid_GridOpacity_Label, () => Strings.Setting_Raid_GridOpactiy_Tooltip);
            public static Setting<float> backgroundOpacity = new("RCRaidBgOpacity", 0.0f, () => Strings.Setting_Raid_PanelOpacity_Label, () => Strings.Setting_Raid_PanelOpacity_Tooltip);
        }

        public static class Module
        {
            public static Setting<bool> highlightEmbolden = new("RCEmbolden", true, () => Strings.Setting_Raid_Embolden_Label, () => Strings.Setting_Raid_Embolden_Tooltip);
            public static Setting<bool> highlightCotm = new("RCCotM", true, () => Strings.Setting_Raid_Cotm_Label, () => Strings.Setting_Raid_Cotm_Tooltip);
            
            public static Setting<bool>[] raidWings = {
                new Setting<bool>("RCw1",true,() => Strings.Setting_Raid_W1_Label,() => Strings.Setting_Raid_W1_Tooltip),
                new Setting<bool>("RCw2",true,() => Strings.Setting_Raid_W2_Label,() => Strings.Setting_Raid_W2_Tooltip),
                new Setting<bool>("RCw3",true,() => Strings.Setting_Raid_W3_Label,() => Strings.Setting_Raid_W3_Tooltip),
                new Setting<bool>("RCw4",true,() => Strings.Setting_Raid_W4_Label,() => Strings.Setting_Raid_W4_Tooltip),
                new Setting<bool>("RCw5",true,() => Strings.Setting_Raid_W5_Label,() => Strings.Setting_Raid_W5_Tooltip),
                new Setting<bool>("RCw6",true,() => Strings.Setting_Raid_W6_Label,() => Strings.Setting_Raid_W6_Tooltip),
                new Setting<bool>("RCw7",true,() => Strings.Setting_Raid_W7_Label,() => Strings.Setting_Raid_W7_Tooltip),
            };
        }
    }

    public static class Strikes
    {
        public static class Style
        {
            public static class Color
            {
                public static Setting<string> uncleared = new("StkcolNotCleared", Colors.SoftRed, () => Strings.Setting_Strike_ColNotClear_Label, () => Strings.Setting_Strike_ColNotClear_Tooltip);
                public static Setting<string> cleared = new("StkColCleared", Colors.Green, () => Strings.Setting_Strike_ColClear_Label, () => Strings.Setting_Strike_ColClear_Tooltip);
                public static Setting<string> text = new("StkColText", Colors.White, () => Strings.Setting_Strike_ColText_Label, () => Strings.Setting_Strike_ColText_Tooltip);
                public static Setting<string> background = new("colStrikeBG", Colors.Black, () => Strings.Setting_Strike_ColBG_Label, () => Strings.Setting_Strike_ColBG_Tooltip);
            }
            
            public static Setting<ContentService.FontSize> fontSize = new("RCStkFontSize", ContentService.FontSize.Size18, () => Strings.Setting_Strike_Font_Label, () => Strings.Setting_Strike_Font_Tooptip);
            public static Setting<LabelDisplay> labelDisplay = new("RCStkLabelDisplay", LabelDisplay.Abbreviation, () => Strings.Setting_Strike_LabelDisplay_Label, () => Strings.Setting_Strike_LabelDisplay_Tooltip);
            public static Setting<Layout> layout = new("RCStkOrientation", Layout.Vertical, () => Strings.Setting_Strike_Layout_Label, () => Strings.Setting_Strike_Layout_Tooltip);
            public static Setting<float> labelOpacity = new("RCStkLabelOpacity", 1f, () => Strings.Setting_Strike_LabelOpacity_Label, () => Strings.Setting_Strike_LabelOpacity_Tooltip);
            public static Setting<float> gridOpacity = new("RCStkOpacity", 0.8f, () => Strings.Setting_Strike_GridOpacity_Label, () => Strings.Setting_Strike_GridOpactiy_Tooltip);
            public static Setting<float> backgroundOpacity = new("RCStrikeBgOpacity", 0.0f, () => Strings.Setting_Strike_PanelOpacity_Label, () => Strings.Setting_Strike_PanelOpacity_Tooltip);
        }

        public static class General
        {
            public static Setting<bool> enabled = new("RCStkEnabled", true, () => "Enable Raids Feature");
            public static Setting<Point> location = new("RCStkLocation", new Point(250, 250));
            public static Setting<bool> positionLock = new("RCStkDrag", true, () => Strings.Setting_Strike_Drag_Label, () => Strings.Setting_Strike_Drag_Tooltip);
            public static Setting<bool> tooltips = new("RCStktooltips", true, () => Strings.Setting_Strike_Tooltips_Label, () => Strings.Setting_Strike_Tooltips_Tooltip);
            public static Setting<bool> toolbarIcon = new("RCStkCornerIcon", true, () => Strings.Setting_Strike_Icon_Label, () => Strings.Setting_Strike_Icon_Tooltip);
            public static Setting<bool> visible = new("RCStkActive", true, () => Strings.Setting_Strike_Visible_Label, () => Strings.Setting_Strike_Visible_Tooltip);
            public static Setting<KeyBinding> keyBind = new("RCStkkeybind", new KeyBinding(Keys.None), () => Strings.Setting_Strike_Keybind_Label, () => Strings.Setting_Strike_Keybind_Tooltip);
        }

        public static class Module
        {
            public static Setting<bool>[] ibsMissions =
            {
                new StrikeSetting<bool>("StrikeVis_cold_war", true, () => Strings.Setting_Strike_CW_Label),
                new StrikeSetting<bool>("StrikeVis_fraenir_of_jormag", true, () => Strings.Setting_Strike_FoJ_Label),
                new StrikeSetting<bool>("StrikeVis_shiverpeak_pass", true, () => Strings.Setting_Strike_SP_Label),
                new StrikeSetting<bool>("StrikeVis_voice_and_claw", true, () => Strings.Setting_Strike_VandC_Label),
                new StrikeSetting<bool>("StrikeVis_whisper_of_jormag", true, () => Strings.Setting_Strike_WoJ_Label),
                new StrikeSetting<bool>("StrikeVis_boneskinner", true, () => Strings.Setting_Strike_BS_Label),
            };

            public static Setting<bool>[] eodMissions =
            {
                new StrikeSetting<bool>("StrikeVis_aetherblade_hideout", true, () => Strings.Setting_Strike_AH_Label),
                new StrikeSetting<bool>("StrikeVis_xunlai_jade_junkyard", true, () => Strings.Setting_Strike_XJJ_Label),
                new StrikeSetting<bool>("StrikeVis_kaineng_overlook", true, () => Strings.Setting_Strike_KO_Label),
                new StrikeSetting<bool>("StrikeVis_harvest_temple", true, () => Strings.Setting_Strike_HT_Label),
                new StrikeSetting<bool>("StrikeVis_old_lion_court", true, () => Strings.Setting_Strike_OLC_Label),
            };

            public static Setting<bool> showIbs = new("StrikeVis_ibs", true, () => Strings.Setting_Strike_IBS);
            public static Setting<bool> showEod = new("StrikeVis_eod", true, () => Strings.Setting_Strike_Eod);
            public static Setting<bool> showPriority = new("StrikeVis_priority", true, () => Strings.Setting_Stike_Priority);
        }
    }
}

public static class SettingCollectionExtensions
{
    public static SettingEntry<TEntry> DefineSetting<TEntry>(this SettingCollection collection, Setting<TEntry> setting)
    {
        return collection.DefineSetting(setting.Key, setting.DefaultValue, setting.Name, setting.Description);
    }
}