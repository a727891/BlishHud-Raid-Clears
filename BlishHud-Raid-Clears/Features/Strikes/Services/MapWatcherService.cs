using Blish_HUD;
using RaidClears.Features.Shared.Models;
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
    protected BossEncounter? _strikeMission = null;
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

    public void MarkStrikeCompleted(BossEncounter mission)
    {
        Service.StrikePersistance.SaveClear(Service.CurrentAccountName, mission);
        DispatchCurrentStrikeClears();
    }

    public void MarkStrikeNotCompleted(BossEncounter mission)
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

    /// <summary>Marks the current strike as completed (MAP_CHANGE or POPUP) and resets state. Call when leaving a strike map to a non-strike map or when entering a different strike map.</summary>
    private async Task CompleteAndResetStrikeAsync()
    {
        if (!_isOnStrikeMap || _strikeMission == null) return;

        switch (Service.Settings.StrikeSettings.StrikeCompletion.Value)
        {
            case Settings.Enums.StrikeComplete.MAP_CHANGE:
                StrikeCompleted?.Invoke(this, _strikeApiName);
                MarkStrikeCompleted(_strikeMission);
                break;
            case Settings.Enums.StrikeComplete.POPUP:
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
                    MarkStrikeCompleted(_strikeMission);
                }
                break;
        }
        Reset();
    }

    private async void CurrentMap_MapChanged(object sender, ValueEventArgs<int> e)
    {
#if DEBUG
        Debug.WriteLine("Loaded Map " + e.ToString()+" "+e.Value.ToString());
#endif
        BossEncounter? newStrike = Service.StrikeData.GetBossEncounterByMapId(e.Value);
        if (newStrike is not null)
        {
            // Entering a strike map. If we were on a different strike, mark the previous one completed first (supports direct raid-to-raid travel).
            bool wasOnDifferentStrike = _isOnStrikeMap && _strikeMission != null && newStrike.EncounterId != _strikeMission.EncounterId;
            if (wasOnDifferentStrike)
                await CompleteAndResetStrikeAsync();

            Reset();
            _isOnStrikeMap = true;
            _strikeApiName = newStrike.EncounterId;
            _strikeName = newStrike.Name;
            _strikeMission = newStrike;
        }
        else
        {
            // Left to a non-strike map; mark current strike completed if we were on one.
            if (_isOnStrikeMap)
            {
                await CompleteAndResetStrikeAsync();
            }
        }
    }

    public void Dispose()
    {
        GameService.Gw2Mumble.CurrentMap.MapChanged -= CurrentMap_MapChanged;
        //GameService.Gw2Mumble.PlayerCharacter.IsInCombatChanged -= PlayerCharacter_IsInCombatChanged;
    }
}