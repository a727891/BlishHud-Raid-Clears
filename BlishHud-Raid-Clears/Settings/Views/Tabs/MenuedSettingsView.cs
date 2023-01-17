
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using Blish_HUD.Settings.UI.Views;
using Microsoft.Xna.Framework;
using RaidClears.Utils;
using SettingsService = RaidClears.Settings.Services.SettingService;

namespace RaidClears.Settings.Views.Tabs;


public class MenuedSettingsView : View
{
    protected SettingsService _settingsService;
    protected FlowPanel _rootFlowPanel;
    protected int _singleColWidth;

    public MenuedSettingsView()
    {
        _settingsService = Module.ModuleInstance.SettingsService ;


    }

    protected override void Build(Container buildPanel)
    {

        _rootFlowPanel = new FlowPanel()
        {
            Parent = buildPanel,
            FlowDirection = ControlFlowDirection.SingleTopToBottom,
            Width = buildPanel.ContentRegion.Width,
            Height = buildPanel.ContentRegion.Height,
            //BackgroundColor = new Color(20,20,100,20)
        };
        _singleColWidth = _rootFlowPanel.ContentRegion.Width;
                    
    }


    protected override void Unload()
    {
        base.Unload();

    }


    protected static FlowPanel CreateTwoColPanel(Container parent)
    {
        return new FlowPanel
        {

            FlowDirection = ControlFlowDirection.LeftToRight,
            Width = parent.Width,//width-2(padding.x)
            HeightSizingMode = SizingMode.AutoSize,
            Parent = parent
        };
    }
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
    
    protected static FlowPanel VisibibilitySettingsFlowPanel(Container parent, SettingEntry<bool> setting)
    {
        FlowPanel panel =  new FlowPanel
        {
            FlowDirection = ControlFlowDirection.SingleTopToBottom,
            Width = parent.Width,
            HeightSizingMode = SizingMode.AutoSize,
            Parent = parent
        };
        panel.VisiblityChanged(setting);
        return panel;
    }
    protected static FlowPanel VisibibilityInvertedSettingsFlowPanel(Container parent, SettingEntry<bool> setting)
    {
        FlowPanel panel = new FlowPanel
        {
            FlowDirection = ControlFlowDirection.SingleTopToBottom,
            Width = parent.Width,
            HeightSizingMode = SizingMode.AutoSize,
            Parent = parent
        };
        panel.InvertedVisiblityChanged(setting);
        return panel;
    }

    protected Label ShowText(string text)
    {
        return ShowText(text, _rootFlowPanel);
    }
    protected Label ShowText(string text, FlowPanel panel)
    {
        return new Label()
        {
            Parent = panel,
            AutoSizeWidth= true,
            AutoSizeHeight= true,
            Text= text,
            WrapText= false,
        };
    }
    protected void AddVerticalSpacer()
    {
        AddVerticalSpacer(_rootFlowPanel);
    }
    protected void AddVerticalSpacer(int height)
    {
        AddVerticalSpacer(_rootFlowPanel, height);
    }
    protected void AddVerticalSpacer(FlowPanel panel)
    {
        new Label()
        {
            Parent = panel,
        };
    }
    protected void AddVerticalSpacer(FlowPanel panel, int height)
    {
        new Label()
        {
            Parent = panel,
            Height = height,
        };
    }

    protected  ViewContainer ShowSettingWithViewContainer(SettingEntry settingEntry)
    {
        return ShowSettingWithViewContainer(settingEntry, _rootFlowPanel, _rootFlowPanel.Width);
    }
    protected  ViewContainer ShowSettingWithViewContainer(SettingEntry settingEntry, Container parent)
    {
        return ShowSettingWithViewContainer(settingEntry, parent, parent.Width);
    }
    protected ViewContainer ShowSettingWithViewContainer(SettingEntry settingEntry, Container parent, int width)
    {
        var viewContainer = new ViewContainer { Parent = parent };
        viewContainer.Show(SettingView.FromType(settingEntry, width));
        return viewContainer;
    }

    protected ViewContainer ShowColorSettingWithViewContainer(SettingEntry<string> settingEntry)
    {
        return ShowColorSettingWithViewContainer(settingEntry, _rootFlowPanel, _rootFlowPanel.Width);
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
        return ShowEnumSettingWithViewContainer(settingEntry, _rootFlowPanel, _rootFlowPanel.Width);
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