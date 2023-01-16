using Blish_HUD.Input;
using Blish_HUD;
using Blish_HUD.Settings;
using RaidClears.Localization;
using Microsoft.Xna.Framework.Input;
using Settings.Enums;
using Microsoft.Xna.Framework;

namespace RaidClears.Settings
{
    public class SettingService // singular because Setting"s"Service already exists in Blish
    {
        public SettingEntry<ApiPollPeriod> ApiPollingPeriod { get; }
        public SettingEntry<KeyBinding> SettingsPanelKeyBind { get; }

        #region Raid Setting Variables
        public SettingEntry<Point> RaidPanelLocationPoint { get; }
        public SettingEntry<bool> RaidPanelDragWithMouseIsEnabled { get; }
        public SettingEntry<bool> RaidPanelAllowTooltips { get; }
        public SettingEntry<bool> RaidCornerIconEnabled { get; }
        public SettingEntry<bool> RaidPanelIsVisible { get; }
        public SettingEntry<KeyBinding> RaidPanelIsVisibleKeyBind { get; }
        public SettingEntry<ContentService.FontSize> RaidPanelFontSize { get; }
        public SettingEntry<LabelDisplay> RaidPanelLabelDisplay { get; }
        public SettingEntry<Layout> RaidPanelLayout { get; }

        public SettingEntry<float> RaidPanelLabelOpacity { get; }
        public SettingEntry<float> RaidPanelGridOpacity { get; }
        public SettingEntry<float> RaidPanelBgOpacity { get; }
        
        public SettingEntry<bool> RaidPanelHighlightEmbolden { get; }
        public SettingEntry<bool> RaidPanelHighlightCotM { get; }
        public SettingEntry<bool> W1IsVisible { get; }
        public SettingEntry<bool> W2IsVisible { get; }
        public SettingEntry<bool> W3IsVisible { get; }
        public SettingEntry<bool> W4IsVisible { get; }
        public SettingEntry<bool> W5IsVisible { get; }
        public SettingEntry<bool> W6IsVisible { get; }
        public SettingEntry<bool> W7IsVisible { get; }

        public SettingEntry<string> RaidPanelColorNotCleared { get; }
        public SettingEntry<string> RaidPanelColorCleared { get; }
        public SettingEntry<string> RaidPanelColorText { get; }
        public SettingEntry<string> RaidPanelColorCotm { get; }
        public SettingEntry<string> RaidPanelColorEmbolden { get; }
        public SettingEntry<string> RaidPanelColorBG { get; }
        #endregion

        #region Dungeon Setting Variables
        public SettingEntry<Point> DungeonPanelLocationPoint { get; }
        public SettingEntry<bool> DungeonPanelDragWithMouseIsEnabled { get; }
        public SettingEntry<bool> DungeonPanelAllowTooltips { get; }
        public SettingEntry<bool> DungeonCornerIconEnabled { get; }
        public SettingEntry<bool> DungeonPanelIsVisible { get; }
        public SettingEntry<KeyBinding> DungeonPanelIsVisibleKeyBind { get; }
        public SettingEntry<ContentService.FontSize> DungeonPanelFontSize { get; }
        public SettingEntry<LabelDisplay> DungeonPanelLabelDisplay { get; }
        public SettingEntry<Layout> DungeonPanelLayout { get; }
        public SettingEntry<float> DungeonPanelLabelOpacity { get; }
        public SettingEntry<float> DungeonPanelGridOpacity { get; }
        public SettingEntry<float> DungeonPanelBgOpacity { get; }
        public SettingEntry<bool> dungeonHighlightFrequenter { get; }
        public SettingEntry<bool> D1IsVisible { get; }
        public SettingEntry<bool> D2IsVisible { get; }
        public SettingEntry<bool> D3IsVisible { get; }
        public SettingEntry<bool> D4IsVisible { get; }
        public SettingEntry<bool> D5IsVisible { get; }
        public SettingEntry<bool> D6IsVisible { get; }
        public SettingEntry<bool> D7IsVisible { get; }
        public SettingEntry<bool> D8IsVisible { get; }
        public SettingEntry<bool> DFIsVisible { get; }
        

        public SettingEntry<string> DungeonPanelColorNotCleared { get; }
        public SettingEntry<string> DungeonPanelColorCleared { get; }
        public SettingEntry<string> DungeonPanelColorText { get; }
        public SettingEntry<string> DungeonPanelColorFreq { get; }
        public SettingEntry<string> DungeonPanelColorBG { get; }
        #endregion

        public SettingService(SettingCollection settings)
        {

            #region ModuleGeneral
            var internalSettingSubCollection = settings.AddSubCollection("internal settings (not visible in UI)");

            ApiPollingPeriod = settings.DefineSetting("RCPoll",
                ApiPollPeriod.MINUTES_5,
                () => Strings.Setting_APIPoll_Label,
                () => Strings.Setting_APIPoll_Tooltip);

            SettingsPanelKeyBind = settings.DefineSetting("RCsettingsKeybind", new KeyBinding(Keys.None),
            () => Strings.Settings_Keybind_Label,
            () => Strings.Settings_Keybind_tooltip);
            SettingsPanelKeyBind.Value.Enabled = true;

            #endregion

            #region RaidPanel
            #region Raid_General

            RaidPanelLocationPoint = internalSettingSubCollection.DefineSetting("RCLocation", new Point(250, 250));

            RaidPanelDragWithMouseIsEnabled = settings.DefineSetting("RCDrag",
               true,
               () => Strings.Setting_Raid_Drag_Label,
               () => Strings.Setting_Raid_Drag_Tooltip);

            RaidPanelAllowTooltips = settings.DefineSetting("RCtooltips",
                true,
               () => Strings.Setting_Raid_Tooltips_Label,
               () => Strings.Setting_Raid_Tooltips_Tooltip);

            #endregion

            #region Raid Layout
            RaidCornerIconEnabled = settings.DefineSetting("RCCornerIcon",
               true,
               () => Strings.Setting_Raid_Icon_Label,
               () => Strings.Setting_Raid_Icon_Tooltip);

            RaidPanelIsVisible = settings.DefineSetting("RCActive",
                true,
                () => Strings.Setting_Raid_Visible_Label,
                () => Strings.Setting_Raid_Visible_Tooltip);

            RaidPanelIsVisibleKeyBind = settings.DefineSetting("RCkeybind", new KeyBinding(Keys.None),
                () => Strings.Setting_Raid_Keybind_Label,
                () => Strings.Setting_Raid_Keybind_Tooltip);
            RaidPanelIsVisibleKeyBind.Value.Enabled = true;


            RaidPanelFontSize = settings.DefineSetting("RCFontSize",
                ContentService.FontSize.Size18,
                () => Strings.Setting_Raid_Font_Label,
                () => Strings.Setting_Raid_Font_Tooptip);

            RaidPanelLabelDisplay = settings.DefineSetting("RCLabelDisplay",
                LabelDisplay.Abbreviation,
                () => Strings.Setting_Raid_LabelDisplay_Label,
                () => Strings.Setting_Raid_LabelDisplay_Tooltip);

            RaidPanelLayout = settings.DefineSetting("RCOrientation",
                Layout.Vertical,
                () => Strings.Setting_Raid_Layout_Label,
                () => Strings.Setting_Raid_Layout_Tooltip);

            RaidPanelLabelOpacity = settings.DefineSetting("RCWingOpacity",
                1f,
                () =>Strings.Setting_Raid_LabelOpacity_Label,
                () => Strings.Setting_Raid_LabelOpacity_Tooltip);
            RaidPanelLabelOpacity.SetRange(0.1f, 1f);

            RaidPanelGridOpacity = settings.DefineSetting("RCEncOpacity",
                0.8f,
                () => Strings.Setting_Raid_GridOpacity_Label,
                () => Strings.Setting_Raid_GridOpactiy_Tooltip);
            RaidPanelGridOpacity.SetRange(0.1f, 1f);

            RaidPanelBgOpacity = settings.DefineSetting("RCRaidBgOpacity",
                0.0f,
                () => Strings.Setting_Raid_PanelOpacity_Label,
                () => Strings.Setting_Raid_PanelOpacity_Tooltip);
            RaidPanelBgOpacity.SetRange(0.0f, 1f);

            RaidPanelHighlightEmbolden = settings.DefineSetting("RCEmbolden",
                true,
                () => Strings.Setting_Raid_Embolden_Label,
                () => Strings.Setting_Raid_Embolden_Tooltip);
            RaidPanelHighlightCotM = settings.DefineSetting("RCCotM",
               true,
               () => Strings.Setting_Raid_Cotm_Label,
               () => Strings.Setting_Raid_Cotm_Tooltip);

            #endregion

            #region Raid WingSelection

            W1IsVisible = settings.DefineSetting("RCw1",
                true,
                () => Strings.Setting_Raid_W1_Label,
                () => Strings.Setting_Raid_W1_Tooltip
                );
            W2IsVisible = settings.DefineSetting("RCw2",
                true,
                () => Strings.Setting_Raid_W2_Label,
                () => Strings.Setting_Raid_W2_Tooltip
                );
            W3IsVisible = settings.DefineSetting("RCw3",
               true,
                () => Strings.Setting_Raid_W3_Label,
                () => Strings.Setting_Raid_W3_Tooltip
                );
            W4IsVisible = settings.DefineSetting("RCw4",
               true,
                () => Strings.Setting_Raid_W4_Label,
                () => Strings.Setting_Raid_W4_Tooltip
                );
            W5IsVisible = settings.DefineSetting("RCw5",
                true,
                () => Strings.Setting_Raid_W5_Label,
                () => Strings.Setting_Raid_W5_Tooltip
                );
            W6IsVisible = settings.DefineSetting("RCw6",
               true,
                () => Strings.Setting_Raid_W6_Label,
                () => Strings.Setting_Raid_W6_Tooltip
                );
            W7IsVisible = settings.DefineSetting("RCw7",
                true,
                () => Strings.Setting_Raid_W7_Label,
                () => Strings.Setting_Raid_W7_Tooltip
                );
            #endregion

            #region Raid Colors
            RaidPanelColorNotCleared = settings.DefineSetting("colNotCleared",
               "#781414",
               () => Strings.Setting_Raid_ColNotClear_Label,
               () => Strings.Setting_Raid_ColNotClear_Tooltip);

            RaidPanelColorCleared = settings.DefineSetting("colCleared",
                "#147814",
                () => Strings.Setting_Raid_ColClear_Label,
                () => Strings.Setting_Raid_ColClear_Tooltip);

            RaidPanelColorText = settings.DefineSetting("colText",
                "#ffffff",
                () => Strings.Setting_Raid_ColText_Label,
                () => Strings.Setting_Raid_ColText_Tooltip);

            RaidPanelColorCotm = settings.DefineSetting("colCotm",
                "#f3f527",
                () => Strings.Setting_Raid_ColCotm_Label,
                () => Strings.Setting_Raid_ColCotm_Tooltip);

            RaidPanelColorEmbolden = settings.DefineSetting("colEmbolden",
                "#5050ff",
                () => Strings.Setting_Raid_ColEmbolden_Label,
                () => Strings.Setting_Raid_ColEmbolden_Tooltip);
            RaidPanelColorBG = settings.DefineSetting("colRaidBG",
                "#000000",
                () => Strings.Setting_Raid_ColBG_Label,
                () => Strings.Setting_Raid_ColBG_Tooltip);
            #endregion
            #endregion

            #region DungeonPanel
            #region DUNGEONS

            DungeonPanelLocationPoint = internalSettingSubCollection.DefineSetting("RCDungeonLoc", new Point(250, 410));
            DungeonPanelDragWithMouseIsEnabled = settings.DefineSetting("RCDunDrag",
               true,
               () => Strings.Setting_Dun_Drag_Label,
               () => Strings.Setting_Dun_Drag_Tooltip);

            DungeonPanelAllowTooltips = settings.DefineSetting("RCDuntooltips",
                true,
               () => Strings.Setting_Dun_Tooltips_Label,
               () => Strings.Setting_Dun_Tooltips_Tooltip);

            DungeonCornerIconEnabled = settings.DefineSetting("RCDungeonCornerIcon",
                true,
                () => Strings.Setting_Dun_Icon_Label,
                () => Strings.Setting_Dun_Icon_Tooltip);

            DungeonPanelIsVisible = settings.DefineSetting("RCDungeonActive",
                true,
                () => Strings.Setting_Dun_Visible_Label,
                () => Strings.Setting_Dun_Visible_Tooltip);

            DungeonPanelIsVisibleKeyBind = settings.DefineSetting("RCDungeonkeybind", new KeyBinding(Keys.None),
                () => Strings.Setting_Dun_Keybind_Label,
                () => Strings.Setting_Dun_Keybind_Tooltip);
            DungeonPanelIsVisibleKeyBind.Value.Enabled = true;


            DungeonPanelFontSize = settings.DefineSetting("RCDungeonFontSize",
                ContentService.FontSize.Size18,
                () => Strings.Setting_Raid_Font_Label,
                () => Strings.Setting_Raid_Font_Tooptip);


            DungeonPanelLabelDisplay = settings.DefineSetting("RCDungeonLabelDisplay",
                LabelDisplay.Abbreviation,
                () => Strings.Setting_Raid_LabelDisplay_Label,
                () => Strings.Setting_Raid_LabelDisplay_Tooltip);
            DungeonPanelLabelDisplay.SetExcluded(new LabelDisplay[]
            {
                LabelDisplay.WingNumber
            });

            DungeonPanelLayout = settings.DefineSetting("RCDungeonOrientation",
                Layout.Vertical,
                () => Strings.Setting_Raid_Layout_Label,
                () => "");

            DungeonPanelLabelOpacity = settings.DefineSetting("RCDungeonOpacity",
                1f,
                () => Strings.Setting_Raid_LabelOpacity_Label,
                () => Strings.Setting_Raid_LabelOpacity_Tooltip);
            DungeonPanelLabelOpacity.SetRange(0.1f, 1f);

            DungeonPanelGridOpacity = settings.DefineSetting("RCPathOpacity",
                0.8f,
                () => Strings.Setting_Raid_GridOpacity_Label,
                () => Strings.Setting_Raid_GridOpactiy_Tooltip);
            DungeonPanelGridOpacity.SetRange(0.1f, 1f);
            DungeonPanelBgOpacity = settings.DefineSetting("RCDunBGOpacity",
                0.0f,
                () => Strings.Setting_Raid_PanelOpacity_Label,
                () => Strings.Setting_Raid_PanelOpacity_Tooltip);
            DungeonPanelBgOpacity.SetRange(0.0f, 1f);

            dungeonHighlightFrequenter = settings.DefineSetting("RCDunFreqHighlight",
                true,
                () => "Hightlight Frequenter Paths",
                () => "");

            D1IsVisible = settings.DefineSetting("RCd1",
                true,
                () => "Ascalonian Catacombs",
                () => "Enable Ascalonian Catacombs on the dungeon display"
                );
            D2IsVisible = settings.DefineSetting("RCd2",
                true,
                () => "Caudecus Manor",
                () => "Enable Caudecus Manor on the dungeon display"
                );
            D3IsVisible = settings.DefineSetting("RCd3",
                true,
                () => "Twilight Arbor",
                () => "Enable Twilight Arbor on the dungeon display"
                );
            D4IsVisible = settings.DefineSetting("RCd4",
                true,
                () => "Sorrows Embrace",
                () => "Enable Sorrows Embrace on the dungeon display"
                );
            D5IsVisible = settings.DefineSetting("RCd5",
                true,
                () => "Citadel of Flame",
                () => "Enable Citadel of Flame on the dungeon display"
                );
            D6IsVisible = settings.DefineSetting("RCd6",
                true,
                () => "Honor of the Waves",
                () => "Enable Honor of the Waves on the dungeon display"
                );
            D7IsVisible = settings.DefineSetting("RCd7",
                true,
                () => "Crucible of Eternity",
                () => "Enable Crucible of Eternity on the dungeon display"
                );
            D8IsVisible = settings.DefineSetting("RCd8",
                true,
                () => "Ruined City of Arah",
                () => "Enable Ruined City of Arah on the dungeon display"
                );
            DFIsVisible = settings.DefineSetting("RCdf",
                true,
                () => "Dungeon Frequenter Summary",
                () => "Enable a dungeon frequenter achievement summary"
                );

            #endregion
            #region Dungeon Colors
            DungeonPanelColorNotCleared = settings.DefineSetting("DunColNotCleared",
               "#781414",
               () => Strings.Setting_Raid_ColNotClear_Label,
               () => Strings.Setting_Raid_ColNotClear_Tooltip);

            DungeonPanelColorCleared = settings.DefineSetting("DunColCleared",
                "#147814",
                () => Strings.Setting_Raid_ColClear_Label,
                () => Strings.Setting_Raid_ColClear_Tooltip);

            DungeonPanelColorText = settings.DefineSetting("DunColText",
                "#ffffff",
                () => Strings.Setting_Raid_ColText_Label,
                () => Strings.Setting_Raid_ColText_Tooltip);

            DungeonPanelColorFreq = settings.DefineSetting("DunColFreq",
                "#f3f527",
                () => Strings.Setting_Dun_ColFreqText_Label,
                () => Strings.Setting_Dun_ColFreqText_Tooltip);

            DungeonPanelColorBG = settings.DefineSetting("DunColBG",
                "#000000",
                () => Strings.Setting_Raid_ColBG_Label,
                () => Strings.Setting_Raid_ColBG_Tooltip);
            #endregion

            #endregion



        }

    }
}