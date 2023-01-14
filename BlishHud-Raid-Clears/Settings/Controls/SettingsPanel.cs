using Blish_HUD.Controls;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using RaidClears.Localization;
using Blish_HUD;
using RaidClears.Settings.Views;
using Blish_HUD.Overlay;
using Blish_HUD.Content;
using Blish_HUD.Overlay.UI.Views;
using RaidClears.Settings.Services;
using Blish_HUD.Settings.UI.Views;

namespace RaidClears.Settings.Controls
{

    public static class SettingPanelFactory
    {
        public static SettingsPanel Create()
        {

            Texture2D windowBackground = Module.ModuleInstance.ContentsManager.GetTexture(@"controls/window/background.png");

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
                windowBackground,
                settingsWindowSize,
                contentRegion,
                windowSize,
                GameService.Graphics.SpriteScreen,
                Module.ModuleInstance.ContentsManager
            );
        }
    }
    public class SettingsPanel : TabbedWindow2
    {

        public OverlaySettingsTab SettingsTab { get; private set; }

        public SettingsPanel(
            Texture2D background, 
            Rectangle windowRegion, 
            Rectangle contentRegion, 
            Point windowSize,
            Container parent,
            ContentsManager contentManager
        ): base(background, windowRegion, contentRegion)
        {
            Id = $"{nameof(Module)}_96b38a83-4163-4d97-b894-282406b29a48";
            Emblem = contentManager.GetTexture(@"module_profile_hero_icon.png");
            Parent = parent;
            Title = Strings.Module_Title;
            Subtitle = Strings.SettingsPanel_Subtitle;
            SavesPosition = true;
            //CanResize = true;
            //BackgroundColor = Color.Violet;

            MenuService raidsMenu = new MenuService();
            raidsMenu.RegisterSettingMenu(
                new MenuItem(Strings.SettingsPanel_Raids_Heading_General),
                (m) => new ModuleSettingsView(),
                int.MinValue
            );
            raidsMenu.RegisterSettingMenu(
                new MenuItem(Strings.SettingsPanel_Raids_Heading_Layout),
                (m) => new ModuleSettingsView(),
                int.MinValue
            );
            raidsMenu.RegisterSettingMenu(
                new MenuItem(Strings.SettingsPanel_Raids_Heading_WingSelection),
                (m) => new ModuleSettingsView(),
                int.MinValue
            );

            MenuService dungeonsMenu = new MenuService();
            dungeonsMenu.RegisterSettingMenu(
                new MenuItem(Strings.SettingsPanel_Raids_Heading_General),
                (m) => new ModuleSettingsView(),
                int.MinValue
            );
            dungeonsMenu.RegisterSettingMenu(
                new MenuItem(Strings.SettingsPanel_Raids_Heading_Layout),
                (m) => new ModuleSettingsView(),
                int.MinValue
            );
            dungeonsMenu.RegisterSettingMenu(
                new MenuItem(Strings.SettingsPanel_Raids_Heading_WingSelection),
                (m) => new ModuleSettingsView(),
                int.MinValue
            );


            Tabs.Add(
                new Tab(
                    contentManager.GetTexture(@"controls/tab_icons/raid.png"),
                    //() => new Views.RaidSettingsView(Module.ModuleInstance.SettingsService),
                    () => new SettingsMenuView(raidsMenu),
                    Strings.SettingsPanel_Tab_Raids
                ));
            Tabs.Add(
                new Tab(
                    contentManager.GetTexture(@"controls/tab_icons/dungeon.png"),
                    //() => new Views.DungeonSettingsView(Module.ModuleInstance.SettingsService),
                    () => new SettingsMenuView(dungeonsMenu),
                    Strings.SettingsPanel_Tab_Dunegons
                ));
            Tabs.Add(
                new Tab(
                    contentManager.GetTexture(@"controls/tab_icons/cog.png"),
                    //() => new Views.GlobalSettingsView(Module.ModuleInstance.SettingsService),
                    () => new ModuleSettingsView(),
                    Strings.SettingsPanel_Tab_General
                ));


            Show();

        }

       

    }
}
