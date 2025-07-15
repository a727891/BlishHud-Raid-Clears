using Newtonsoft.Json;
using RaidClears.Features.Shared.Enums;
using System;
using System.Collections.Generic;
using System.IO;

namespace RaidClears.Features.Fractals.Services;


[Serializable]
public class InstabilitiesData
{
    [JsonIgnore]
    public static string FILENAME = "instabilities.json";
    [JsonIgnore]
    public static string FILE_URL = $"{Module.STATIC_HOST_URL}/fractal_instabilities.json";

    [JsonProperty("instabilities")]
    public Dictionary<string, int[][]> Instabilities { get; set; } = new Dictionary<string, int[][]>();

    [JsonProperty("instability_names")]
    public string[] Names { get; set; } = new string[] { };


    public List<string> GetInstabsForLevelOnDay(int level, int day)
    {
        List<string> instabs = new List<string>();
        Instabilities.TryGetValue(level.ToString(), out int[][] value);
        if( value?.Length >= day )
        {
            int[] list = value[day];
            foreach(int i in list)
            {
                if(Names.Length >= i)
                {
                    instabs.Add(Names[i]);
                }
            }
        }
        
        return instabs;
    }


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

    public static InstabilitiesData Load()
    {
        if (GetConfigFileInfo() is { Exists: true} configFileInfo)
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

    private static InstabilitiesData LoadFileFromCache(string fileText)
    {
        var loadedCharacterConfiguration = JsonConvert.DeserializeObject<InstabilitiesData>(fileText);

        if (loadedCharacterConfiguration == null)
        {
            loadedCharacterConfiguration = new InstabilitiesData();
        }

        return loadedCharacterConfiguration;
    }

    public static InstabilitiesData DownloadFile()
    {
        try
        {
            using (var webClient = new System.Net.WebClient())
            {
                var json = webClient.DownloadString(FILE_URL);

                InstabilitiesData? instabs = JsonConvert.DeserializeObject<InstabilitiesData>(json);

                if (instabs == null)
                {
                    return new InstabilitiesData();
                }
                instabs.Save();
                return instabs;
            }
        }
        catch (Exception r)
        {
            return new InstabilitiesData();
        }
    }
}