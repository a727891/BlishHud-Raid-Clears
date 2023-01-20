using Blish_HUD.Input;
using Blish_HUD.Settings;
using RaidClears.Localization;
using Microsoft.Xna.Framework.Input;
using RaidClears.Settings.Enums;
using RaidClears.Settings.Models;

namespace RaidClears.Settings.Services;

public class SettingService // singular because Setting"s"Service already exists in Blish
{
    public SettingEntry<ApiPollPeriod> ApiPollingPeriod { get; }
    public SettingEntry<KeyBinding> SettingsPanelKeyBind { get; }
    public RaidSettings RaidSettings { get; }
    public DungeonSettings DungeonSettings { get; }
    public StrikeSettings StrikeSettings { get; }

    public SettingService(SettingCollection settings)
    {
        ApiPollingPeriod = settings.DefineSetting("RCPoll",
            ApiPollPeriod.MINUTES_5,
            () => Strings.Setting_APIPoll_Label,
            () => Strings.Setting_APIPoll_Tooltip);

        SettingsPanelKeyBind = settings.DefineSetting("RCsettingsKeybind", 
            new KeyBinding(Keys.None),
        () => Strings.Settings_Keybind_Label,
        () => Strings.Settings_Keybind_tooltip);
        SettingsPanelKeyBind.Value.Enabled = true;

        RaidSettings = new RaidSettings(settings);
        DungeonSettings = new DungeonSettings(settings);
        StrikeSettings = new StrikeSettings(settings);
    }

    public void CopyRaidVisualsToDungeons() => DungeonSettings.Style = RaidSettings.Style;
    public void CopyRaidVisualsToStrikes() => StrikeSettings.Style = RaidSettings.Style;

    // public void AlignStrikesWithRaidPanel()
    // {
    //     var raidPanel = Service.RaidsPanel;
    //     var strikeLoc = StrikeSettings.Generic.Location;
    //
    //     var padding = raidPanel.ControlPadding.ToPoint();
    //
    //     strikeLoc.Value = RaidSettings.Style.Layout switch
    //     {
    //         { Value: Layout.Horizontal or Layout.SingleRow } => raidPanel.Location + new Point(raidPanel.Size.X + padding.X, 0),
    //         { Value: Layout.Vertical or Layout.SingleColumn } => raidPanel.Location + new Point(0, raidPanel.Size.Y + padding.Y),
    //         _ => strikeLoc.Value
    //     };
    // }
}