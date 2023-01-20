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

public class RaidsSettingTab : ISettingsMenuRegistrar
{
    public event EventHandler<EventArgs>? RegistrarListChanged;
    private readonly List<MenuViewItem> _registeredMenuItems = new();
    
    public RaidsSettingTab()
    {
        _registeredMenuItems.Add(new MenuViewItem(
            new MenuItem(Strings.SettingsPanel_Raids_Heading_General),
            _ => new GenericGeneralView(Service.Settings.RaidSettings.Generic, new List<SettingEntry>
            {
                Service.Settings.RaidSettings.RaidPanelHighlightEmbolden,
                Service.Settings.RaidSettings.RaidPanelHighlightCotM,
            })));
        
        _registeredMenuItems.Add(new MenuViewItem(
            new MenuItem(Strings.SettingsPanel_Raids_Heading_Layout),
            _ => new GenericStyleView(Service.Settings.RaidSettings.Style, new List<SettingEntry<string>>
            {
                Service.Settings.RaidSettings.RaidPanelColorEmbolden,
                Service.Settings.RaidSettings.RaidPanelColorCotm
            })));
        
        _registeredMenuItems.Add(new MenuViewItem(
            new MenuItem(Strings.SettingsPanel_Raids_Heading_WingSelection),
            _ => new RaidWingSelectionView(Service.Settings.RaidSettings)
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