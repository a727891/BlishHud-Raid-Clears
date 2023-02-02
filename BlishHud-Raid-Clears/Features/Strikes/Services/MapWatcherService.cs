using Blish_HUD;
using RaidClears.Features.Raids.Models;
using RaidClears.Features.Shared.Enums;
using RaidClears.Features.Shared.Enums.Extensions;
using RaidClears.Features.Shared.Models;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RaidClears.Features.Strikes.Services;

public class MapWatcherService: IDisposable
{
    protected bool _isOnStrikeMap = false;
    protected bool _enteredCombat = false;
    protected bool _leftCombat = false;
    protected string _strikeApiName = string.Empty;
    protected string _strikeName = string.Empty;

    public event EventHandler<string>? LeftStrikeMapWithCombatStartAndEnd;

    public MapWatcherService()
    {
        GameService.Gw2Mumble.CurrentMap.MapChanged += CurrentMap_MapChanged;
        GameService.Gw2Mumble.PlayerCharacter.IsInCombatChanged += PlayerCharacter_IsInCombatChanged;

#if DEBUG
        Task.Delay(500).ContinueWith(_ =>
        {
            CurrentMap_MapChanged(this, new ValueEventArgs<int>((int)MapIds.StrikeMaps.AetherbladeHideout));
            PlayerCharacter_IsInCombatChanged(this, new ValueEventArgs<bool>(true));
            PlayerCharacter_IsInCombatChanged(this, new ValueEventArgs<bool>(false));
            CurrentMap_MapChanged(this, new ValueEventArgs<int>((int)-1));
            //Service.StrikeConfirmWindow.AskComplete(_strikeName, _strikeApiName, (s) => LeftStrikeMapWithCombatStartAndEnd?.Invoke(this, s));

        });
#endif
    }

    protected void Reset()
    {
        _isOnStrikeMap = false;
        _enteredCombat = false;
        _leftCombat = false;
        _strikeApiName = string.Empty;
        _strikeName = string.Empty;
    }

    private void PlayerCharacter_IsInCombatChanged(object sender, ValueEventArgs<bool> e)
    {
        if(_isOnStrikeMap && e.Value) 
        { 
            _enteredCombat = true;
        }
        if(_isOnStrikeMap  && _enteredCombat && !e.Value)
        {
            _leftCombat = true;
        }
    }

    private void CurrentMap_MapChanged(object sender, ValueEventArgs<int> e)
    {
        if (Enum.IsDefined(typeof(MapIds.StrikeMaps), e.Value))
        {
            Reset();
            _isOnStrikeMap= true;
            _strikeApiName = ((MapIds.StrikeMaps)e.Value).GetApiLabel();
            _strikeName = ((MapIds.StrikeMaps)e.Value).GetLabel();
        }
        else
        {
            switch (Service.Settings.StrikeSettings.StrikeCompletion.Value)
            {
                case Settings.Enums.StrikeComplete.MAP_CHANGE:

                    if (_isOnStrikeMap && _enteredCombat && _leftCombat)
                    {
                        //trigger update
                        LeftStrikeMapWithCombatStartAndEnd?.Invoke(this, _strikeApiName);
                    }
                    break;
                case Settings.Enums.StrikeComplete.POPUP:
                    if (_isOnStrikeMap)
                    {
                        //Ask user
                        Service.StrikeConfirmWindow.AskComplete(_strikeName, _strikeApiName, (s) => LeftStrikeMapWithCombatStartAndEnd?.Invoke(this,s));
                    }
                    break;
                default: break;
            }
            Reset();
        }
    }

    public void Dispose()
    {
        GameService.Gw2Mumble.CurrentMap.MapChanged -= CurrentMap_MapChanged;
        GameService.Gw2Mumble.PlayerCharacter.IsInCombatChanged -= PlayerCharacter_IsInCombatChanged;
    }
}