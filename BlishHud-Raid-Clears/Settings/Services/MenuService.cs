
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RaidClears.Settings.Services;

public class MenuService : ISettingsMenuRegistrar // warning
{
    private Views.SettingsMenuView View { get; set; }

    public event EventHandler<EventArgs> RegistrarListChanged;

    private readonly List<(MenuItem MenuItem, Func<MenuItem, IView> ViewFunc, int Index)> _registeredMenuItems = new();

    public void SetSettingMenuView(Views.SettingsMenuView v)
    {
        View = v;
    }

    public IView? GetMenuItemView(MenuItem selectedMenuItem)
    {
        foreach (var (menuItem, viewFunc, _) in _registeredMenuItems)
        {
            if (menuItem == selectedMenuItem || menuItem.GetDescendants().Contains(selectedMenuItem))
            {
                return viewFunc(selectedMenuItem);
            }
        }

        return null;
    }

    public void RefreshMenuView()
    {
        if (_registeredMenuItems.Count < 1) return;

        View.SetSettingView(GetMenuItemView(_registeredMenuItems.First().MenuItem));
    }

    public IEnumerable<MenuItem> GetSettingMenus() => _registeredMenuItems.OrderBy(mi => mi.Index).Select(mi => mi.MenuItem);

    public void RegisterSettingMenu(MenuItem menuItem, Func<MenuItem, IView> viewFunc, int index = 0)
    {
        _registeredMenuItems.Add((menuItem, viewFunc, index));

        RegistrarListChanged.Invoke(this, EventArgs.Empty);
    }
}
