using Blish_HUD;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;



namespace GatheringTools.Settings
{
    public class SettingService // singular because Setting"s"Service already exists in Blish
    {
        public SettingService(SettingCollection settings)
        {

            ShowRaidsCornerIconSetting = settings.DefineSetting("RCCornerIcon",
                false,
                () => "Display top left toggle button",
                () => "Add a button next to Blish on the top left of screen that hides or shows the Raid Clears window.");

            RaidPanelIsVisible = settings.DefineSetting("RCActive",
                true,
                () => "Display on screen",
                () => "Enable the Raid Clears grid.");

            RaidPanelIsVisibleKeyBind = settings.DefineSetting("RCkeybind", new KeyBinding(Keys.None),
                () => "Display on screen keybind",
                () => "Reveal or hide the display from key press.");

            RaidPanelDragWithMouseIsEnabledSetting = settings.DefineSetting("RCDrag",
                false,
                () => "Enable Dragging",
                () => "Click and drag to reposition the display. (Overrides clickthrough)");

            RaidPanelAllowTooltipsSetting = settings.DefineSetting("RCtooltips",
                false,
                () => "Allow tooltips",
                () => "Hovering the mouse over an encounter will display the full name");

            RaidPanelFontSizeSetting = settings.DefineSetting("RCFontSize",
                ContentService.FontSize.Size12,
                () => "Font Size",
                () => "Change the size of the grid");

            RaidPanelWingLabelsSetting = settings.DefineSetting("RCLabelDisplay",
                WingLabel.Abbreviation,
                () => "Wing Label",
                () => "Display wing label as wing number or abbreviated name");

            RaidPanelOrientationSetting = settings.DefineSetting("RCOrientation",
                Orientation.Vertical,
                () => "Orientation",
                () => "Display the wings in a vertial column or horizontal row");

            #region WingVisibilitySettings
            W1IsVisibleSetting = settings.DefineSetting("RCw1",
                true,
                () => "W1 / Spirit Vale",
                () => "Enable Spirit Vale on the main display"
                );
            W2IsVisibleSetting = settings.DefineSetting("RCw2",
                true,
                () => "W2 / Salvation Pass",
                () => "Enable Salvation Pass on the main display"
                );
            W3IsVisibleSetting = settings.DefineSetting("RCw3",
                true,
                () => "W3 / Stronghold of the Faithful",
                () => "Enable Stronghold of the Faithful on the main display"
                );
            W4IsVisibleSetting = settings.DefineSetting("RCw4",
                true,
                () => "W4 / Bastion of the Penitent",
                () => "Enable Bastion of the Penitent on the main display"
                );
            W5IsVisibleSetting = settings.DefineSetting("RCw5",
                true,
                () => "W5 / Hall of Chains",
                () => "Enable Hall of Chains on the main display"
                );
            W6IsVisibleSetting = settings.DefineSetting("RCw6",
                true,
                () => "W6 / Mythwright Gambit",
                () => "Enable Mythwright Gambit on the main display"
                );
            W7IsVisibleSetting = settings.DefineSetting("RCw7",
                true,
                () => "W7 / The Key of Ahdashim",
                () => "Enable The Key of Ahdashim on the main display"
                );
            #endregion

            RaidPanelLocationPoint = settings.DefineSetting("RCLocation",
                    new Point(100, 100),
                    () => "",
                    () => "");


            var internalSettingSubCollection = settings.AddSubCollection("internal settings (not visible in UI)");
            RaidPanelLocationPoint = internalSettingSubCollection.DefineSetting("RCLocation", new Point(100, 100));



        }

        public void ToggleRaidPanelVisibility()
        {
            RaidPanelIsVisible.Value = !RaidPanelIsVisible.Value;
        }

        public SettingEntry<Point> RaidPanelLocationPoint { get; }
        public SettingEntry<bool> RaidPanelIsVisible { get; }
        public SettingEntry<bool> RaidPanelAllowTooltipsSetting { get; }
        public SettingEntry<bool> RaidPanelDragWithMouseIsEnabledSetting { get; }
        public SettingEntry<ContentService.FontSize> RaidPanelFontSizeSetting { get; }
        public SettingEntry<WingLabel> RaidPanelWingLabelsSetting { get; }
        public SettingEntry<Orientation> RaidPanelOrientationSetting { get; }
        public SettingEntry<bool> W1IsVisibleSetting { get; }
        public SettingEntry<bool> W2IsVisibleSetting { get; }
        public SettingEntry<bool> W3IsVisibleSetting { get; }
        public SettingEntry<bool> W4IsVisibleSetting { get; }
        public SettingEntry<bool> W5IsVisibleSetting { get; }
        public SettingEntry<bool> W6IsVisibleSetting { get; }
        public SettingEntry<bool> W7IsVisibleSetting { get; }
        public SettingEntry<KeyBinding> RaidPanelIsVisibleKeyBind { get; }

        public SettingEntry<bool> ShowRaidsCornerIconSetting { get; }

        public SettingEntry<bool> LogoutButtonIsVisible { get; }
        public SettingEntry<bool> LogoutButtonDragWithMouseIsEnabledSetting { get; }

        public SettingEntry<KeyBinding> LogoutKeyBindingSetting { get; }

        public const int DEFAULT_LOGOUT_BUTTON_POSITION_Y = 50;
    }
}