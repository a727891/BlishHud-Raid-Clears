using Blish_HUD.Settings;
using Newtonsoft.Json;
using RaidClears.Features.Raids.Models;
using RaidClears.Features.Shared.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace RaidClears.Features.Raids.Services;


[Serializable]
public abstract class Labelable
{
    protected bool _isRaid = false;
    protected bool _isStrike = false;
    protected bool _isFractal = false;

    [JsonProperty("encounterLabels")]
    public Dictionary<string, string> EncounterLabels { get; set; } = new();


    public abstract void SetEncounterLabel(string encounterApiId, string label);
    public string GetEncounterLabel(string encounterApiId)
    {
        if(encounterApiId== "voice_and_claw")
        {
            Debug.WriteLine("test");
        }
        if(EncounterLabels.TryGetValue(encounterApiId,out var value)){
            return value;
        }
        if (_isRaid)
        {
            return Service.RaidData.GetRaidEncounterByApiId(encounterApiId).Abbriviation;
        }
        else if(_isStrike)
        {
            return Service.StrikeData.GetStrikeMissionById(encounterApiId).Abbriviation;
        }else if (_isFractal)
        {
            return Service.FractalMapData.GetFractalByApiName(encounterApiId).ShortLabel;
        }
        else
        {
            return "undefined";
        }
    }
    public string GetEncounterLabel(RaidEncounter enc)
    {
        if (EncounterLabels.TryGetValue(enc.ApiId, out var value)){
            return value;
        }
        return enc.Abbriviation;
    }
    public string GetEncounterLabel(EncounterInterface enc)
    {
        if (EncounterLabels.TryGetValue(enc.Id, out var value))
        {
            return value;
        }
        return enc.Abbriviation;
    }

    public abstract void Save();
}