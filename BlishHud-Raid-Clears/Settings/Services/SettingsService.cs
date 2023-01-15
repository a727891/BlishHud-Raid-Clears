using Blish_HUD.Input;
using Blish_HUD;
using Blish_HUD.Settings;
using RaidClears.Localization;
using Microsoft.Xna.Framework.Input;
using Settings.Enums;

namespace RaidClears.Settings
{
    public class SettingService // singular because Setting"s"Service already exists in Blish
    {
        public SettingEntry<ApiPollPeriod> ApiPollingPeriod { get; }

        #region Raid Setting Variables
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
        #endregion

        public SettingService(SettingCollection settings)
        {

            #region ModuleGeneral
            ApiPollingPeriod = settings.DefineSetting("RCPoll",
                ApiPollPeriod.MINUTES_5,
                () => Strings.Setting_APIPoll_Label,
                () => Strings.Setting_APIPoll_Tooltip);

            #endregion

            #region RaidPanel
            #region Raid_General
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
               false,
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
                ContentService.FontSize.Size24,
                () => Strings.Setting_Raid_Font_Label,
                () => Strings.Setting_Raid_Font_Tooptip);

            RaidPanelLabelDisplay = settings.DefineSetting("RCLabelDisplay",
                LabelDisplay.Abbreviation,
                () => "Label style",
                () => "Display wing label as wing number or abbreviated name");

            RaidPanelLayout = settings.DefineSetting("RCOrientation",
                Layout.Vertical,
                () => Strings.Setting_Raid_Layout_Label,
                () => Strings.Setting_Raid_Layout_Tooltip);

            RaidPanelLabelOpacity = settings.DefineSetting("RCWingOpacity",
                1f,
                () =>Strings.Setting_Raid_LabelOpactiy_Label,
                () => Strings.Setting_Raid_LabelOpacity_Tooltip);
            RaidPanelLabelOpacity.SetRange(0.1f, 1f);

            RaidPanelGridOpacity = settings.DefineSetting("RCEncOpacity",
                0.8f,
                () => Strings.Setting_Raid_GridOpactiy_Label,
                () => Strings.Setting_Raid_GridOpactiy_Tooltip);
            RaidPanelGridOpacity.SetRange(0.1f, 1f);

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
            #endregion
            #endregion

            #region DungeonPanel

            #endregion



        }

    }
}