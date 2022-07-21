using System.Diagnostics;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using Blish_HUD.Settings.UI.Views;
using Microsoft.Xna.Framework;

namespace RaidClears.Settings
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
                FlowDirection = ControlFlowDirection.SingleTopToBottom,
                CanScroll = true,
                OuterControlPadding = new Vector2(10, 20),
                ControlPadding = new Vector2(0, 10),
                Width = buildPanel.Width-10,
                HeightSizingMode    = SizingMode.Fill,
                Parent              = buildPanel
            };

            var singleColumnWidth = buildPanel.Width - ((int)_rootFlowPanel.OuterControlPadding.X * 2);
            //CreatePatchNotesButton(_rootFlowPanel);

            var generalSettingFlowPanel = CreateSettingsGroupFlowPanel("General Options", _rootFlowPanel);

            ShowSettingWithViewContainer(_settingService.RaidPanelApiPollingPeriod, generalSettingFlowPanel, singleColumnWidth);
            ShowSettingWithViewContainer(_settingService.RaidPanelIsVisibleKeyBind, generalSettingFlowPanel, singleColumnWidth);
            ShowSettingWithViewContainer(_settingService.ShowRaidsCornerIconSetting, generalSettingFlowPanel, singleColumnWidth);
            ShowSettingWithViewContainer(_settingService.RaidPanelIsVisible, generalSettingFlowPanel, singleColumnWidth);
            ShowSettingWithViewContainer(_settingService.RaidPanelAllowTooltipsSetting, generalSettingFlowPanel, singleColumnWidth);
            ShowSettingWithViewContainer(_settingService.RaidPanelDragWithMouseIsEnabledSetting, generalSettingFlowPanel, singleColumnWidth);

            var layoutFlowPanel = CreateSettingsGroupFlowPanel("Layout and Visuals", _rootFlowPanel);
            ShowSettingWithViewContainer(_settingService.RaidPanelOrientationSetting, layoutFlowPanel, singleColumnWidth);
            ShowSettingWithViewContainer(_settingService.RaidPanelFontSizeSetting, layoutFlowPanel, singleColumnWidth);
            ShowSettingWithViewContainer(_settingService.RaidPanelWingLabelsSetting, layoutFlowPanel, singleColumnWidth);
            ShowSettingWithViewContainer(_settingService.RaidPanelWingLabelOpacity, layoutFlowPanel, singleColumnWidth);
            ShowSettingWithViewContainer(_settingService.RaidPanelEncounterOpacity, layoutFlowPanel, singleColumnWidth);


            var wingSelectionFlowPanel = CreateSettingsGroupFlowPanel("Wing Selection", _rootFlowPanel);
            ShowSettingWithViewContainer(_settingService.W1IsVisibleSetting, wingSelectionFlowPanel, singleColumnWidth);
            ShowSettingWithViewContainer(_settingService.W2IsVisibleSetting, wingSelectionFlowPanel, singleColumnWidth);
            ShowSettingWithViewContainer(_settingService.W3IsVisibleSetting, wingSelectionFlowPanel, singleColumnWidth);
            ShowSettingWithViewContainer(_settingService.W4IsVisibleSetting, wingSelectionFlowPanel, singleColumnWidth);
            ShowSettingWithViewContainer(_settingService.W5IsVisibleSetting, wingSelectionFlowPanel, singleColumnWidth);
            ShowSettingWithViewContainer(_settingService.W6IsVisibleSetting, wingSelectionFlowPanel, singleColumnWidth);
            ShowSettingWithViewContainer(_settingService.W7IsVisibleSetting, wingSelectionFlowPanel, singleColumnWidth);


          
        }

        private static FlowPanel CreateSettingsGroupFlowPanel(string title, Container parent)
        {
            return new FlowPanel
            {
                Title               = title,
                FlowDirection       = ControlFlowDirection.SingleTopToBottom,
                OuterControlPadding = new Vector2(10, 10),
                ShowBorder          = true,
                Width     = parent.Width-20,//width-2(padding.x)
                //WidthSizingMode = SizingMode.Fill,
                HeightSizingMode    = SizingMode.AutoSize,
                Parent              = parent
            };
        }

        private static ViewContainer ShowSettingWithViewContainer(SettingEntry settingEntry, Container parent, int width)
        {
            var viewContainer = new ViewContainer { Parent = parent };
            viewContainer.Show(SettingView.FromType(settingEntry, parent.Width));
            return viewContainer;
        }

        private readonly SettingService _settingService;
        private FlowPanel _rootFlowPanel;

    }
}