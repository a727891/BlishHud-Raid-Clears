using Newtonsoft.Json;
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

    [JsonProperty("fracal_instabilities")]
    public string Instabilities { get; set; } = null;

    [JsonProperty("fractal_map_data")]
    public string MapData { get; set; } = null;



    private static FileInfo GetConfigFileInfo()
    {
        var pluginConfigDirectory = Service.DirectoriesManager.GetFullDirectoryPath(Module.DIRECTORY_PATH);

        return new FileInfo($@"{pluginConfigDirectory}\{FILENAME}");
    }

    public void Save()
    {
        //PluginLog.Verbose($"{DateTime.Now} - {CharacterData.Name} Saved");

        var configFileInfo = GetConfigFileInfo();

        var serializedContents = JsonConvert.SerializeObject(this, Formatting.None);

        using var writer = new StreamWriter(configFileInfo.FullName);
        writer.Write(serializedContents);
        writer.Close();

        //PluginLog.Warning("Tried to save a config with invalid LocalContentID, aborting save.");

    }

    public static ModuleMetaDataService Load()
    {
        if (GetConfigFileInfo() is { Exists: true, LastWriteTime: var lastWriteTime } configFileInfo && (DateTime.Now - lastWriteTime).TotalDays < 1)
        {
            using var reader = new StreamReader(configFileInfo.FullName);
            var fileText = reader.ReadToEnd();
            reader.Close();

            return LoadFileFromCache(fileText);
        }
        else
        {
            return DownloadFile();
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

                ModuleMetaDataService? instabs = JsonConvert.DeserializeObject<ModuleMetaDataService>(json);

                if (instabs == null)
                {
                    return new ModuleMetaDataService();
                }
                instabs.Save();
                return instabs;
            }
        }
        catch (Exception r)
        {
            return new ModuleMetaDataService();
        }
    }
}