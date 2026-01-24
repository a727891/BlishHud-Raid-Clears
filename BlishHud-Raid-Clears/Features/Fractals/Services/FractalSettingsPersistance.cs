using Blish_HUD.Settings;
using Newtonsoft.Json;
using RaidClears.Features.Fractals.Models;
using RaidClears.Localization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RaidClears.Features.Fractals.Services;

[Serializable]
public class FractalSettingsPersistance
{
    public event EventHandler<bool>? FractalSettingsChanged;

    [JsonIgnore]
    public static string FILENAME = "fractal_settings.json";

    [JsonIgnore]
    protected Dictionary<string, SettingEntry<bool>> VirtualSettingsEnties = new();

    [JsonProperty("version")]
    public string Version { get; set; } = "1.0.0";

    [JsonProperty("challengeMotes")]
    public Dictionary<string, bool> ChallengeMotes { get; set; } = new();

    public void DefineEmpty()
    {
        foreach (var scale in Service.FractalMapData.ChallengeMotes)
        {
            var fractal = Service.FractalMapData.GetFractalForScale(scale);
            if (fractal.ApiLabel != "undefined")
            {
                ChallengeMotes.Add(fractal.ApiLabel, true);
            }
        }
    }

    public SettingEntry<bool> GetChallengeMoteVisible(FractalMap fractal)
    {
        if (VirtualSettingsEnties.ContainsKey(fractal.ApiLabel))
        {
            return VirtualSettingsEnties[fractal.ApiLabel];
        }
        if (!ChallengeMotes.ContainsKey(fractal.ApiLabel))
        {
            ChallengeMotes.Add(fractal.ApiLabel, true);
            Save();
        }

        var setting = new SettingEntry<bool>()
        {
            Value = ChallengeMotes[fractal.ApiLabel],
            GetDescriptionFunc = () => string.Format(Strings.Settings_Fractal_ChallengeMoteVisible_Description, fractal.Label),
            GetDisplayNameFunc = () => $"{fractal.Label}"
        };
        setting.SettingChanged += (_, e) =>
        {
            ChallengeMotes[fractal.ApiLabel] = e.NewValue;
            Save();
        };

        VirtualSettingsEnties.Add(fractal.ApiLabel, setting);
        return setting;
    }

    public SettingEntry<bool>? GetChallengeMoteVisibleByApiId(string apiId)
    {
        foreach (var scale in Service.FractalMapData.ChallengeMotes)
        {
            var fractal = Service.FractalMapData.GetFractalForScale(scale);
            if (fractal.ApiLabel == apiId)
            {
                return GetChallengeMoteVisible(fractal);
            }
        }
        return null;
    }

    public void Save()
    {
        var configFileInfo = GetConfigFileInfo();

        var serializedContents = JsonConvert.SerializeObject(this, Formatting.Indented);

        using var writer = new StreamWriter(configFileInfo.FullName, false, Encoding.UTF8);
        writer.Write(serializedContents);
        writer.Close();
        FractalSettingsChanged?.Invoke(this, true);
    }

    private static FileInfo GetConfigFileInfo()
    {
        var pluginConfigDirectory = Service.DirectoriesManager.GetFullDirectoryPath(Module.DIRECTORY_PATH);

        return new FileInfo($@"{pluginConfigDirectory}\{FILENAME}");
    }

    public static FractalSettingsPersistance Load()
    {
        if (GetConfigFileInfo() is { Exists: true } configFileInfo)
        {
            using var reader = new StreamReader(configFileInfo.FullName, Encoding.UTF8);
            var fileText = reader.ReadToEnd();
            reader.Close();

            return LoadExistingCharacterConfiguration(fileText);
        }
        else
        {
            return CreateNewCharacterConfiguration();
        }
    }

    private static FractalSettingsPersistance LoadExistingCharacterConfiguration(string fileText)
    {
        var loadedCharacterConfiguration = JsonConvert.DeserializeObject<FractalSettingsPersistance>(fileText);

        if (loadedCharacterConfiguration == null)
        {
            loadedCharacterConfiguration = new FractalSettingsPersistance();
        }

        return HandleVersionUpgrade(loadedCharacterConfiguration);
    }

    private static FractalSettingsPersistance HandleVersionUpgrade(FractalSettingsPersistance data)
    {
        if (data.Version == "1.0.0")
        {
            return data;
        }
        else
        {
            return new FractalSettingsPersistance();
        }
    }

    private static FractalSettingsPersistance CreateNewCharacterConfiguration()
    {
        var newCharacterConfiguration = new FractalSettingsPersistance();
        newCharacterConfiguration.DefineEmpty();
        newCharacterConfiguration.Save();
        return newCharacterConfiguration;
    }
}
