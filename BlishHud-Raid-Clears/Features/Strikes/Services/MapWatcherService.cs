using Blish_HUD;
using RaidClears.Features.Raids.Models;
using RaidClears.Features.Shared.Enums;
using RaidClears.Features.Shared.Enums.Extensions;
using RaidClears.Features.Shared.Models;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RaidClears.Features.Strikes.Services;

public class MapWatcherService: IDisposable
{
    protected bool _isOnStrikeMap = false;
    protected bool _enteredCombat = false;
    protected bool _leftCombat = false;
    protected string _strikeApiName = string.Empty;

    public event EventHandler<string>? LeftStrikeMapWithCombatStartAndEnd;

    public MapWatcherService()
    {
        GameService.Gw2Mumble.CurrentMap.MapChanged += CurrentMap_MapChanged;
        GameService.Gw2Mumble.PlayerCharacter.IsInCombatChanged += PlayerCharacter_IsInCombatChanged;
    }

    protected void Reset()
    {
        _isOnStrikeMap = false;
        _enteredCombat = false;
        _leftCombat = false;
        _strikeApiName = string.Empty;
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
        }
        else
        {
            if (_isOnStrikeMap && _enteredCombat && _leftCombat)
            {
                //trigger update
                LeftStrikeMapWithCombatStartAndEnd?.Invoke(this, _strikeApiName);

                Reset();
            }

        }
    }

    public void Dispose()
    {
        GameService.Gw2Mumble.CurrentMap.MapChanged -= CurrentMap_MapChanged;
        GameService.Gw2Mumble.PlayerCharacter.IsInCombatChanged -= PlayerCharacter_IsInCombatChanged;
    }
}