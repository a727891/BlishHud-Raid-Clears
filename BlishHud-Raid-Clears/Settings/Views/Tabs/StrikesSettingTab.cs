﻿using System;
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

public class StrikesSettingTab : ISettingsMenuRegistrar
{
    public event EventHandler<EventArgs>? RegistrarListChanged;
    private readonly List<MenuViewItem> _registeredMenuItems = new();

    public StrikesSettingTab()
    {
        _registeredMenuItems.Add(new MenuViewItem(
            new MenuItem(Strings.SettingsPanel_Strikes_Heading_Selection),
            (m) => new GenericGeneralView(Service.Settings.StrikeSettings.Generic)
        ));
        
        _registeredMenuItems.Add(new MenuViewItem(
            new MenuItem(Strings.SettingsPanel_Strikes_Heading_Selection),
            (m) => new GenericStyleView(Service.Settings.StrikeSettings.Style)
        ));
        
        _registeredMenuItems.Add(new MenuViewItem(
            new MenuItem(Strings.SettingsPanel_Strikes_Heading_Selection),
            (m) => new StrikeSelectionView(Service.Settings.StrikeSettings)
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