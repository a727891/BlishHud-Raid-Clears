﻿using System;
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
    Setting<T>(Key, DefaultValue, Name, () => $"Enable {Name?.Invoke()} on the dungeon overlay");

public record StrikeSetting<T>(string Key, T DefaultValue, Func<string>? Name = null) :
    Setting<T>(Key, DefaultValue, Name, () => $"Enable {Name?.Invoke()} on the strike overlay");

public static class Settings
{
    public static class Dungeons
    {
        public static class General
        {
            public static Setting<bool> enable = new("RCDungeonEnable", true, () => Strings.Setting_Dun_Enabled, () => Strings.Setting_Dun_Enabled_Tooltip);
            public static Setting<Point> location = new("RCDungeonLoc", new Point(250, 500));
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
                public static Setting<string> uncleared = new("DunColNotCleared", Colors.LightGray, () => Strings.Setting_Raid_ColNotClear_Label, () => Strings.Setting_Raid_ColNotClear_Tooltip);
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
            public static Setting<Point> location = new("RCLocation", new Point(250, 210));
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
                public static Setting<string> uncleared = new("colNotCleared", Colors.LightGray, () => Strings.Setting_Raid_ColNotClear_Label, () => Strings.Setting_Raid_ColNotClear_Tooltip);
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
                new("RCw1",true,() => Strings.Setting_Raid_W1_Label,() => Strings.Setting_Raid_W1_Tooltip),
                new("RCw2",true,() => Strings.Setting_Raid_W2_Label,() => Strings.Setting_Raid_W2_Tooltip),
                new("RCw3",true,() => Strings.Setting_Raid_W3_Label,() => Strings.Setting_Raid_W3_Tooltip),
                new("RCw4",true,() => Strings.Setting_Raid_W4_Label,() => Strings.Setting_Raid_W4_Tooltip),
                new("RCw5",true,() => Strings.Setting_Raid_W5_Label,() => Strings.Setting_Raid_W5_Tooltip),
                new("RCw6",true,() => Strings.Setting_Raid_W6_Label,() => Strings.Setting_Raid_W6_Tooltip),
                new("RCw7",true,() => Strings.Setting_Raid_W7_Label,() => Strings.Setting_Raid_W7_Tooltip),
                new("RCw8",true,() => Strings.Setting_Raid_W8_Label,() => Strings.Setting_Raid_W8_Tooltip),
            };
        }
    }

    public static class Strikes
    {
        public static class Style
        {
            public static class Color
            {
                public static Setting<string> uncleared = new("StkcolNotCleared", Colors.LightGray, () => Strings.Setting_Strike_ColNotClear_Label, () => Strings.Setting_Strike_ColNotClear_Tooltip);
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
            public static Setting<bool> enabled = new("RCStkEnabled", true, () => "Enable strikes Feature");
            public static Setting<Point> location = new("RCStkLocation", new Point(250, 370));
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
                new StrikeSetting<bool>("StrikeVis_shiverpeak_pass", true, () => Strings.Setting_Strike_SP_Label),
                new StrikeSetting<bool>("StrikeVis_fraenir_of_jormag", true, () => Strings.Setting_Strike_FoJ_Label),
                new StrikeSetting<bool>("StrikeVis_voice_and_claw", true, () => Strings.Setting_Strike_VandC_Label),
                new StrikeSetting<bool>("StrikeVis_whisper_of_jormag", true, () => Strings.Setting_Strike_WoJ_Label),
                new StrikeSetting<bool>("StrikeVis_boneskinner", true, () => Strings.Setting_Strike_BS_Label),
                new StrikeSetting<bool>("StrikeVis_cold_war", true, () => Strings.Setting_Strike_CW_Label),
                new StrikeSetting<bool>("StrikeVis_dragonstorm", true, () => "Dragonstorm"),
            };

            public static Setting<bool>[] eodMissions =
            {
                new StrikeSetting<bool>("StrikeVis_aetherblade_hideout", true, () => Strings.Setting_Strike_AH_Label),
                new StrikeSetting<bool>("StrikeVis_xunlai_jade_junkyard", true, () => Strings.Setting_Strike_XJJ_Label),
                new StrikeSetting<bool>("StrikeVis_kaineng_overlook", true, () => Strings.Setting_Strike_KO_Label),
                new StrikeSetting<bool>("StrikeVis_harvest_temple", true, () => Strings.Setting_Strike_HT_Label),
                new StrikeSetting<bool>("StrikeVis_old_lion_court", true, () => Strings.Setting_Strike_OLC_Label),
            };
            public static Setting<bool>[] sotoMissions =
            {
                new StrikeSetting<bool>("StrikeVis_cosmic_observatory", true, () => "Cosmic Observatory"),
                new StrikeSetting<bool>("StrikeVis_temple_of_febe", true, () => "Temple of Febe"),
               
            };

            public static Setting<bool> showIbs = new("StrikeVis_ibs", true, () => Strings.Setting_Strike_IBS);
            public static Setting<bool> showEod = new("StrikeVis_eod", true, () => Strings.Setting_Strike_Eod);
            public static Setting<bool> showSotO = new("StrikeVis_soto", true, () => "Enable Secrets of the Obscure");
            public static Setting<bool> showPriority = new("StrikeVis_priority", true, () => Strings.Setting_Stike_Priority);
            public static Setting<bool> anchorToRaids = new("RCAnchorToRaids", false, () => Strings.Settings_Strike_AnchorToRaidLabel, () => Strings.Settings_Strike_AnchorToRaidTooltip);

            public static Setting<StrikeComplete> strikeCompletion = new("RCStrikeComplete", StrikeComplete.MAP_CHANGE, () => Strings.Settings_Strike_Completion, () => Strings.Settings_Strike_CompletionTooltip);
        }
    }

    public static class Fractal
    {
        public static class Style
        {
            public static class Color
            {
                public static Setting<string> uncleared = new("FraccolNotCleared", Colors.LightGray, () => Strings.Setting_Fractals_ColNotClear_Label, () => Strings.Setting_Fractals_ColNotClear_Tooltip);
                public static Setting<string> cleared = new("FracColCleared", Colors.Green, () => Strings.Setting_Fractals_ColClear_Label, () => Strings.Setting_Fractals_ColClear_Tooltip);
                public static Setting<string> text = new("FracColText", Colors.White, () => Strings.Setting_Fractals_ColText_Label, () => Strings.Setting_Fractals_ColText_Tooltip);
                public static Setting<string> background = new("colFracBG", Colors.Black, () => Strings.Setting_Fractals_ColBG_Label, () => Strings.Setting_Fractals_ColBG_Tooltip);
            }

            public static Setting<ContentService.FontSize> fontSize = new("RCFracFontSize", ContentService.FontSize.Size18, () => Strings.Setting_Fractals_Font_Label, () => Strings.Setting_Fractals_Font_Tooptip);
            public static Setting<LabelDisplay> labelDisplay = new("RCFracLabelDisplay", LabelDisplay.Abbreviation, () => Strings.Setting_Fractals_LabelDisplay_Label, () => Strings.Setting_Fractals_LabelDisplay_Tooltip);
            public static Setting<Layout> layout = new("RCFracOrientation", Layout.Vertical, () => Strings.Setting_Fractals_Layout_Label, () => Strings.Setting_Fractals_Layout_Tooltip);
            public static Setting<float> labelOpacity = new("RCFracLabelOpacity", 1f, () => Strings.Setting_Fractals_LabelOpacity_Label, () => Strings.Setting_Fractals_LabelOpacity_Tooltip);
            public static Setting<float> gridOpacity = new("RCFracOpacity", 0.8f, () => Strings.Setting_Fractals_GridOpacity_Label, () => Strings.Setting_Fractals_GridOpactiy_Tooltip);
            public static Setting<float> backgroundOpacity = new("RCSFracBgOpacity", 0.0f, () => Strings.Setting_Fractals_PanelOpacity_Label, () => Strings.Setting_Fractals_PanelOpacity_Tooltip);
        }

        public static class General
        {
            public static Setting<bool> enabled = new("RCFracEnabled", true, () => "Enable Fractals Feature");
            public static Setting<Point> location = new("RCFracLocation", new Point(250, 445));
            public static Setting<bool> positionLock = new("RCFracDrag", true, () => Strings.Setting_Fractals_Drag_Label, () => Strings.Setting_Fractals_Drag_Tooltip);
            public static Setting<bool> tooltips = new("RCFractooltips", true, () => Strings.Setting_Fractals_Tooltips_Label, () => Strings.Setting_Fractals_Tooltips_Tooltip);
            public static Setting<bool> toolbarIcon = new("RCFracCornerIcon", true, () => Strings.Setting_Fractals_Icon_Label, () => Strings.Setting_Fractals_Icon_Tooltip);
            public static Setting<bool> visible = new("RCFracActive", true, () => Strings.Setting_Fractals_Visible_Label, () => Strings.Setting_Fractals_Visible_Tooltip);
            public static Setting<KeyBinding> keyBind = new("RCSFrackeybind", new KeyBinding(Keys.None), () => Strings.Setting_Fractals_Keybind_Label, () => Strings.Setting_Fractals_Keybind_Tooltip);
        }

        public static class Module
        {
            public static Setting<bool> showCMs = new("FractalCM", true, () => "Show Challenge Motes",()=>"Display the CM fractals, hover tooltip shows Instabilities");
            public static Setting<bool> showTierN = new("FractalTierN", true, () => Strings.Fractals_DailyTierN);
            public static Setting<bool> showRecs = new("FractalRecs", true, () => Strings.Fractals_DailyRecommended);
            public static Setting<bool> tomorrow = new("FractalTierTomorrow", false, () => "Tomorrow's Tier",()=>"Show tomorow's TierN fractals. Useful for statics that pre-clear before reset");

            public static Setting<StrikeComplete> completionMethod = new("RCFractalComplete", StrikeComplete.MAP_CHANGE, () => Strings.Settings_Strike_Completion, () => Strings.Settings_Fractals_Completion);

        }
    }
}

public static class SettingCollectionExtensions
{
    public static SettingEntry<TEntry> DefineSetting<TEntry>(this SettingCollection collection, Setting<TEntry> setting)
    {
        return collection.DefineSetting(setting.Key, setting.DefaultValue, setting.Name, setting.Description);
    }
    public static SettingEntry<TEntry> DefineSettingRange<TEntry>(this SettingCollection collection, Setting<TEntry> setting,float min, float max)
    {
        var settingEntry = collection.DefineSetting(setting.Key, setting.DefaultValue, setting.Name, setting.Description);
        (settingEntry as SettingEntry<float>).SetRange(min, max);
        return settingEntry;
    }


}