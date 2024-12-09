using Blish_HUD.Input;
using Blish_HUD.Settings;
using RaidClears.Localization;
using Microsoft.Xna.Framework.Input;
using RaidClears.Settings.Enums;
using RaidClears.Settings.Models;
using Microsoft.Xna.Framework;
using System.Runtime.Serialization.Formatters;
namespace RaidClears.Settings.Services;

public class SettingService // singular because Setting"s"Service already exists in Blish
{
    public SettingEntry<ApiPollPeriod> ApiPollingPeriod { get; }
    public SettingEntry<KeyBinding> SettingsPanelKeyBind { get; }
    public SettingEntry<bool> GlobalCornerIconEnabled { get; }
    public SettingEntry<bool> ScreenClamp { get; }

    public SettingEntry<bool> OrganicGridBoxBackgrounds { get; }

    public SettingEntry<int> CornerIconPriority { get; }
    public RaidSettings RaidSettings { get; }
    public DungeonSettings DungeonSettings { get; }
    public StrikeSettings StrikeSettings { get; }
    public FractalSettings FractalSettings { get; }

    public SettingService(SettingCollection settings)
    {
        ApiPollingPeriod = settings.DefineSetting("RCPoll",
            ApiPollPeriod.MINUTES_5,
            () => Strings.Setting_APIPoll_Label,
            () => Strings.Setting_APIPoll_Tooltip);

        CornerIconPriority = settings.DefineSetting("RCCornerPriority",
            53, //.947 of Int32MaxValue to match the original corner icon priority
            () => Strings.CornerIconPriority_Label,
            () => Strings.CornerIconPriority_Tooltlp);
        CornerIconPriority.SetRange(0, 1000);

        SettingsPanelKeyBind = settings.DefineSetting("RCsettingsKeybind",
            new KeyBinding(Keys.None),
        () => Strings.Settings_Keybind_Label,
        () => Strings.Settings_Keybind_tooltip);
        SettingsPanelKeyBind.Value.Enabled = true;

        GlobalCornerIconEnabled = settings.DefineSetting("RCGlobalCornerIcon",
            true,
            () => Strings.Setting_CornerIconEnable,
            () => Strings.Setting_CornerIconEnableTooltip);

        ScreenClamp = settings.DefineSetting("RCScreenClamp",
            true,
            () => "Keep overlay windows on screen",
            () => "When turned on, this will make sure that all overlay windows stay within the visible area of your screen, so they don't go off the edges"
            );

        OrganicGridBoxBackgrounds = settings.DefineSetting("RCStylize",
            true,
            () => "'GW2 Style' background boxes",
            () => "On: Backgrounds will appear with fuzzy edges more akin to GW2's style,\nOff: Background will be rectangles"
            );

        RaidSettings = new RaidSettings(settings);
        DungeonSettings = new DungeonSettings(settings);
        StrikeSettings = new StrikeSettings(settings);
        FractalSettings = new FractalSettings(settings);

        StrikeSettings.AnchorToRaidPanel.SettingChanged += (_, e) => { if (e.NewValue) AlignStrikesWithRaidPanel(); };
        RaidSettings.Generic.Location.SettingChanged += (_, e) => { if (StrikeSettings.AnchorToRaidPanel.Value) AlignStrikesWithRaidPanel(); };
    }

    public void CopyRaidSettings(DisplayStyle settings)
    {
        settings.Layout.Value = RaidSettings.Style.Layout.Value;
        settings.LabelDisplay.Value = RaidSettings.Style.LabelDisplay.Value;
        settings.LabelOpacity.Value = RaidSettings.Style.LabelOpacity.Value;
        settings.GridOpacity.Value = RaidSettings.Style.GridOpacity.Value;
        settings.BgOpacity.Value = RaidSettings.Style.BgOpacity.Value;
        settings.FontSize.Value = RaidSettings.Style.FontSize.Value;


        settings.Color.Background.Value = RaidSettings.Style.Color.Background.Value;
        settings.Color.NotCleared.Value = RaidSettings.Style.Color.NotCleared.Value;
        settings.Color.Cleared.Value = RaidSettings.Style.Color.Cleared.Value;
        settings.Color.Text.Value = RaidSettings.Style.Color.Text.Value;

    }

    public void AlignStrikesWithRaidPanel()
    {
        var raidPanel = Service.RaidWindow;
        var strikeLoc = StrikeSettings.Generic.Location;
   
        var padding = raidPanel.ControlPadding.ToPoint();
   
        strikeLoc.Value = RaidSettings.Style.Layout switch
        {
            { Value: Layout.Horizontal or Layout.SingleRow } => raidPanel.Location + new Point(raidPanel.Size.X + padding.X, 0),
            { Value: Layout.Vertical or Layout.SingleColumn } => raidPanel.Location + new Point(0, raidPanel.Size.Y + padding.Y),
            _ => strikeLoc.Value
       };
    }


}