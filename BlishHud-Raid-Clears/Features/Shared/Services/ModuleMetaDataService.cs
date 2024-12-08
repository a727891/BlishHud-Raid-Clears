using Newtonsoft.Json;
using RaidClears.Features.Fractals.Services;
using RaidClears.Features.Raids.Services;
using RaidClears.Features.Shared.Services;
using RaidClears.Features.Strikes.Services;
using System;
using System.Collections.Generic;
using System.IO;

namespace RaidClears.Shared.Services;


[Serializable]
public class ModuleMetaDataService
{
    [JsonIgnore]
    public static string FILENAME = "clears_tracker.json";
    [JsonIgnore]
    public static string FILE_URL = "https://bhm.blishhud.com/Soeed.RaidClears/static/clears_tracker.json";

    [JsonProperty("fractal_instabilities")]
    public string InstabilitiesVersion { get; set; } = null;

    [JsonProperty("fractal_map_data")]
    public string FractalMapVersion { get; set; } = null;

    [JsonProperty("strike_data")]
    public string StrikeDataVersion { get; set; } = null;

    [JsonProperty("raid_data")]
    public string RaidDataVersion { get; set; } = null;

    [JsonProperty("assets")]
    public List<string> Assets { get; set; } = new();


    private static FileInfo GetConfigFileInfo()
    {
        var pluginConfigDirectory = Service.DirectoriesManager.GetFullDirectoryPath(Module.DIRECTORY_PATH);

        return new FileInfo($@"{pluginConfigDirectory}\{FILENAME}");
    }

    public void Save()
    {
        var configFileInfo = GetConfigFileInfo();

        var serializedContents = JsonConvert.SerializeObject(this, Formatting.None);

        using var writer = new StreamWriter(configFileInfo.FullName);
        writer.Write(serializedContents);
        writer.Close();

    }

    public static void CheckVersions()
    {
        ModuleMetaDataService webFile = DownloadFile();
        ModuleMetaDataService localFile = Load();

        if(webFile.InstabilitiesVersion != localFile.InstabilitiesVersion)
        {
            InstabilitiesData.DownloadFile();
            Module.ModuleLogger.Info($"JSON File: Instababilites UPDATED to version {webFile.InstabilitiesVersion}");
        }
        else
        {
            Module.ModuleLogger.Info($"JSON File: Instababilites are current on version {webFile.InstabilitiesVersion}");
        }

        if(webFile.FractalMapVersion!= localFile.FractalMapVersion)
        {
            FractalMapData.DownloadFile();
            Module.ModuleLogger.Info($"JSON File: Fractal Map Data UPDATED to version {webFile.FractalMapVersion}");
        }
        else
        {
            Module.ModuleLogger.Info($"JSON File: Fractal Map Data is current on version {webFile.FractalMapVersion}");
        }

        if (webFile.StrikeDataVersion!= localFile.StrikeDataVersion)
        {
            StrikeData.DownloadFile();
            Module.ModuleLogger.Info($"JSON File: Strike Data UPDATED to version {webFile.StrikeDataVersion}");
        }
        else
        {
            Module.ModuleLogger.Info($"JSON File: Strike Data is current on version {webFile.StrikeDataVersion}");
        }

        if (webFile.RaidDataVersion != localFile.RaidDataVersion)
        {
            RaidData.DownloadFile();
            Module.ModuleLogger.Info($"JSON File: Raid Data UPDATED to version {webFile.RaidDataVersion}");
        }
        else
        {
            Module.ModuleLogger.Info($"JSON File: Raid Data is current on version {webFile.RaidDataVersion}");
        }

        webFile.Save();
        webFile.ValidateAssetCache(webFile.Assets);
    }
    public void ValidateAssetCache(List<string> assets)
    {
        DownloadTextureService _textures = new DownloadTextureService();

        foreach (string asset in assets)
        {
            _textures.ValidateTextureCache(asset);
        }
    }
    public static ModuleMetaDataService Load()
    {
        if (GetConfigFileInfo() is { Exists: true } configFileInfo)
        {
            using var reader = new StreamReader(configFileInfo.FullName);
            var fileText = reader.ReadToEnd();
            reader.Close();

            return LoadFileFromCache(fileText);
        }
        else
        {
            return new ModuleMetaDataService();
        }
    }

    private static ModuleMetaDataService LoadFileFromCache(string fileText)
    {
        var loadedCharacterConfiguration = JsonConvert.DeserializeObject<ModuleMetaDataService>(fileText);

        if (loadedCharacterConfiguration == null)
        {
            loadedCharacterConfiguration = new ModuleMetaDataService();
        }

        return loadedCharacterConfiguration;
    }

    private static ModuleMetaDataService DownloadFile()
    {
        try
        {
            using (var webClient = new System.Net.WebClient())
            {
                var json = webClient.DownloadString(FILE_URL);

                ModuleMetaDataService? metaFile = JsonConvert.DeserializeObject<ModuleMetaDataService>(json);

                if (metaFile == null)
                {
                    return new ModuleMetaDataService();
                }
                return metaFile;
            }
        }
        catch (Exception r)
        {
            return new ModuleMetaDataService();
        }
    }
}