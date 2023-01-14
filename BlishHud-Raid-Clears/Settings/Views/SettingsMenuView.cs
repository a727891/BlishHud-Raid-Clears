using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using Blish_HUD.Settings.UI.Presenters;
using Microsoft.Xna.Framework;

namespace RaidClears.Settings.Views
{
    public class SettingsMenuPresenter : Presenter<SettingsMenuView, ISettingsMenuRegistrar>
    {

        public SettingsMenuPresenter(SettingsMenuView view, ISettingsMenuRegistrar model) : base(view, model) { }

        protected override Task<bool> Load(IProgress<string> progress)
        {
            this.View.MenuItemSelected += OnMenuItemSelected;

            this.Model.RegistrarListChanged += OnRegistrarListChanged;

            return base.Load(progress);
        }

        private void OnRegistrarListChanged(object sender, EventArgs e) => UpdateView();

        private void OnMenuItemSelected(object sender, ControlActivatedEventArgs e)
        {
            this.View.SetSettingView(this.Model.GetMenuItemView(e.ActivatedControl as MenuItem));
        }

        protected override void UpdateView()
        {
            this.View.SetMenuItems(this.Model.GetSettingMenus());
        }

    } 
    /// <summary>
    /// Typically used with a <see cref="Presenters.SettingsMenuPresenter"/> to create
    /// a settings tab view where menu items can be registered.
    /// </summary>
    public class SettingsMenuView : View
    {

        public event EventHandler<ControlActivatedEventArgs> MenuItemSelected;

        private Menu _menuSettingsList;
        private ViewContainer _settingViewContainer;

        public SettingsMenuView(ISettingsMenuRegistrar settingsMenuRegistrar)
        {
            this.WithPresenter(new SettingsMenuPresenter(this, settingsMenuRegistrar));
        }

        protected override void Build(Container buildPanel)
        {
            FlowPanel _menuFlow = new FlowPanel()
            {
                Location = new Point(10, 10),
                Parent = buildPanel,
                Width = buildPanel.ContentRegion.Width,
                Height = buildPanel.ContentRegion.Height,
                FlowDirection = ControlFlowDirection.SingleLeftToRight,
                ControlPadding = new Vector2(5,5)
            };

            var settingsMenuSection = new Panel()
            {
                ShowBorder = true,
                Size = new Point(250, _menuFlow.Height),
                Title = "",
                Parent = _menuFlow,
                CanScroll = true,
            };

            _menuSettingsList = new Menu()
            {
                Size = settingsMenuSection.ContentRegion.Size,
                MenuItemHeight = 40,
                Parent = settingsMenuSection,
                CanSelect = true,
            };

            _menuSettingsList.ItemSelected += SettingsListMenuOnItemSelected;

            _settingViewContainer = new ViewContainer()
            {
                FadeView = true,
                Size = new Point(_menuFlow.Width - settingsMenuSection.Width - 10, _menuFlow.Height),
                Parent = _menuFlow
            };
        }

        public void SetSettingView(IView view)
        {
            _settingViewContainer.Show(view);
        }

        public void SetMenuItems(IEnumerable<MenuItem> menuItems)
        {
            if (_menuSettingsList == null) return;

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

        private void SettingsListMenuOnItemSelected(object sender, ControlActivatedEventArgs e)
        {
            this.MenuItemSelected?.Invoke(this, e);
        }

        protected override void Unload()
        {
            base.Unload();

            _menuSettingsList.ItemSelected -= SettingsListMenuOnItemSelected;
        }

    }
}
