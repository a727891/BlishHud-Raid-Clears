using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using RaidClears.Localization;
using RaidClears.Utils;

namespace RaidClears.Settings.Views.Tabs;

public class RaidsSettingTab : ISettingsMenuRegistrar
{
    public event EventHandler<EventArgs>? RegistrarListChanged;
    private readonly List<MenuViewItem> _registeredMenuItems = new();
    
    public RaidsSettingTab()
    {
        _registeredMenuItems.Add(new MenuViewItem(
            new MenuItem(Strings.SettingsPanel_Raids_Heading_General),
            (m) => new GenericGeneralView(Service.Settings.RaidSettings.Generic)
            ));
        
        _registeredMenuItems.Add(new MenuViewItem(
            new MenuItem(Strings.SettingsPanel_Raids_Heading_Layout),
            (m) => new GenericStyleView(Service.Settings.RaidSettings.Style)
        ));
        
        _registeredMenuItems.Add(new MenuViewItem(
            new MenuItem(Strings.SettingsPanel_Raids_Heading_WingSelection),
            (m) => new RaidWingSelectionView(Service.Settings.RaidSettings)
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