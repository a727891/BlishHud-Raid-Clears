using Newtonsoft.Json;
using RaidClears.Features.Shared.Enums;
using System;
using System.Collections.Generic;
using System.IO;

namespace RaidClears.Features.Fractals.Services;


[Serializable]
public class FractalPersistance
{
    [JsonIgnore]
    public static string FILENAME = "fractal_clears.json";

    [JsonProperty("version")]
    public string Version { get; set; } = "2.2.0";

    [JsonProperty("accountClears")]
    public Dictionary<string, Dictionary<Encounters.Fractal, DateTime>> AccountClears { get; set; } = new();

    public Dictionary<Encounters.Fractal, DateTime> GetEmpty() => new Dictionary<Encounters.Fractal, DateTime>
                {
                    //{ Encounters.Fractal.MistlockObservatory, new() },
                    { Encounters.Fractal.AetherbladeFractal, new() },
                    { Encounters.Fractal.AquaticRuinsFractal, new() },
                    { Encounters.Fractal.CaptainMaiTrinBossFractal, new() },
                    { Encounters.Fractal.ChaosFractal, new() },
                    { Encounters.Fractal.CliffsideFractal, new() },
                    { Encounters.Fractal.DeepstoneFractal, new() },
                    { Encounters.Fractal.MoltenBossFractal, new() },
                    { Encounters.Fractal.MoltenFurnaceFractal, new() },
                    { Encounters.Fractal.NightmareFractal, new() },
                    { Encounters.Fractal.ShatteredObservatoryFractal, new() },
                    { Encounters.Fractal.SirensReefFractal, new() },
                    { Encounters.Fractal.SilentSurfFractal, new() },
                    { Encounters.Fractal.SnowblindFractal, new() },
                    { Encounters.Fractal.SunquaPeakFractal, new() },
                    { Encounters.Fractal.SolidOceanFractal, new() },
                    { Encounters.Fractal.SwamplandFractal, new() },
                    { Encounters.Fractal.ThaumanovaReactorFractal, new() },
                    { Encounters.Fractal.TwilightOasisFractal, new() },
                    { Encounters.Fractal.UncategorizedFractal, new() },
                    { Encounters.Fractal.UndergroundFacilityFractal, new() },
                    { Encounters.Fractal.UrbanBattlegroundFractal, new() },
                    { Encounters.Fractal.VolcanicFractal, new() }
                };

    public void SaveClear(string account, Encounters.Fractal mission)
    {
        Dictionary<Encounters.Fractal, DateTime> clears;
        if (!AccountClears.TryGetValue(account, out clears))
        {
            clears = GetEmpty();
            AccountClears.Add(account, clears);
            // the key isn't in the dictionary.
        }

        clears[mission] = DateTime.UtcNow;
        AccountClears[account]= clears;
        Save();

    }
    public void RemoveClear(string account, Encounters.Fractal mission)
    {
        Dictionary<Encounters.Fractal, DateTime> clears;
        if (!AccountClears.TryGetValue(account, out clears))
        {
            clears = GetEmpty();
            AccountClears.Add(account, clears);
            // the key isn't in the dictionary.
        }

        clears[mission] = new DateTime();
        AccountClears[account] = clears;
        Save();
    }

    public void Save()
    {
        //PluginLog.Verbose($"{DateTime.Now} - {CharacterData.Name} Saved");

        var configFileInfo = GetConfigFileInfo();

        var serializedContents = JsonConvert.SerializeObject(this, Formatting.Indented);

        using var writer = new StreamWriter(configFileInfo.FullName);
        writer.Write(serializedContents);
        writer.Close();

        //PluginLog.Warning("Tried to save a config with invalid LocalContentID, aborting save.");

    }

    private static FileInfo GetConfigFileInfo()
    {
        var pluginConfigDirectory = Service.DirectoriesManager.GetFullDirectoryPath(Module.DIRECTORY_PATH);

        return new FileInfo($@"{pluginConfigDirectory}\{FILENAME}");
    }

    public static FractalPersistance Load()
    {
        if (GetConfigFileInfo() is { Exists: true } configFileInfo)
        {
            using var reader = new StreamReader(configFileInfo.FullName);
            var fileText = reader.ReadToEnd();
            reader.Close();

            return LoadExistingCharacterConfiguration(fileText);
        }
        else
        {
            return CreateNewCharacterConfiguration();
        }
    }

    private static FractalPersistance LoadExistingCharacterConfiguration(string fileText)
    {
        var loadedCharacterConfiguration = JsonConvert.DeserializeObject<FractalPersistance>(fileText);

        if (loadedCharacterConfiguration == null)
        {
            loadedCharacterConfiguration = new FractalPersistance();
        }

        return loadedCharacterConfiguration;
    }

    private static FractalPersistance CreateNewCharacterConfiguration()
    {
        var newCharacterConfiguration = new FractalPersistance();
        newCharacterConfiguration.AccountClears.Add("default", newCharacterConfiguration.GetEmpty());
        newCharacterConfiguration.Save();
        return newCharacterConfiguration;
    }
}