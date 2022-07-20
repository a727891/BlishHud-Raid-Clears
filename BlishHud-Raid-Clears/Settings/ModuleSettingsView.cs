using System.Diagnostics;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using Blish_HUD.Settings.UI.Views;
using Microsoft.Xna.Framework;

namespace GatheringTools.Settings
{
    public class ModuleSettingsView : View
    {
        public ModuleSettingsView(SettingService settingService)
        {
            _settingService = settingService;
        }

        protected override void Build(Container buildPanel)
        {
            _rootFlowPanel = new FlowPanel
            {
                FlowDirection       = ControlFlowDirection.SingleTopToBottom,
                CanScroll           = true,
                OuterControlPadding = new Vector2(10, 20),
                ControlPadding      = new Vector2(0, 10),
                WidthSizingMode     = SizingMode.Fill,
                HeightSizingMode    = SizingMode.Fill,
                Parent              = buildPanel
            };

            //CreatePatchNotesButton(_rootFlowPanel);

            var generalSettingFlowPanel = CreateSettingsGroupFlowPanel("General Options", _rootFlowPanel);
            ShowSettingWithViewContainer(_settingService.ShowRaidsCornerIconSetting, generalSettingFlowPanel, buildPanel.Width);
            ShowSettingWithViewContainer(_settingService.RaidPanelIsVisible, generalSettingFlowPanel, buildPanel.Width);
            ShowSettingWithViewContainer(_settingService.RaidPanelIsVisibleKeyBind, generalSettingFlowPanel, buildPanel.Width);
            ShowSettingWithViewContainer(_settingService.RaidPanelAllowTooltipsSetting, generalSettingFlowPanel, buildPanel.Width);
            ShowSettingWithViewContainer(_settingService.RaidPanelDragWithMouseIsEnabledSetting, generalSettingFlowPanel, buildPanel.Width);

            var layoutFlowPanel = CreateSettingsGroupFlowPanel("Layout", _rootFlowPanel);
            ShowSettingWithViewContainer(_settingService.RaidPanelOrientationSetting, generalSettingFlowPanel, buildPanel.Width);

            var miscOptionsFlowPanel = CreateSettingsGroupFlowPanel("Misc. Options", _rootFlowPanel);
            ShowSettingWithViewContainer(_settingService.RaidPanelFontSizeSetting, generalSettingFlowPanel, buildPanel.Width);
            ShowSettingWithViewContainer(_settingService.RaidPanelWingLabelsSetting, generalSettingFlowPanel, buildPanel.Width);

            var wingSelectionFlowPanel = CreateSettingsGroupFlowPanel("Wing Selection", _rootFlowPanel);
            ShowSettingWithViewContainer(_settingService.W1IsVisibleSetting, generalSettingFlowPanel, buildPanel.Width);
            ShowSettingWithViewContainer(_settingService.W2IsVisibleSetting, generalSettingFlowPanel, buildPanel.Width);
            ShowSettingWithViewContainer(_settingService.W3IsVisibleSetting, generalSettingFlowPanel, buildPanel.Width);
            ShowSettingWithViewContainer(_settingService.W4IsVisibleSetting, generalSettingFlowPanel, buildPanel.Width);
            ShowSettingWithViewContainer(_settingService.W5IsVisibleSetting, generalSettingFlowPanel, buildPanel.Width);
            ShowSettingWithViewContainer(_settingService.W6IsVisibleSetting, generalSettingFlowPanel, buildPanel.Width);
            ShowSettingWithViewContainer(_settingService.W7IsVisibleSetting, generalSettingFlowPanel, buildPanel.Width);


            //ShowOrHideLogoutButtonSettings(_settingService.LogoutButtonIsVisible.Value);
            //_settingService.LogoutButtonIsVisible.SettingChanged += (s, e) => ShowOrHideLogoutButtonSettings(e.NewValue);

          
        }

       /* private void ShowOrHideLogoutButtonSettings(bool isVisible)
        {
            _logoutSetting2.Visible = isVisible;
            _logoutSetting3.Visible = isVisible;
            _logoutSetting4.Visible = isVisible;
            _logoutSetting5.Visible = isVisible;
            _logoutSetting6.Visible = isVisible;
        }*/


        private static FlowPanel CreateSettingsGroupFlowPanel(string title, Container parent)
        {
            return new FlowPanel
            {
                Title               = title,
                FlowDirection       = ControlFlowDirection.SingleTopToBottom,
                OuterControlPadding = new Vector2(10, 10),
                ShowBorder          = true,
                WidthSizingMode     = SizingMode.Fill,
                HeightSizingMode    = SizingMode.AutoSize,
                Parent              = parent
            };
        }

        private static ViewContainer ShowSettingWithViewContainer(SettingEntry settingEntry, Container parent, int width)
        {
            var viewContainer = new ViewContainer { Parent = parent };
            viewContainer.Show(SettingView.FromType(settingEntry, width));
            return viewContainer;
        }

        private readonly SettingService _settingService;
        private FlowPanel _rootFlowPanel;

    }
}