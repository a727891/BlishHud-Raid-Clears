using Blish_HUD;
using RaidClears.Localization;
using RaidClears.Shared.Controls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RaidClears.Features.Fractals.Services;

public class FractalMapWatcherService: IDisposable
{
    protected bool _isOnFractalMap = false;
    //protected bool _enteredCombat = false;
    //protected bool _leftCombat = false;
    protected FractalMap? _fractal = null;
    protected string _fractalApiName = string.Empty;
    protected string _fractalName = string.Empty;

    public event EventHandler<string>? FractalComplete;
    public event EventHandler<List<string>>? CompletedFractal;

    public FractalMapWatcherService()
    {
        GameService.Gw2Mumble.CurrentMap.MapChanged += CurrentMap_MapChanged;

#if DEBUG
        Task.Delay(800).ContinueWith(_ =>
        {
            //CurrentMap_MapChanged(this, new ValueEventArgs<int>((int)MapIds.FractalMaps.AetherbladeFractal));
            //CurrentMap_MapChanged(this, new ValueEventArgs<int>((int)-1));

        });
#endif
    }
 
    public void DispatchCurrentClears()
    {
        Dictionary<string, DateTime> clears = new();

        if (!Service.FractalPersistance.AccountClears.TryGetValue(Service.CurrentAccountName, out clears))
        {
            clears = new();
        }

        List<string> clearedStrikesThisReset = new();

        foreach (KeyValuePair<string, DateTime> entry in clears)
        {
            if(entry.Value >= Service.ResetWatcher.LastDailyReset)
            {     
                clearedStrikesThisReset.Add(entry.Key);
            }
        }

        CompletedFractal?.Invoke(this, clearedStrikesThisReset);

    }
    public void MarkCompleted(FractalMap fractal)
    {
        Service.FractalPersistance.SaveClear(Service.CurrentAccountName, fractal);
        DispatchCurrentClears();
    }

    public void MarkNotCompleted(FractalMap fractal)
    {
        Service.FractalPersistance.RemoveClear(Service.CurrentAccountName, fractal);
        DispatchCurrentClears();
    }

    protected void Reset()
    {
        _fractal = null;
        _isOnFractalMap = false;
        _fractalApiName = string.Empty;
        _fractalName = string.Empty;
    }

    /// <summary>Marks the current fractal as completed (MAP_CHANGE or POPUP) and resets state. Call when leaving a fractal map to a non-fractal map or when entering a different fractal map.</summary>
    private async Task CompleteAndResetFractalAsync()
    {
        if (!_isOnFractalMap || _fractal == null) return;

        switch (Service.Settings.FractalSettings.CompletionMethod.Value)
        {
            case Settings.Enums.StrikeComplete.MAP_CHANGE:
                FractalComplete?.Invoke(this, _fractalApiName);
                MarkCompleted(_fractal);
                break;
            case Settings.Enums.StrikeComplete.POPUP:
                var dialog = new ConfirmDialog(
                    _fractalName,
                    Strings.Strike_Confirm_Message,
                    new[] {
                        new ButtonDefinition(Strings.Strike_Confirm_Btn_Yes, DialogResult.OK),
                        new ButtonDefinition(Strings.Strike_Confirm_Btn_No, DialogResult.Cancel)
                    });
                var result = await dialog.ShowDialog();
                dialog.Dispose();
                if (result == DialogResult.OK)
                {
                    FractalComplete?.Invoke(this, _fractalApiName);
                    MarkCompleted(_fractal);
                }
                break;
        }
        Reset();
    }

    private async void CurrentMap_MapChanged(object sender, ValueEventArgs<int> e)
    {
        FractalMap? newFractal = Service.FractalMapData.GetFractalMapById(e.Value);
        if (newFractal is not null)
        {
            // Entering a fractal map. If we were on a different fractal, mark the previous one completed first (supports direct fractal-to-fractal travel).
            bool wasOnDifferentFractal = _isOnFractalMap && _fractal != null && newFractal.ApiLabel != _fractal.ApiLabel;
            if (wasOnDifferentFractal)
                await CompleteAndResetFractalAsync();

            Reset();
            _isOnFractalMap = true;
            _fractalApiName = newFractal.ApiLabel;
            _fractalName = newFractal.Label;
            _fractal = newFractal;
        }
        else
        {
            // Left to a non-fractal map; mark current fractal completed if we were on one.
            if (_isOnFractalMap)
            {
                await CompleteAndResetFractalAsync();
            }
        }
    }

    public void Dispose()
    {
        GameService.Gw2Mumble.CurrentMap.MapChanged -= CurrentMap_MapChanged;
        //GameService.Gw2Mumble.PlayerCharacter.IsInCombatChanged -= PlayerCharacter_IsInCombatChanged;
    }
}