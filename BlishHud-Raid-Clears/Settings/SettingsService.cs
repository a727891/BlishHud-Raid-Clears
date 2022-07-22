using Blish_HUD;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;



namespace RaidClears.Settings
{
    public class SettingService // singular because Setting"s"Service already exists in Blish
    {
        public SettingService(SettingCollection settings)
        {
            RaidPanelApiPollingPeriod = settings.DefineSetting("RCPoll",
                ApiPollPeriod.MINUTES_5,
                () => "Api Poll Frequency",
                () => "How often should the GW2 API be checked for updated information");
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
            RaidPanelIsVisibleKeyBind.Value.Enabled = true;

            RaidPanelDragWithMouseIsEnabledSetting = settings.DefineSetting("RCDrag",
                false,
                () => "Enable Dragging",
                () => "Click and drag to reposition the clears window.");

            RaidPanelAllowTooltipsSetting = settings.DefineSetting("RCtooltips",
                false,
                () => "Allow tooltips",
                () => "Hovering the mouse over an encounter will display the full name");

            RaidPanelFontSizeSetting = settings.DefineSetting("RCFontSize",
                ContentService.FontSize.Size11,
                () => "Font Size   ",
                () => "Change the size of the grid (Weird sizes from available fonts)");
            RaidPanelFontSizeSetting.SetExcluded( new ContentService.FontSize[] { 
                ContentService.FontSize.Size12 ,
                ContentService.FontSize.Size18 ,
                ContentService.FontSize.Size22 ,
                ContentService.FontSize.Size34 ,
                ContentService.FontSize.Size36
            } );

            RaidPanelWingLabelsSetting = settings.DefineSetting("RCLabelDisplay",
                WingLabel.Abbreviation,
                () => "Wing Label",
                () => "Display wing label as wing number or abbreviated name");

            RaidPanelOrientationSetting = settings.DefineSetting("RCOrientation",
                Orientation.Vertical,
                () => "Orientation",
                () => "Display the wings in a vertial column or horizontal row");

            RaidPanelWingLabelOpacity = settings.DefineSetting("RCWingOpacity",
                1f,
                () => "Wing Label Opacity",
                () => "Wing label transparency, Hidden <--> Full Visible");
            RaidPanelWingLabelOpacity.SetRange(0f, 1f);
            RaidPanelEncounterOpacity = settings.DefineSetting("RCEncOpacity",
                0.8f,
                () => "Encounter Opacity",
                () => "Encounter label transparency, Hidden <--> Full Visible");
            RaidPanelEncounterOpacity.SetRange(0f, 1f);

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


            var internalSettingSubCollection = settings.AddSubCollection("internal settings (not visible in UI)");
            RaidPanelLocationPoint = internalSettingSubCollection.DefineSetting("RCLocation", new Point(100, 100));



        }

        public void ToggleRaidPanelVisibility()
        {
            RaidPanelIsVisible.Value = !RaidPanelIsVisible.Value;
        }

        public bool[] GetWingVisibilitySettings()
        {
            return new bool[7] {
                W1IsVisibleSetting.Value,
                W2IsVisibleSetting.Value,
                W3IsVisibleSetting.Value,
                W4IsVisibleSetting.Value,
                W5IsVisibleSetting.Value,
                W6IsVisibleSetting.Value,
                W7IsVisibleSetting.Value
            };
        }

        public SettingEntry<ApiPollPeriod> RaidPanelApiPollingPeriod { get; }
        public SettingEntry<Point> RaidPanelLocationPoint { get; }
        public SettingEntry<bool> RaidPanelIsVisible { get; }
        public SettingEntry<bool> RaidPanelAllowTooltipsSetting { get; }
        public SettingEntry<bool> RaidPanelDragWithMouseIsEnabledSetting { get; }
        public SettingEntry<ContentService.FontSize> RaidPanelFontSizeSetting { get; }
        public SettingEntry<WingLabel> RaidPanelWingLabelsSetting { get; }
        public SettingEntry<Orientation> RaidPanelOrientationSetting { get; }
        public SettingEntry<float> RaidPanelWingLabelOpacity { get; }
        public SettingEntry<float> RaidPanelEncounterOpacity { get; }
        public SettingEntry<bool> W1IsVisibleSetting { get; }
        public SettingEntry<bool> W2IsVisibleSetting { get; }
        public SettingEntry<bool> W3IsVisibleSetting { get; }
        public SettingEntry<bool> W4IsVisibleSetting { get; }
        public SettingEntry<bool> W5IsVisibleSetting { get; }
        public SettingEntry<bool> W6IsVisibleSetting { get; }
        public SettingEntry<bool> W7IsVisibleSetting { get; }
        public SettingEntry<KeyBinding> RaidPanelIsVisibleKeyBind { get; }

        public SettingEntry<bool> ShowRaidsCornerIconSetting { get; }

    }
}