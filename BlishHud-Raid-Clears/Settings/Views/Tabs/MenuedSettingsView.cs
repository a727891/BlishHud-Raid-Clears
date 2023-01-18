using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using Blish_HUD.Settings.UI.Views;
using RaidClears.Utils;
using SettingsService = RaidClears.Settings.Services.SettingService;

namespace RaidClears.Settings.Views.Tabs;

public class MenuedSettingsView : View // warning
{
    protected SettingsService settingsService;
    protected FlowPanel rootFlowPanel;

    protected override void Build(Container buildPanel)
    {
        rootFlowPanel = new FlowPanel
        {
            Parent = buildPanel,
            FlowDirection = ControlFlowDirection.SingleTopToBottom,
            Width = buildPanel.ContentRegion.Width,
            Height = buildPanel.ContentRegion.Height,
            //BackgroundColor = new Color(20,20,100,20)
        };
    }
    
    protected static FlowPanel VisibilitySettingsFlowPanel(Container parent, SettingEntry<bool> setting)
    {
        var panel =  new FlowPanel
        {
            FlowDirection = ControlFlowDirection.SingleTopToBottom,
            Width = parent.Width,
            HeightSizingMode = SizingMode.AutoSize,
            Parent = parent
        };
        
        panel.VisiblityChanged(setting);
        return panel;
    }
    
    protected static FlowPanel VisibilityInvertedSettingsFlowPanel(Container parent, SettingEntry<bool> setting)
    {
        var panel = new FlowPanel
        {
            FlowDirection = ControlFlowDirection.SingleTopToBottom,
            Width = parent.Width,
            HeightSizingMode = SizingMode.AutoSize,
            Parent = parent
        };
        
        panel.InvertedVisiblityChanged(setting);
        return panel;
    }

    protected void ShowText(string text)
    {
        ShowText(text, rootFlowPanel);
    }
    
    protected static void ShowText(string text, FlowPanel panel) // warning still doesn't like this
    {
        var _ = new Label
        {
            Parent = panel,
            AutoSizeWidth= true,
            AutoSizeHeight= true,
            Text= text,
            WrapText= false,
        };
    }
    
    protected void AddVerticalSpacer() => AddVerticalSpacer(rootFlowPanel);
    
    protected static void AddVerticalSpacer(FlowPanel panel)
    {
        var _ = new Label
        {
            Parent = panel,
        };
    }

    protected void ShowSettingWithViewContainer(SettingEntry settingEntry) => ShowSettingWithViewContainer(settingEntry, rootFlowPanel, rootFlowPanel.Width);
    protected static void ShowSettingWithViewContainer(SettingEntry settingEntry, Container parent) => ShowSettingWithViewContainer(settingEntry, parent, parent.Width);
    private static void ShowSettingWithViewContainer(SettingEntry settingEntry, Container parent, int width)
    {
        var viewContainer = new ViewContainer { Parent = parent };
        viewContainer.Show(SettingView.FromType(settingEntry, width));
    }

    protected ViewContainer ShowColorSettingWithViewContainer(SettingEntry<string> settingEntry)
    {
        return ShowColorSettingWithViewContainer(settingEntry, rootFlowPanel, rootFlowPanel.Width);
    }
    
    protected ViewContainer ShowColorSettingWithViewContainer(SettingEntry<string> settingEntry, Container parent)
    {
        return ShowColorSettingWithViewContainer(settingEntry, parent, parent.Width);
    }
    
    protected ViewContainer ShowColorSettingWithViewContainer(SettingEntry<string> settingEntry, Container parent, int width)
    {
        var viewContainer = new ViewContainer { Parent = parent };
        viewContainer.Show(new ColorSettingView(settingEntry, width));
        return viewContainer;
    }

    protected ViewContainer ShowEnumSettingWithViewContainer(SettingEntry settingEntry)
    {
        return ShowEnumSettingWithViewContainer(settingEntry, rootFlowPanel, rootFlowPanel.Width);
    }
    protected ViewContainer ShowEnumSettingWithViewContainer(SettingEntry settingEntry, Container parent)
    {
        return ShowEnumSettingWithViewContainer(settingEntry, parent, parent.Width);
    }
    protected ViewContainer ShowEnumSettingWithViewContainer(SettingEntry settingEntry, Container parent, int width)
    {
        var viewContainer = new ViewContainer { Parent = parent };
        viewContainer.Show(EnumSettingView.FromEnum(settingEntry, width));
        return viewContainer;
    }
}