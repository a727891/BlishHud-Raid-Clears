using System.Diagnostics;
using Blish_HUD.Controls;
using Blish_HUD.Content;
using Blish_HUD;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using Blish_HUD.Settings.UI.Views;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework;

namespace RaidClears.Settings
{
    public class ModuleSettingsView : View
    {
        public ModuleSettingsView(SettingService settingService, RaidClears.Module m, TextureService textureService)
        {
            _m = m;
            _settingService = settingService;
            _textureService = textureService;
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

            //var singleColumnWidth = buildPanel.Width - ((int)_rootFlowPanel.OuterControlPadding.X * 2);
            var singleColumnWidth = buildPanel.Width;
            var doubleColWidth = (singleColumnWidth / 2) - 100;

            var generalSettingFlowPanel = CreateSettingsGroupFlowPanel("General Options", _rootFlowPanel);
            generalSettingFlowPanel.CanCollapse = true;
            var col2 = CreateTwoColPanel(generalSettingFlowPanel);
            

            ShowSettingWithViewContainer(_settingService.RaidPanelApiPollingPeriod, col2, doubleColWidth);
            _apiPollLabel = CreateApiPollRemainingLabel(col2, doubleColWidth);
            ShowSettingWithViewContainer(_settingService.AllowTooltipsSetting, generalSettingFlowPanel, singleColumnWidth);
            ShowSettingWithViewContainer(_settingService.DragWithMouseIsEnabledSetting, generalSettingFlowPanel, singleColumnWidth);

            var raidsSettingFlowPanel = CreateSettingsGroupFlowPanel("Raid Settings", _rootFlowPanel);
            raidsSettingFlowPanel.CanCollapse = true;
            raidsSettingFlowPanel.Collapse();
            raidsSettingFlowPanel.ShowTint = true;


            ShowSettingWithViewContainer(_settingService.RaidPanelIsVisibleKeyBind, raidsSettingFlowPanel, singleColumnWidth);
            ShowSettingWithViewContainer(_settingService.ShowRaidsCornerIconSetting, raidsSettingFlowPanel, singleColumnWidth);
            ShowSettingWithViewContainer(_settingService.RaidPanelIsVisible, raidsSettingFlowPanel, singleColumnWidth);

            var layoutFlowPanel = CreateSettingsGroupFlowPanel("Layout and Visuals", raidsSettingFlowPanel);
            layoutFlowPanel.CanCollapse = true; 
            ShowSettingWithViewContainer(_settingService.RaidPanelOrientationSetting, layoutFlowPanel, singleColumnWidth);
            ShowSettingWithViewContainer(_settingService.RaidPanelFontSizeSetting, layoutFlowPanel, singleColumnWidth);
            ShowSettingWithViewContainer(_settingService.RaidPanelWingLabelsSetting, layoutFlowPanel, singleColumnWidth);
            ShowSettingWithViewContainer(_settingService.RaidPanelWingLabelOpacity, layoutFlowPanel, singleColumnWidth);
            ShowSettingWithViewContainer(_settingService.RaidPanelEncounterOpacity, layoutFlowPanel, singleColumnWidth);


            var wingSelectionFlowPanel = CreateSettingsGroupFlowPanel("Wing Selection", raidsSettingFlowPanel);
            wingSelectionFlowPanel.CanCollapse = true;  
            ShowSettingWithViewContainer(_settingService.W1IsVisibleSetting, wingSelectionFlowPanel, singleColumnWidth);
            ShowSettingWithViewContainer(_settingService.W2IsVisibleSetting, wingSelectionFlowPanel, singleColumnWidth);
            ShowSettingWithViewContainer(_settingService.W3IsVisibleSetting, wingSelectionFlowPanel, singleColumnWidth);
            ShowSettingWithViewContainer(_settingService.W4IsVisibleSetting, wingSelectionFlowPanel, singleColumnWidth);
            ShowSettingWithViewContainer(_settingService.W5IsVisibleSetting, wingSelectionFlowPanel, singleColumnWidth);
            ShowSettingWithViewContainer(_settingService.W6IsVisibleSetting, wingSelectionFlowPanel, singleColumnWidth);
            ShowSettingWithViewContainer(_settingService.W7IsVisibleSetting, wingSelectionFlowPanel, singleColumnWidth);



            ShowSettingWithViewContainer(_settingService.DungeonsEnabled, _rootFlowPanel, singleColumnWidth);
            _dungeonsSettingFlowPanel = CreateSettingsGroupFlowPanel("Dungeon Settings", _rootFlowPanel);
            _dungeonsSettingFlowPanel.CanCollapse = true;
            _dungeonsSettingFlowPanel.Collapse();
            _dungeonsSettingFlowPanel.ShowTint = true;
            

            ShowSettingWithViewContainer(_settingService.DungeonPanelIsVisibleKeyBind, _dungeonsSettingFlowPanel, singleColumnWidth);
            ShowSettingWithViewContainer(_settingService.ShowDungeonCornerIconSetting, _dungeonsSettingFlowPanel, singleColumnWidth);
            ShowSettingWithViewContainer(_settingService.DungeonPanelIsVisible, _dungeonsSettingFlowPanel, singleColumnWidth);

            var dungeonLayoutFlowPanel = CreateSettingsGroupFlowPanel("Layout and Visuals", _dungeonsSettingFlowPanel);
            dungeonLayoutFlowPanel.CanCollapse = true;
            ShowSettingWithViewContainer(_settingService.DungeonPanelOrientationSetting, dungeonLayoutFlowPanel, singleColumnWidth);
            ShowSettingWithViewContainer(_settingService.DungeonPanelFontSizeSetting, dungeonLayoutFlowPanel, singleColumnWidth);
            ShowSettingWithViewContainer(_settingService.DungeonPanelWingLabelsSetting, dungeonLayoutFlowPanel, singleColumnWidth);
            ShowSettingWithViewContainer(_settingService.DungeonPanelWingLabelOpacity, dungeonLayoutFlowPanel, singleColumnWidth);
            ShowSettingWithViewContainer(_settingService.DungeonPanelEncounterOpacity, dungeonLayoutFlowPanel, singleColumnWidth);


            var dungeonSelectionFlowPanel = CreateSettingsGroupFlowPanel("Dungeon Selection", _dungeonsSettingFlowPanel);
            dungeonSelectionFlowPanel.CanCollapse = true;
            ShowSettingWithViewContainer(_settingService.D1IsVisibleSetting, dungeonSelectionFlowPanel, singleColumnWidth);
            ShowSettingWithViewContainer(_settingService.D2IsVisibleSetting, dungeonSelectionFlowPanel, singleColumnWidth);
            ShowSettingWithViewContainer(_settingService.D3IsVisibleSetting, dungeonSelectionFlowPanel, singleColumnWidth);
            ShowSettingWithViewContainer(_settingService.D4IsVisibleSetting, dungeonSelectionFlowPanel, singleColumnWidth);
            ShowSettingWithViewContainer(_settingService.D5IsVisibleSetting, dungeonSelectionFlowPanel, singleColumnWidth);
            ShowSettingWithViewContainer(_settingService.D6IsVisibleSetting, dungeonSelectionFlowPanel, singleColumnWidth);
            ShowSettingWithViewContainer(_settingService.D7IsVisibleSetting, dungeonSelectionFlowPanel, singleColumnWidth);
            ShowSettingWithViewContainer(_settingService.D8IsVisibleSetting, dungeonSelectionFlowPanel, singleColumnWidth);
            ShowSettingWithViewContainer(_settingService.DFIsVisibleSetting, dungeonSelectionFlowPanel, singleColumnWidth);


            ReloadApiPollLabelText();
            _settingService.RaidPanelApiPollingPeriod.SettingChanged += (s, e) => ReloadApiPollLabelText();

            DungeonFeatureToggled(_settingService.DungeonsEnabled.Value);
            _settingService.DungeonsEnabled.SettingChanged += (s, e) => DungeonFeatureToggled(e.NewValue);

        }

        public void DungeonFeatureToggled(bool enabled)
        {
            if (!enabled)
            {
                _dungeonsSettingFlowPanel.Hide();
            }
            else
            {
                _dungeonsSettingFlowPanel.Show();
            }
            _rootFlowPanel.Invalidate();
        }

        private static FlowPanel CreateTwoColPanel(Container parent)
        {
            return new FlowPanel
            {

                FlowDirection = ControlFlowDirection.LeftToRight,
                //OuterControlPadding = new Vector2(10, 10),
                ShowBorder = false,
                Width = parent.Width - 20,//width-2(padding.x)
                HeightSizingMode = SizingMode.AutoSize,
                Parent = parent
            };
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

        private Label CreateApiPollRemainingLabel(Container parent,int width)
        {
    
            return new Label()
            {
                AutoSizeHeight = true,
                Text = "",
                Parent = parent,
                Width = width,
            };
        }

        private void ReloadApiPollLabelText()
        {
            if (_apiPollLabel != null)
            {
                var secondsRemaining = _m.GetTimeoutSecondsRemaining();
                var labelText = $"Next Api call in ~{secondsRemaining.ToString()} seconds";
                if (secondsRemaining < 0)
                {
                    labelText = $"Waiting for a valid API token";
                }
                _apiPollLabel.Text = labelText;
            }
        }

        private FlowPanel _dungeonsSettingFlowPanel;
        private RaidClears.Module _m;
        private TextureService _textureService;
        private Label _apiPollLabel;
        private readonly SettingService _settingService;
        private FlowPanel _rootFlowPanel;

    }
}