using Blish_HUD;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Settings.Enums;

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

            DragWithMouseIsEnabledSetting = settings.DefineSetting("RCDrag",
                false,
                () => "Enable Dragging",
                () => "Click and drag to reposition the clears window.");

            AllowTooltipsSetting = settings.DefineSetting("RCtooltips",
                false,
                () => "Allow tooltips",
                () => "Hovering the mouse over an encounter will display the full name");

            #region RAIDS
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

            RaidPanelHighlightEmbolden = settings.DefineSetting("RCEmbolden",
                true,
                () => "Highlight the weekly 'Emboldened' raid wing",
                () => "Colors the text blue for the weekly Emboldened raid wing\nEmbolden mode increases player health, damage, and healing for each stack.");
            RaidPanelHighlightCotM = settings.DefineSetting("RCCotM",
               true,
               () => "Highlight the weekly 'Call of the Mist' raid wing",
               () => "Colors the text golden for the weekly Call of the Mists raid wing\nCall of the Mists doubles all gold in the boss loot chest");


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

            DungeonsEnabled = settings.DefineSetting("RCDungeonsEnabled",
                false,
                () => "Enable Dungeon Tracking Feature",
                () => "Turn on the daily dungeon and dungeon frequenter feature.");
            #region DUNGEONS
            ShowDungeonCornerIconSetting = settings.DefineSetting("RCDungeonCornerIcon",
                false,
                () => "Display top left toggle button",
                () => "Add a button next to Blish on the top left of screen that hides or shows the Raid Clears window.");

            DungeonPanelIsVisible = settings.DefineSetting("RCDungeonActive",
                true,
                () => "Display on screen",
                () => "Enable the Raid Clears grid.");

            DungeonPanelIsVisibleKeyBind = settings.DefineSetting("RCDungeonkeybind", new KeyBinding(Keys.None),
                () => "Display on screen keybind",
                () => "Reveal or hide the display from key press.");
            DungeonPanelIsVisibleKeyBind.Value.Enabled = true;


            DungeonPanelFontSizeSetting = settings.DefineSetting("RCDungeonFontSize",
                ContentService.FontSize.Size11,
                () => "Font Size       ",
                () => "Change the size of the grid (Weird sizes from available fonts)");
            DungeonPanelFontSizeSetting.SetExcluded(new ContentService.FontSize[] {
                ContentService.FontSize.Size12 ,
                ContentService.FontSize.Size18 ,
                ContentService.FontSize.Size22 ,
                ContentService.FontSize.Size34 ,
                ContentService.FontSize.Size36
            });

            DungeonPanelWingLabelsSetting = settings.DefineSetting("RCDungeonLabelDisplay",
                DungeonLabel.Abbreviation,
                () => "Dungeon Label",
                () => "Display wing label as wing number or abbreviated name");

            DungeonPanelOrientationSetting = settings.DefineSetting("RCDungeonOrientation",
                DungeonOrientation.Vertical,
                () => "Orientation     ",
                () => "Display the dungeons in a vertial column or horizontal row");

            DungeonPanelWingLabelOpacity = settings.DefineSetting("RCDungeonOpacity",
                1f,
                () => "Dungeon Label Opacity",
                () => "Dungeon label transparency, Hidden <--> Full Visible");
            DungeonPanelWingLabelOpacity.SetRange(0f, 1f);
            DungeonPanelEncounterOpacity = settings.DefineSetting("RCPathOpacity",
                0.8f,
                () => "Path Opacity",
                () => "Path label transparency, Hidden <--> Full Visible");
            DungeonPanelEncounterOpacity.SetRange(0f, 1f);

            
            D1IsVisibleSetting = settings.DefineSetting("RCd1",
                true,
                () => "Ascalonian Catacombs",
                () => "Enable Ascalonian Catacombs on the dungeon display"
                );
            D2IsVisibleSetting = settings.DefineSetting("RCd2",
                true,
                () => "Caudecus Manor",
                () => "Enable Caudecus Manor on the dungeon display"
                );
            D3IsVisibleSetting = settings.DefineSetting("RCd3",
                true,
                () => "Twilight Arbor",
                () => "Enable Twilight Arbor on the dungeon display"
                );
            D4IsVisibleSetting = settings.DefineSetting("RCd4",
                true,
                () => "Sorrows Embrace",
                () => "Enable Sorrows Embrace on the dungeon display"
                );
            D5IsVisibleSetting = settings.DefineSetting("RCd5",
                true,
                () => "Citadel of Flame",
                () => "Enable Citadel of Flame on the dungeon display"
                );
            D6IsVisibleSetting = settings.DefineSetting("RCd6",
                true,
                () => "Honor of the Waves",
                () => "Enable Honor of the Waves on the dungeon display"
                );
            D7IsVisibleSetting = settings.DefineSetting("RCd7",
                true,
                () => "Crucible of Eternity",
                () => "Enable Crucible of Eternity on the dungeon display"
                );
            D8IsVisibleSetting = settings.DefineSetting("RCd8",
                true,
                () => "Ruined City of Arah",
                () => "Enable Ruined City of Arah on the dungeon display"
                );
            DFIsVisibleSetting = settings.DefineSetting("RCdf",
                true,
                () => "Dungeon Frequenter Summary",
                () => "Enable a dungeon frequenter achievement summary"
                );

            #endregion

            var internalSettingSubCollection = settings.AddSubCollection("internal settings (not visible in UI)");
            RaidPanelLocationPoint = internalSettingSubCollection.DefineSetting("RCLocation", new Point(100, 100));
            DungeonPanelLocationPoint = internalSettingSubCollection.DefineSetting("RCDungeonLoc", new Point(200, 100));

        }

        public void ToggleRaidPanelVisibility()
        {
            RaidPanelIsVisible.Value = !RaidPanelIsVisible.Value;
        }

        public void ToggleDungeonPanelVisibility()
        {
            DungeonPanelIsVisible.Value = !DungeonPanelIsVisible.Value; 
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

        public bool[] GetDungeonVisibilitySettings()
        {
            return new bool[9] {
                D1IsVisibleSetting.Value,
                D2IsVisibleSetting.Value,
                D3IsVisibleSetting.Value,
                D4IsVisibleSetting.Value,
                D5IsVisibleSetting.Value,
                D6IsVisibleSetting.Value,
                D7IsVisibleSetting.Value,
                D8IsVisibleSetting.Value,
                DFIsVisibleSetting.Value,
            };
        }

        public SettingEntry<ApiPollPeriod> RaidPanelApiPollingPeriod { get; }
        public SettingEntry<Point> RaidPanelLocationPoint { get; }
        public SettingEntry<bool> RaidPanelIsVisible { get; }
        public SettingEntry<bool>  DungeonsEnabled { get; }
        public SettingEntry<bool> AllowTooltipsSetting { get; }
        public SettingEntry<bool> DragWithMouseIsEnabledSetting { get; }
        public SettingEntry<ContentService.FontSize> RaidPanelFontSizeSetting { get; }
        public SettingEntry<WingLabel> RaidPanelWingLabelsSetting { get; }
        public SettingEntry<Orientation> RaidPanelOrientationSetting { get; }
        public SettingEntry<float> RaidPanelWingLabelOpacity { get; }
        public SettingEntry<float> RaidPanelEncounterOpacity { get; }
        public SettingEntry<bool> RaidPanelHighlightEmbolden { get; }
        public SettingEntry<bool> RaidPanelHighlightCotM{ get; }
        public SettingEntry<bool> W1IsVisibleSetting { get; }
        public SettingEntry<bool> W2IsVisibleSetting { get; }
        public SettingEntry<bool> W3IsVisibleSetting { get; }
        public SettingEntry<bool> W4IsVisibleSetting { get; }
        public SettingEntry<bool> W5IsVisibleSetting { get; }
        public SettingEntry<bool> W6IsVisibleSetting { get; }
        public SettingEntry<bool> W7IsVisibleSetting { get; }
        public SettingEntry<KeyBinding> RaidPanelIsVisibleKeyBind { get; }

        public SettingEntry<bool> ShowRaidsCornerIconSetting { get; }

        #region DUNGEONS
        public SettingEntry<Point> DungeonPanelLocationPoint { get; }
        public SettingEntry<bool> DungeonPanelIsVisible { get; }

        public SettingEntry<ContentService.FontSize> DungeonPanelFontSizeSetting { get; }
        public SettingEntry<DungeonLabel> DungeonPanelWingLabelsSetting { get; }
        public SettingEntry<DungeonOrientation> DungeonPanelOrientationSetting { get; }
        public SettingEntry<float> DungeonPanelWingLabelOpacity { get; }
        public SettingEntry<float> DungeonPanelEncounterOpacity { get; }
        public SettingEntry<bool> D1IsVisibleSetting { get; }
        public SettingEntry<bool> D2IsVisibleSetting { get; }
        public SettingEntry<bool> D3IsVisibleSetting { get; }
        public SettingEntry<bool> D4IsVisibleSetting { get; }
        public SettingEntry<bool> D5IsVisibleSetting { get; }
        public SettingEntry<bool> D6IsVisibleSetting { get; }
        public SettingEntry<bool> D7IsVisibleSetting { get; }
        public SettingEntry<bool> D8IsVisibleSetting { get; }
        public SettingEntry<bool> DFIsVisibleSetting { get; }
        public SettingEntry<KeyBinding> DungeonPanelIsVisibleKeyBind { get; }

        public SettingEntry<bool> ShowDungeonCornerIconSetting { get; }

        #endregion

    }
}