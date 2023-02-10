using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using RaidClears.Localization;
using RaidClears.Settings.Views.SubViews;
using RaidClears.Settings.Views.SubViews.Generics;
using RaidClears.Utils;

namespace RaidClears.Settings.Views.Tabs;

public class DungeonSettingTab : ISettingsMenuRegistrar
{
    public event EventHandler<EventArgs>? RegistrarListChanged;
    private readonly List<MenuViewItem> _registeredMenuItems = new();

    public DungeonSettingTab()
    {
        _registeredMenuItems.Add(new MenuViewItem(
            new MenuItem(Strings.SettingsPanel_Dun_Heading_General),
            _ => new GenericGeneralView(Service.Settings.DungeonSettings.Generic, new List<SettingEntry>
            {
                Service.Settings.DungeonSettings.DungeonHighlightFrequenter,
            })));

        _registeredMenuItems.Add(new MenuViewItem(
            new MenuItem(Strings.SettingsPanel_Dun_Heading_Layout),
            _ => new GenericStyleView(Service.Settings.DungeonSettings.Style, new List<SettingEntry<string>>
            {
                Service.Settings.DungeonSettings.DungeonPanelColorFreq,
            }, true)));
        
        _registeredMenuItems.Add(new MenuViewItem(
            new MenuItem(Strings.SettingsPanel_Dun_Heading_PathSelection),
            _ => new DungeonPathSelectionView(Service.Settings.DungeonSettings)
        ));
    }
    
    public IEnumerable<MenuItem> GetSettingMenus() => 
        _registeredMenuItems
            .Select(mi => mi.MenuItem);

    public IView? GetMenuItemView(MenuItem selectedMenuItem)
    {
        foreach (var (menuItem, viewFunc) in _registeredMenuItems) {
            if (menuItem == selectedMenuItem || menuItem.GetDescendants().Contains(selectedMenuItem)) {
                return viewFunc(selectedMenuItem);
            }
        }

        return null;
    }
}