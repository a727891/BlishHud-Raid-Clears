using Blish_HUD;
using Gw2Sharp.Mumble;
using Gw2Sharp.WebApi.V2.Models;
using RaidClears.Features.Shared.Enums;
using RaidClears.Features.Shared.Enums.Extensions;
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
    protected Encounters.StrikeMission? _strikeMission = null;
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
            CurrentMap_MapChanged(this, new ValueEventArgs<int>((int)MapIds.StrikeMaps.OldLionsCourt));
            CurrentMap_MapChanged(this, new ValueEventArgs<int>((int)-1));

        });
#endif
    }
 
    public void DispatchCurrentStrikeClears()
    {
        Dictionary<Encounters.StrikeMission, DateTime> clears = new();

        if (!Service.StrikePersistance.AccountClears.TryGetValue(Service.CurrentAccountName, out clears))
        {
            clears = new();
        }

        List<string> clearedStrikesThisReset = new();

        foreach (KeyValuePair<Encounters.StrikeMission, DateTime> entry in clears)
        {
            if(entry.Key.GetExpansionType()== StrikeMissionType.Ibs && entry.Value >= Service.ResetWatcher.LastDailyReset)
            {     
                clearedStrikesThisReset.Add(entry.Key.GetApiLabel());
                clearedStrikesThisReset.Add($"priority_{entry.Key.GetApiLabel()}");

            }
            if (
                (entry.Key.GetExpansionType() == StrikeMissionType.Eod || entry.Key.GetExpansionType() == StrikeMissionType.SotO)
                && entry.Value >= Service.ResetWatcher.LastWeeklyReset)
            {
                clearedStrikesThisReset.Add(entry.Key.GetApiLabel());
                if(entry.Value >= Service.ResetWatcher.LastDailyReset)
                {
                    clearedStrikesThisReset.Add($"priority_{entry.Key.GetApiLabel()}");
                }
            }
            if (
                (entry.Key.GetExpansionType() == StrikeMissionType.Eod || entry.Key.GetExpansionType() == StrikeMissionType.SotO)
                 && entry.Value >= Service.ResetWatcher.LastDailyReset)
            {
                clearedStrikesThisReset.Add($"priority_{entry.Key.GetApiLabel()}");
            }
        }

        CompletedStrikes?.Invoke(this, clearedStrikesThisReset);

    }

    public void MarkStrikeCompleted(Encounters.StrikeMission mission)
    {
        Service.StrikePersistance.SaveClear(Service.CurrentAccountName, mission);
        DispatchCurrentStrikeClears();
    }

    public void MarkStrikeNotCompleted(Encounters.StrikeMission mission)
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
        if (Enum.IsDefined(typeof(MapIds.StrikeMaps), e.Value))
        {
            Reset();
            _isOnStrikeMap= true;
            _strikeApiName = ((MapIds.StrikeMaps)e.Value).GetApiLabel();
            _strikeName = ((MapIds.StrikeMaps)e.Value).GetLabel();
            _strikeMission = ((MapIds.StrikeMaps)e.Value).GetStrikeMission();
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
                                MarkStrikeCompleted((Encounters.StrikeMission) _strikeMission);
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
                                    MarkStrikeCompleted((Encounters.StrikeMission)_strikeMission);
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