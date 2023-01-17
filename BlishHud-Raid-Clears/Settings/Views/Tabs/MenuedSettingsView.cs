using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using Blish_HUD.Settings.UI.Views;
using Microsoft.Xna.Framework;
using RaidClears.Utils;
using SettingsService = RaidClears.Settings.Services.SettingService;

namespace RaidClears.Settings.Views.Tabs;

// todo: this seems super scuffed?
// There's a bunch of function chains that create objects, and then don't return them??

public class MenuedSettingsView : View
{
    protected SettingsService settingsService;
    protected FlowPanel rootFlowPanel;
    private int _singleColWidth;

    protected MenuedSettingsView()
    {
        settingsService = Module.ModuleInstance.SettingsService ;
    }

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
        
        _singleColWidth = rootFlowPanel.ContentRegion.Width;
    }

    protected static FlowPanel CreateTwoColPanel(Container parent) => new()
    {
        FlowDirection = ControlFlowDirection.LeftToRight,
        Width = parent.Width,//width-2(padding.x)
        HeightSizingMode = SizingMode.AutoSize,
        Parent = parent
    };

    protected static FlowPanel CreateSettingsGroupFlowPanel(string title, Container parent)
    {
        return new FlowPanel
        {
            Title = title,
            FlowDirection = ControlFlowDirection.SingleTopToBottom,
            OuterControlPadding = new Vector2(10, 10),
            Width = parent.Width,
            HeightSizingMode = SizingMode.AutoSize,
            Parent = parent
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

    // todo: this is probably super wrong
    protected void ShowText(string text)
    {
        ShowText(text, rootFlowPanel);
    }
    
    protected Label ShowText(string text, FlowPanel panel)
    {
        return new Label
        {
            Parent = panel,
            AutoSizeWidth= true,
            AutoSizeHeight= true,
            Text= text,
            WrapText= false,
        };
    }
    
    protected void AddVerticalSpacer() => AddVerticalSpacer(rootFlowPanel);

    protected void AddVerticalSpacer(int height) => AddVerticalSpacer(rootFlowPanel, height);

    protected void AddVerticalSpacer(FlowPanel panel)
    {
        new Label
        {
            Parent = panel,
        };
    }

    private void AddVerticalSpacer(Container panel, int height)
    {
        new Label
        {
            Parent = panel,
            Height = height,
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