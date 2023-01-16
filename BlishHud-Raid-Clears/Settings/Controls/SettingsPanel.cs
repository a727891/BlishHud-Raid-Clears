using Blish_HUD.Controls;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using RaidClears.Localization;
using Blish_HUD;
using Blish_HUD.Overlay;
using Blish_HUD.Settings.UI.Views;
using RaidClears.Settings.Services;
using RaidClears.Settings.Views;
using System.Collections.Generic;
using System.Linq;
using System;

namespace RaidClears.Settings.Controls
{

    public static class SettingPanelFactory
    {
        public static SettingsPanel Create()
        {

            Point windowLocation = new Point(36, 26); //magic numbers stolen from EventTable
            Point windowWidth = new Point(1100, 714);

            Point windowSize = new Point(800, 600); //Resize window to desired value

            Rectangle settingsWindowSize = new Rectangle(windowLocation, windowWidth);
            int contentRegionPaddingY = settingsWindowSize.Y - 15; //More EventTable magic numbers
            int contentRegionPaddingX = settingsWindowSize.X + 46;
            Rectangle contentRegion = new Rectangle(
                contentRegionPaddingX,
                contentRegionPaddingY,
                settingsWindowSize.Width - 52,
                settingsWindowSize.Height - contentRegionPaddingY
            );

            return new SettingsPanel(
                Module.ModuleInstance.TexturesService.SettingWindowBackground,
                settingsWindowSize,
                contentRegion,
                windowSize,
                GameService.Graphics.SpriteScreen,
                Module.ModuleInstance.ContentsManager
            );
        }
    }
    public class SettingsPanel : TabbedWindow2, IDisposable
    {

        public OverlaySettingsTab SettingsTab { get; private set; }

        public SettingsPanel(
            Texture2D background, 
            Rectangle windowRegion, 
            Rectangle contentRegion, 
            Point windowSize,
            Container parent,
            ContentsManager contentManager
        ): base(background, windowRegion, contentRegion, windowSize)
        {
            Id = $"{nameof(Module)}_96b38a83-4163-4d97-b894-282406b29a48";
            Emblem = Module.ModuleInstance.TexturesService.SettingWindowEmblem;
            Parent = parent;
            Title = Strings.Module_Title;
            Subtitle = Strings.SettingsPanel_Subtitle;
            SavesPosition = true;

            Module.ModuleInstance.SettingsService.SettingsPanelKeyBind.Value.Activated += (s, e) => ToggleWindow();

            #region RaidPanelSettings
            MenuService raidsMenu = new MenuService();
            raidsMenu.RegisterSettingMenu(
                new MenuItem(Strings.SettingsPanel_Raids_Heading_General),
                (m) => new RaidGeneralView(),
                int.MinValue
            );
            raidsMenu.RegisterSettingMenu(
                new MenuItem(Strings.SettingsPanel_Raids_Heading_Layout),
                (m) => new RaidVisualsView(),
                int.MinValue
            );
            raidsMenu.RegisterSettingMenu(
                new MenuItem(Strings.SettingsPanel_Raids_Heading_WingSelection),
                (m) => new RaidWingSelectionView(),
                int.MinValue
            );

            Tabs.Add(
               new Tab(
                   Module.ModuleInstance.TexturesService.SettingTabRaid,
                   //() => new Views.RaidSettingsView(Module.ModuleInstance.SettingsService),
                   () => new Views.SettingsMenuView(raidsMenu),
                   Strings.SettingsPanel_Tab_Raids
               ));
            #endregion

            #region DungeonPanelSettings
            MenuService dungeonsMenu = new MenuService();
            dungeonsMenu.RegisterSettingMenu(
                new MenuItem(Strings.SettingsPanel_Dun_Heading_General),
                (m) => new DungeonGeneralView(),
                int.MinValue
            );
            dungeonsMenu.RegisterSettingMenu(
                new MenuItem(Strings.SettingsPanel_Dun_Heading_Layout),
                (m) => new DungeonVisualsView(),
                int.MinValue
            );
            dungeonsMenu.RegisterSettingMenu(
                new MenuItem(Strings.SettingsPanel_Dun_Heading_PathSelection),
                (m) => new DungeonPathSelectionView(),
                int.MinValue
            );

            Tabs.Add(
                new Tab(
                    Module.ModuleInstance.TexturesService.SettingTabDungeon,
                    //() => new Views.DungeonSettingsView(Module.ModuleInstance.SettingsService),
                    () => new Views.SettingsMenuView(dungeonsMenu),
                    Strings.SettingsPanel_Tab_Dunegons
                ));
            #endregion

            #region GeneralModuleSettings
            Tabs.Add(
                new Tab(
                    Module.ModuleInstance.TexturesService.SettingTabGeneral,
                    //() => new Views.GlobalSettingsView(Module.ModuleInstance.SettingsService),
                    () => new ModuleGeneralSettingView(),
                    Strings.SettingsPanel_Tab_General
                ));

            #endregion

            //Make Menuviews rerender the first tab on Tabbed panel change
            this.TabChanged += (s, e) =>
            {
                if (e.NewValue.Name == Strings.SettingsPanel_Tab_Raids)
                {
                    raidsMenu.RefreshMenuView();
                }
                else if (e.NewValue.Name == Strings.SettingsPanel_Tab_Dunegons)
                {
                    raidsMenu.RefreshMenuView();
                }
                else if (e.NewValue.Name == Strings.SettingsPanel_Tab_General)
                {

                }
            };

            
            //Show();

        }

    }
}
