using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using RaidClears.Settings.Services;

namespace RaidClears.Settings.Views;

public class SettingsMenuView : View
{
    public event EventHandler<ControlActivatedEventArgs> MenuItemSelected;

    private Menu _menuSettingsList;
    private ViewContainer _settingViewContainer;

    public SettingsMenuView(MenuService settingsMenuRegistrar) // warning
    {
        WithPresenter(new SettingsMenuPresenter(this, settingsMenuRegistrar));
        settingsMenuRegistrar.SetSettingMenuView(this);
    }

    protected override void Build(Container buildPanel)
    {
        var menuFlow = new FlowPanel
        {
            Location = new Point(10, 10),
            Parent = buildPanel,
            Width = buildPanel.ContentRegion.Width,
            Height = buildPanel.ContentRegion.Height,
            FlowDirection = ControlFlowDirection.SingleLeftToRight,
            ControlPadding = new Vector2(5,5)
        };

        var settingsMenuSection = new Panel
        {
            ShowBorder = true,
            Size = new Point(250, menuFlow.Height),
            Title = "",
            Parent = menuFlow,
            CanScroll = true,
        };

        _menuSettingsList = new Menu
        {
            Size = settingsMenuSection.ContentRegion.Size,
            MenuItemHeight = 40,
            Parent = settingsMenuSection,
            CanSelect = true,
        };

        _menuSettingsList.ItemSelected += SettingsListMenuOnItemSelected;

        _settingViewContainer = new ViewContainer
        {
            ShowBorder= true,
            FadeView = true,
            Size = new Point(menuFlow.Width - settingsMenuSection.Width - 10, menuFlow.Height),
            Parent = menuFlow
        };
    }

    public void SetSettingView(IView? view) => _settingViewContainer.Show(view);

    public void SetMenuItems(IEnumerable<MenuItem> menuItems)
    {
        var selectedMenuItem = _menuSettingsList.SelectedMenuItem;

        _menuSettingsList.ClearChildren();

        foreach (var menuItem in menuItems)
        {
            menuItem.Parent = _menuSettingsList;
        }

        if (selectedMenuItem?.Parent != _menuSettingsList)
        {
            _menuSettingsList.Select(_menuSettingsList.First() as MenuItem);
        }
    }

    private void SettingsListMenuOnItemSelected(object sender, ControlActivatedEventArgs e) => MenuItemSelected.Invoke(this, e);

    protected override void Unload()
    {
        base.Unload();

        _menuSettingsList.ItemSelected -= SettingsListMenuOnItemSelected;
    }
}
