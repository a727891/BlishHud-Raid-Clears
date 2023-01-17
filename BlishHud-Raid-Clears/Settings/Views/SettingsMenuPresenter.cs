﻿using System;
using System.Threading.Tasks;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;

namespace RaidClears.Settings.Views;

public class SettingsMenuPresenter : Presenter<SettingsMenuView, ISettingsMenuRegistrar>
{
    public SettingsMenuPresenter(SettingsMenuView view, ISettingsMenuRegistrar model) : base(view, model) { }

    protected override Task<bool> Load(IProgress<string> progress)
    {
        View.MenuItemSelected += OnMenuItemSelected;

        Model.RegistrarListChanged += OnRegistrarListChanged;

        return base.Load(progress);
    }

    private void OnRegistrarListChanged(object sender, EventArgs e) => UpdateView();

    private void OnMenuItemSelected(object sender, ControlActivatedEventArgs e) => View.SetSettingView(Model.GetMenuItemView(e.ActivatedControl as MenuItem));

    protected override void UpdateView() => View.SetMenuItems(Model.GetSettingMenus());
} 