using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Settings.UI.Views;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using RaidClears.Localization;
using RaidClears.Settings.Views.Tabs;

namespace RaidClears.Settings.Controls;

public class SettingsPanel : TabbedWindow2
{
    private static Texture2D? Background => Service.TexturesService?.SettingWindowBackground;

    private static Rectangle Region => new()
    {
        Location = new Point(-7, +25),
        Size = new Point(1155, 710),
    };
    
    private static Rectangle WindowSize => new()
    {
        Location = new Point(52, 25),
        Size = Region.Size - Region.Location,
    };

    public SettingsPanel() : base(Background, Region, WindowSize)
    {
        Id = $"{nameof(Module)}_96b38a83-4163-4d97-b894-282406b29a48";
        Emblem = Service.TexturesService?.SettingWindowEmblem;
        Parent = GameService.Graphics.SpriteScreen;
        Title = Strings.Module_Title;
        Subtitle = Strings.SettingsPanel_Subtitle;
        SavesPosition = true;
        _backgroundColor = new Color(10, 10, 10);

        Service.Settings.SettingsPanelKeyBind.Value.Activated += (_, _) => ToggleWindow();

        BuildTabs();
        
        Task.Delay(500).ContinueWith(_ => Show());
    }
    
    private void BuildTabs()
    {
        Tabs.Add(
            new Tab(
                Service.TexturesService?.SettingTabRaid,
                () => new SettingsMenuView(new RaidsSettingTab()),
                Strings.SettingsPanel_Tab_Raids
            ));
        
        Tabs.Add(new Tab(
            Service.TexturesService?.SettingTabDungeon,
            () => new SettingsMenuView(new DungeonSettingTab()),
                Strings.SettingsPanel_Tab_Dunegons
            ));
        
        Tabs.Add(new Tab(
            Service.TexturesService?.SettingTabGeneral,
            () => new ModuleGeneralSettingView(),
            Strings.SettingsPanel_Tab_General
        ));
    }
}
