using RaidClears.Utils;
using RaidClears.Features.Shared.Services;
using RaidClears.Features.Shared.Controls;
using RaidClears.Features.Strikes.Models;
using RaidClears.Settings.Models;
using Blish_HUD;
using Blish_HUD.Controls;
using RaidClears.Features.Strikes.Services;
using System.Collections.Generic;
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

        (this as FlowPanel).LayoutChange(Settings.Style.Layout);
        (this as GridPanel).BackgroundColorChange(Settings.Style.BgOpacity, Settings.Style.Color.Background);

        RegisterKeyBindService(
            new KeyBindHandlerService(
                Settings.Generic.ShowHideKeyBind,
                Settings.Generic.Visible
            )
        );
    }


    private void UpdateClearsAtReset(object sender, DateTime reset)
    {
        Service.MapWatcher.DispatchCurrentStrikeClears();
    }
    private void _mapService_CompletedStrikes(object sender, List<string> strikesCompletedThisReset)
    {
        foreach (var group in _strikes)
        {
            foreach (var encounter in group.boxes)
            {
                encounter.SetCleared(strikesCompletedThisReset.Contains(encounter.id));
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
                return;
            }
            foreach (var encounter in expansion.boxes)
            {
                if (encounter.id == encounterApiId)
                {
                    encounter.SetLabel(newLabel);
                    //return;//Keep going for priority strikes
                }

            }
        }
    }
}
