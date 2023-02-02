using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Settings.UI.Views;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using RaidClears.Localization;
using RaidClears.Settings.Views.SubViews;
using RaidClears.Settings.Views.Tabs;
using RaidClears.Settings.Views;

namespace RaidClears.Settings.Controls;

public class SettingsPanel : TabbedWindow2
{
    private static Texture2D? Background => Service.Textures?.SettingWindowBackground;

    //Where on the background texture should the panel render
    private static Rectangle SettingPanelRegion => new()
    {
        Location = new Point(38, 25),
        //Location = new Point(-7, +25),
        //Size = new Point(Background!.Width, Background!.Height),
        Size = new Point(1100, 705),
    };
    
    private static Rectangle SettingPanelContentRegion => new()
    {
        Location = SettingPanelRegion.Location + new Point(52, 0),
        Size = SettingPanelRegion.Size - SettingPanelRegion.Location,
    };
    private static Point SettingPanelWindowSize => new(800, 600);

    public SettingsPanel() : base(Background, SettingPanelRegion, SettingPanelContentRegion, SettingPanelWindowSize)
    {
        Id = $"{nameof(Module)}_96b38a83-4163-4d97-b894-282406b29a48";
        Emblem = Service.Textures?.SettingWindowEmblem;
        Parent = GameService.Graphics.SpriteScreen;
        Title = Strings.Module_Title;
        Subtitle = Strings.SettingsPanel_Subtitle;
        SavesPosition = true;
        //_backgroundColor = new Color(10, 10, 10);

        Service.Settings.SettingsPanelKeyBind.Value.Activated += (_, _) => ToggleWindow();

        BuildTabs();
        
#if DEBUG
        Task.Delay(500).ContinueWith(_ => Show());
#endif
    }
    
    private void BuildTabs()
    {
        Tabs.Add(
            new Tab(
                Service.Textures?.SettingTabRaid,
                () => new CustomSettingMenuView(new RaidsSettingTab()),
                Strings.SettingsPanel_Tab_Raids
            ));
        
        Tabs.Add(new Tab(
            Service.Textures?.SettingTabDungeon,
            () => new CustomSettingMenuView(new DungeonSettingTab()),
                Strings.SettingsPanel_Tab_Dunegons
            ));
        
        Tabs.Add(new Tab(
            Service.Textures?.SettingTabStrikes,
            () => new CustomSettingMenuView(new StrikesSettingTab()),
            Strings.SettingsPanel_Tab_Strikes
        ));
        
        Tabs.Add(new Tab(
            Service.Textures?.SettingTabGeneral,
            () => new MainSettingsView(),
            Strings.SettingsPanel_Tab_General
        ));
    }
}
