using Blish_HUD;
using Blish_HUD.Controls;
using RaidClears.Settings.Views.Tabs;

namespace RaidClears.Settings.Views
{
    public class RaidVisualsView : MenuedSettingsView
    {
        public RaidVisualsView()
        { 
        }

        protected override void Build(Container buildPanel)
        {
            base.Build(buildPanel);
            ShowEnumSettingWithViewContainer(_settingsService.RaidPanelLayout);
            ShowEnumSettingWithViewContainer(_settingsService.RaidPanelFontSize);
            ShowEnumSettingWithViewContainer(_settingsService.RaidPanelLabelDisplay);
            ShowSettingWithViewContainer(_settingsService.RaidPanelLabelOpacity);
            ShowSettingWithViewContainer(_settingsService.RaidPanelGridOpacity);
            ShowSettingWithViewContainer(_settingsService.RaidPanelHighlightEmbolden);
            ShowSettingWithViewContainer(_settingsService.RaidPanelHighlightCotM);

            //ShowColorSettingWithViewContainer(SettingsService.RaidPanelColorUnknown);
            new Label
            {
                AutoSizeHeight = true,
                Parent = _rootFlowPanel,
                Text = "Customize the colors by entering a Hex color code. (Tip: Google 'color picker' for help)",
                Width = _singleColWidth,
                WrapText = true,
            };
            ShowColorSettingWithViewContainer(_settingsService.RaidPanelColorNotCleared);
            ShowColorSettingWithViewContainer(_settingsService.RaidPanelColorCleared);
            ShowColorSettingWithViewContainer(_settingsService.RaidPanelColorCotm);
            ShowColorSettingWithViewContainer(_settingsService.RaidPanelColorEmbolden);
            ShowColorSettingWithViewContainer(_settingsService.RaidPanelColorText);


        }


    }
}