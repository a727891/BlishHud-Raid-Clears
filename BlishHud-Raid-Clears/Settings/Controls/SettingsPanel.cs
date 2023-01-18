using Blish_HUD.Controls;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using RaidClears.Localization;
using Blish_HUD;
using RaidClears.Settings.Services;
using RaidClears.Settings.Views.Tabs;

namespace RaidClears.Settings.Controls;


public static class SettingPanelFactory
{
    public static SettingsPanel Create()
    {
        var windowLocation = new Point(36, 26); //magic numbers stolen from EventTable
        var windowWidth = new Point(1100, 714);

        var windowSize = new Point(800, 600); //Resize window to desired value

        var settingsWindowSize = new Rectangle(windowLocation, windowWidth);
        var contentRegionPaddingY = settingsWindowSize.Y - 15; //More EventTable magic numbers
        var contentRegionPaddingX = settingsWindowSize.X + 46;
        var contentRegion = new Rectangle(
            contentRegionPaddingX,
            contentRegionPaddingY,
            settingsWindowSize.Width - 52,
            settingsWindowSize.Height - contentRegionPaddingY
        );

        return new SettingsPanel(
            Module.moduleInstance.TexturesService.SettingWindowBackground,
            settingsWindowSize,
            contentRegion,
            windowSize,
            GameService.Graphics.SpriteScreen
        );
    }
}
public class SettingsPanel : TabbedWindow2
{
    public SettingsPanel(
        Texture2D background, 
        Rectangle windowRegion, 
        Rectangle contentRegion, 
        Point windowSize,
        Container parent
    ): base(background, windowRegion, contentRegion, windowSize)
    {
        Id = $"{nameof(Module)}_96b38a83-4163-4d97-b894-282406b29a48";
        Emblem = Module.moduleInstance.TexturesService.SettingWindowEmblem;
        Parent = parent;
        Title = Strings.Module_Title;
        Subtitle = Strings.SettingsPanel_Subtitle;
        SavesPosition = true;

        Module.moduleInstance.SettingsService.SettingsPanelKeyBind.Value.Activated += (_, _) => ToggleWindow();

        #region RaidPanelSettings
        var raidsMenu = new MenuService();
        raidsMenu.RegisterSettingMenu(
            new MenuItem(Strings.SettingsPanel_Raids_Heading_General),
            (_) => new RaidGeneralView(),
            int.MinValue
        );
        raidsMenu.RegisterSettingMenu(
            new MenuItem(Strings.SettingsPanel_Raids_Heading_Layout),
            (_) => new RaidVisualsView(),
            int.MinValue
        );
        raidsMenu.RegisterSettingMenu(
            new MenuItem(Strings.SettingsPanel_Raids_Heading_WingSelection),
            (_) => new RaidWingSelectionView(),
            int.MinValue
        );

        Tabs.Add(
           new Tab(
               Module.moduleInstance.TexturesService.SettingTabRaid,
               //() => new Views.RaidSettingsView(Module.ModuleInstance.SettingsService),
               () => new Views.SettingsMenuView(raidsMenu),
               Strings.SettingsPanel_Tab_Raids
           ));
        #endregion

        #region DungeonPanelSettings
        var dungeonsMenu = new MenuService();
        dungeonsMenu.RegisterSettingMenu(
            new MenuItem(Strings.SettingsPanel_Dun_Heading_General),
            (_) => new DungeonGeneralView(),
            int.MinValue
        );
        
        dungeonsMenu.RegisterSettingMenu(
            new MenuItem(Strings.SettingsPanel_Dun_Heading_Layout),
            (_) => new DungeonVisualsView(),
            int.MinValue
        );
        
        dungeonsMenu.RegisterSettingMenu(
            new MenuItem(Strings.SettingsPanel_Dun_Heading_PathSelection),
            (_) => new DungeonPathSelectionView(),
            int.MinValue
        );

        Tabs.Add(
            new Tab(
                Module.moduleInstance.TexturesService.SettingTabDungeon,
                //() => new Views.DungeonSettingsView(Module.ModuleInstance.SettingsService),
                () => new Views.SettingsMenuView(dungeonsMenu),
                Strings.SettingsPanel_Tab_Dunegons
            ));
        #endregion

        #region StrikesPanelSettings
        var strikesMenu = new MenuService();
        strikesMenu.RegisterSettingMenu(
            new MenuItem(Strings.SettingsPanel_Raids_Heading_General),
            (_) => new StrikesGeneralView(),
            int.MinValue
        );
        strikesMenu.RegisterSettingMenu(
            new MenuItem(Strings.SettingsPanel_Raids_Heading_Layout),
            (_) => new StrikesVisualsView(),
            int.MinValue
        );
        strikesMenu.RegisterSettingMenu(
            new MenuItem(Strings.SettingsPanel_Strikes_Heading_Selection),
            (_) => new StrikesSelectionView(),
            int.MinValue
        );

        Tabs.Add(
           new Tab(
               Module.moduleInstance.TexturesService.SettingTabStrikes,
               //() => new Views.RaidSettingsView(Module.ModuleInstance.SettingsService),
               () => new Views.SettingsMenuView(strikesMenu),
               Strings.SettingsPanel_Tab_Strikes
           ));
        #endregion
        #region GeneralModuleSettings
        Tabs.Add(
            new Tab(
                Module.moduleInstance.TexturesService.SettingTabGeneral,
                //() => new Views.GlobalSettingsView(Module.ModuleInstance.SettingsService),
                () => new ModuleGeneralSettingView(),
                Strings.SettingsPanel_Tab_General
            ));

        #endregion

        //Make Menuviews rerender the first tab on Tabbed panel change
        TabChanged += (_, e) =>
        {
            if (e.NewValue.Name == Strings.SettingsPanel_Tab_Raids)
            {
                raidsMenu.RefreshMenuView();
            }
            else if (e.NewValue.Name == Strings.SettingsPanel_Tab_Dunegons)
            {
                dungeonsMenu.RefreshMenuView();
            }
            else if (e.NewValue.Name == Strings.SettingsPanel_Tab_Strikes)
            {
                strikesMenu.RefreshMenuView();
            }
            else if (e.NewValue.Name == Strings.SettingsPanel_Tab_General)
            {

            }
        };
    }
}
