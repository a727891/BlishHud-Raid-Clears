using Blish_HUD;
using RaidClears.Localization;
using RaidClears.Shared.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace RaidClears.Features.Strikes.Services;

public class MapWatcherService: IDisposable
{
    protected bool _isOnStrikeMap = false;
    //protected bool _enteredCombat = false;
    //protected bool _leftCombat = false;
    protected StrikeMission? _strikeMission = null;
    protected string _strikeApiName = string.Empty;
    protected string _strikeName = string.Empty;

    public event EventHandler<string>? StrikeCompleted;
    public event EventHandler<List<string>>? CompletedStrikes;

    public MapWatcherService()
    {
        GameService.Gw2Mumble.CurrentMap.MapChanged += CurrentMap_MapChanged;

#if DEBUG
        Task.Delay(800).ContinueWith(_ =>
        {
           /* CurrentMap_MapChanged(this, new ValueEventArgs<int>((int)MapIds.StrikeMaps.OldLionsCourt));
            CurrentMap_MapChanged(this, new ValueEventArgs<int>((int)-1));*/

        });
#endif
    }
 
    public void DispatchCurrentStrikeClears()
    {
        Dictionary<string, DateTime> clears = new();

        if (!Service.StrikePersistance.AccountClears.TryGetValue(Service.CurrentAccountName, out clears))
        {
            clears = new();
        }

        List<string> clearedStrikesThisReset = new();

        foreach (KeyValuePair<string, DateTime> entry in clears)
        {
            switch(Service.StrikeData.GetStrikeMissionResetById(entry.Key))
            {
                case "daily":
                    if(entry.Value >= Service.ResetWatcher.LastDailyReset)
                    {
                        clearedStrikesThisReset.Add(entry.Key);
                        clearedStrikesThisReset.Add($"priority_{entry.Key}");
                    }
                    break;
                default:
                    if (entry.Value >= Service.ResetWatcher.LastWeeklyReset)
                    {
                        clearedStrikesThisReset.Add(entry.Key);
                    }
                    if (entry.Value >= Service.ResetWatcher.LastDailyReset)
                    {
                        clearedStrikesThisReset.Add($"priority_{entry.Key}");
                    }
                    break;
            } 
        }

        CompletedStrikes?.Invoke(this, clearedStrikesThisReset);

    }

    public void MarkStrikeCompleted(StrikeMission mission)
    {
        Service.StrikePersistance.SaveClear(Service.CurrentAccountName, mission);
        DispatchCurrentStrikeClears();
    }

    public void MarkStrikeNotCompleted(StrikeMission mission)
    {
        Service.StrikePersistance.RemoveClear(Service.CurrentAccountName, mission);
        DispatchCurrentStrikeClears();
    }

    protected void Reset()
    {
        _strikeMission = null;
        _isOnStrikeMap = false;
        _strikeApiName = string.Empty;
        _strikeName = string.Empty;
    }

    private async void CurrentMap_MapChanged(object sender, ValueEventArgs<int> e)
    {
#if DEBUG
        Debug.WriteLine("Loaded Map " + e.ToString()+" "+e.Value.ToString());
#endif
        StrikeMission? _strikeMap = Service.StrikeData.GetStrikeMisisonByMapId(e.Value);
        if (_strikeMap is not null)
        {
            Reset();
            _isOnStrikeMap= true;
            _strikeApiName = _strikeMap.Id;
            _strikeName = _strikeMap.Name ;
            _strikeMission = _strikeMap;
        }
        else
        {
            if (_isOnStrikeMap)
            {
                switch (Service.Settings.StrikeSettings.StrikeCompletion.Value)
                {
                    case Settings.Enums.StrikeComplete.MAP_CHANGE:
                            //trigger update
                            StrikeCompleted?.Invoke(this, _strikeApiName);
                            if (_strikeMission != null)
                            {
                                MarkStrikeCompleted(_strikeMission);
                            }
                        break;
                    case Settings.Enums.StrikeComplete.POPUP:
                            //Ask user
                            var dialog = new ConfirmDialog(
                                _strikeName,
                                Strings.Strike_Confirm_Message,
                                new[] {
                                    new ButtonDefinition(Strings.Strike_Confirm_Btn_Yes, DialogResult.OK),
                                    new ButtonDefinition(Strings.Strike_Confirm_Btn_No, DialogResult.Cancel)
                                });

                            var result = await dialog.ShowDialog();
                            dialog.Dispose();

                            if (result == DialogResult.OK)
                            {

                                StrikeCompleted?.Invoke(this, _strikeApiName);
                                if (_strikeMission != null)
                                {
                                    MarkStrikeCompleted(_strikeMission);
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