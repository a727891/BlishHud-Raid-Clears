using Blish_HUD;
using Gw2Sharp.Mumble;
using Gw2Sharp.WebApi.V2.Models;
using RaidClears.Features.Shared.Enums;
using RaidClears.Features.Shared.Enums.Extensions;
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

    private async void CurrentMap_MapChanged(object sender, ValueEventArgs<int> e)
    {
        FractalMap? _fractalMap = Service.FractalMapData.GetFractalMapById(e.Value);
        if (_fractalMap is not null)
        {
            Reset();
            _isOnFractalMap= true;
            _fractalApiName = _fractalMap.ApiLabel;
            _fractalName = _fractalMap.Label;
            _fractal = _fractalMap;
        }
        else
        {
            if (_isOnFractalMap)
            {
                switch (Service.Settings.FractalSettings.CompletionMethod.Value)
                {
                    case Settings.Enums.StrikeComplete.MAP_CHANGE:
                            //trigger update
                            FractalComplete?.Invoke(this, _fractalApiName);
                            if (_fractal != null)
                            {
                                MarkCompleted(_fractal);
                            }
                        break;
                    case Settings.Enums.StrikeComplete.POPUP:
                            //Ask user
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
                                if (_fractal != null)
                                {
                                    MarkCompleted(_fractal);
                                }
                        }

                    
                        break;
                    default: break;
                }
                Reset();
            }
        }
    }

    public void Dispose()
    {
        GameService.Gw2Mumble.CurrentMap.MapChanged -= CurrentMap_MapChanged;
        //GameService.Gw2Mumble.PlayerCharacter.IsInCombatChanged -= PlayerCharacter_IsInCombatChanged;
    }
}