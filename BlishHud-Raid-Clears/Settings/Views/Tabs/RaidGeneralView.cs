using Blish_HUD.Controls;
using RaidClears.Settings.Views.Tabs;

namespace RaidClears.Settings.Views
{
    public class RaidGeneralView : MenuedSettingsView
    {
        public RaidGeneralView()
        { 
        }

        protected override void Build(Container buildPanel)
        {
            base.Build(buildPanel);

            ShowText("This is the the raids general options and keybind panel");
            //ShowSettingWithViewContainer(_settingsService.DragWithMouseIsEnabledSetting, _rootFlowPanel);
           // ShowSettingWithViewContainer(_settingsService.RaidPanelIsVisible, _rootFlowPanel);
           // ShowSettingWithViewContainer(_settingsService.ShowRaidsCornerIconSetting, _rootFlowPanel);
           // AddVerticalSpacer();
           // ShowSettingWithViewContainer(_settingsService.RaidPanelIsVisibleKeyBind, _rootFlowPanel, _singleColWidth);

        }


    }
}