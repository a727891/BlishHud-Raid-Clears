using RaidClears.Utils;
using RaidClears.Features.Shared.Services;
using RaidClears.Features.Shared.Controls;
using RaidClears.Features.Strikes.Models;
using RaidClears.Settings.Models;
using Blish_HUD;
using Blish_HUD.Controls;
using RaidClears.Features.Strikes.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace RaidClears.Features.Strikes;

public class StrikesPanel : GridPanel
{
    private static StrikeSettings Settings => Service.Settings.StrikeSettings;

    private IEnumerable<Strike> _strikes;
    private readonly MapWatcherService _mapService;
    
    public StrikesPanel() : base(Settings.Generic, GameService.Graphics.SpriteScreen)
    {

        _mapService = Service.MapWatcher;
        _strikes = StrikeMetaData.Create(this);

        //_mapService.StrikeCompleted += _mapService_LeftStrikeMapWithCombatStartAndEnd;
        _mapService.CompletedStrikes += _mapService_CompletedStrikes;
        Service.ResetWatcher.DailyReset += UpdateClearsAtReset;
        Service.ResetWatcher.WeeklyReset += UpdateClearsAtReset;

        Service.ApiPollingService!.ApiPollingTrigger += OnApiPollingTrigger;

        Settings.Style.Color.Cleared.SettingChanged += (_, _) => ApplyEncounterBackgroundColors();
        Settings.Style.Color.NotCleared.SettingChanged += (_, _) => ApplyEncounterBackgroundColors();
        Settings.StrikePanelColorNonWeeklyBounty.SettingChanged += (_, _) => ApplyEncounterBackgroundColors();
        Settings.StrikePanelHighlightNonWeeklyBounty.SettingChanged += (_, _) => ApplyEncounterBackgroundColors();

        (this as FlowPanel).LayoutChange(Settings.Style.Layout);
        (this as GridPanel).BackgroundColorChange(Settings.Style.BgOpacity, Settings.Style.Color.Background);

        RegisterKeyBindService(
            new KeyBindHandlerService(
                Settings.Generic.ShowHideKeyBind,
                Settings.Generic.Visible
            )
        );

        ApplyEncounterBackgroundColors();
    }


    private void OnApiPollingTrigger(object sender, bool _)
    {
        Task.Run(async () =>
        {
            await WeeklyStrikeClearsService.RefreshFromApiAsync();
            GameService.Graphics.QueueMainThreadRender(_ => Service.MapWatcher.DispatchCurrentStrikeClears());
        });
    }

    private void UpdateClearsAtReset(object sender, DateTime reset)
    {
        Service.MapWatcher.DispatchCurrentStrikeClears();
    }
    private void _mapService_CompletedStrikes(object sender, List<string> strikesCompletedThisReset)
    {
        foreach (var group in _strikes)
        {
            if (group is DailyBountyTomorrow)
            {
                continue;
            }
            foreach (var encounter in group.boxes)
            {
                encounter.SetCleared(strikesCompletedThisReset.Contains(encounter.id));
            }
        }
        ApplyEncounterBackgroundColors();
    }

    /// <summary>Applies background colors to all strike encounter boxes. Cleared uses cleared color; uncleared uses non-weekly bounty color when enabled and not in weekly set, otherwise uncleared color. Daily Bounty / Daily Bounty Tomorrow groups are skipped.</summary>
    private void ApplyEncounterBackgroundColors()
    {
        var clearedColor = Settings.Style.Color.Cleared.Value.HexToXnaColor();
        var notClearedColor = Settings.Style.Color.NotCleared.Value.HexToXnaColor();
        var nonWeeklyColor = Settings.StrikePanelColorNonWeeklyBounty.Value.HexToXnaColor();
        var highlightNonWeekly = Settings.StrikePanelHighlightNonWeeklyBounty.Value;
        var weeklyBounties = Service.WeeklyBountyEncounters;

        foreach (var group in _strikes)
        {
            if (group is DailyBounty || group is DailyBountyTomorrow)
                continue;
            foreach (var encounter in group.boxes)
            {
                if (encounter.IsCleared)
                    encounter.Box.BackgroundColor = clearedColor;
                else if (highlightNonWeekly && weeklyBounties != null && !weeklyBounties.IsWeeklyBounty(encounter.id))
                    encounter.Box.BackgroundColor = nonWeeklyColor;
                else
                    encounter.Box.BackgroundColor = notClearedColor;
            }
        }

        Invalidate();
    }


    public void ForceInvalidate()
    {
    }

    protected override void DisposeControl()
    {
        base.DisposeControl();
        if (Service.ApiPollingService != null)
            Service.ApiPollingService.ApiPollingTrigger -= OnApiPollingTrigger;
        _mapService.CompletedStrikes -= _mapService_CompletedStrikes;
        Service.ResetWatcher.DailyReset -= UpdateClearsAtReset;
        Service.ResetWatcher.WeeklyReset -= UpdateClearsAtReset;
        foreach (var strike in _strikes)
        {
            strike.Dispose();
        }
    }

    public void UpdateEncounterLabel(string encounterApiId, string newLabel)
    {
        foreach (var expansion in _strikes)
        {
            if (expansion.id == encounterApiId)
            {
                expansion.GroupLabel.Text = newLabel;
                expansion.GroupLabel.Invalidate();
            }
            foreach (var encounter in expansion.boxes)
            {
                if (encounter.id == encounterApiId)
                    encounter.SetLabel(newLabel);
            }
        }
        Invalidate();
    }
}
