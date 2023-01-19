using System;
using System.Threading.Tasks;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using Blish_HUD.Settings.UI.Views;

namespace RaidClears.Settings.Views;

public class CustomSettingsMenuPresenter : Presenter<CustomSettingsMenuView, ISettingsMenuRegistrar>
{
    public CustomSettingsMenuPresenter(CustomSettingsMenuView view, ISettingsMenuRegistrar model) : base(view, model) { }

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