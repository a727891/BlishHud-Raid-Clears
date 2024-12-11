using RaidClears.Utils;
using RaidClears.Features.Shared.Services;
using RaidClears.Features.Shared.Controls;
using RaidClears.Settings.Models;
using Blish_HUD;
using Blish_HUD.Controls;
using System.Collections.Generic;
using System;
using RaidClears.Features.Fractals.Models;
using RaidClears.Features.Fractals.Services;

namespace RaidClears.Features.Fractals;

public class FractalsPanel : GridPanel
{
    private static FractalSettings Settings => Service.Settings.FractalSettings;

    private readonly IEnumerable<Fractal> _fractals;
    private readonly FractalMapWatcherService _mapService;
    
    public FractalsPanel() : base(Settings.Generic, GameService.Graphics.SpriteScreen)
    {

        _mapService = Service.FractalMapWatcher;
        _fractals = FractalMetaData.Create(this);

        //_mapService.FractalComplete += _mapService_LeftStrikeMapWithCombatStartAndEnd;
       _mapService.CompletedFractal += _mapService_CompletedStrikes;
        Service.ResetWatcher.DailyReset += UpdateClearsAtReset;

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
        //Service.MapWatcher.DispatchCurrentClears();
    }
    private void _mapService_CompletedStrikes(object sender, List<string> strikesCompletedThisReset)
    {
        foreach (var group in _fractals)
        {
            if(group.GetType() == typeof(TierNTomorrow)){
                continue;
            }
            foreach (var encounter in group.boxes)
            {
                encounter.SetCleared(strikesCompletedThisReset.Contains(encounter.id));
            }
        }
        Invalidate();
    }

    public void ForceInvalidate()
    {
        /*foreach(var strike in _fractals)
        {
            foreach( var s in strike.boxes)
            {
s               s.Box?.Parent.Invalidate();
            }
        }*/
    }

    protected override void DisposeControl()
    {
       base.DisposeControl();
       // _mapService.FractalComplete -= _mapService_LeftStrikeMapWithCombatStartAndEnd;
        _mapService.CompletedFractal -= _mapService_CompletedStrikes;
        Service.ResetWatcher.DailyReset -= UpdateClearsAtReset;
        Service.ResetWatcher.WeeklyReset -= UpdateClearsAtReset;
        foreach (var strike in _fractals)
        {
            strike.Dispose();
        }
    }

    public void UpdateEncounterLabel(string encounterApiId, string newLabel)
    {
        foreach (var fractalGroup in _fractals)
        {
            if (fractalGroup.id == encounterApiId)
            {
                fractalGroup.GroupLabel.Text = newLabel;
                return;
            }
            foreach (var fractal in fractalGroup.boxes)
            {
                if (fractal.id == encounterApiId)
                {
                    fractal.SetLabel(newLabel);
                    return;
                }

            }
        }
    }
}
