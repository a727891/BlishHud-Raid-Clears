using Newtonsoft.Json;
using RaidClears.Features.Shared.Models;
using RaidClears;
using System;
using System.IO;
using System.Text;

namespace RaidClears.Features.Shared.Services;

public static class DailyBountyDataService
{
    private const string FILENAME = "daily_bounties.json";

    private static string FileUrl => $"{Module.STATIC_HOST_URL}{Module.STATIC_HOST_API_VERSION}{FILENAME}";

    private static FileInfo GetConfigFileInfo()
    {
        var pluginConfigDirectory = Service.DirectoriesManager.GetFullDirectoryPath(Module.DIRECTORY_PATH);
        return new FileInfo(Path.Combine(pluginConfigDirectory, FILENAME));
    }

    public static DailyBountyData Load()
    {
        if (GetConfigFileInfo() is { Exists: true } configFileInfo)
        {
            using var reader = new StreamReader(configFileInfo.FullName, Encoding.UTF8);
            var fileText = reader.ReadToEnd();
            reader.Close();
            return LoadFromJson(fileText);
        }
        return DownloadFile();
    }

    private static DailyBountyData LoadFromJson(string fileText)
    {
        var data = JsonConvert.DeserializeObject<DailyBountyData>(fileText);
        return data ?? new DailyBountyData();
    }

    public static DailyBountyData DownloadFile()
    {
        try
        {
            using var webClient = new System.Net.WebClient();
            webClient.Encoding = Encoding.UTF8;
            var json = webClient.DownloadString(FileUrl);
            var data = JsonConvert.DeserializeObject<DailyBountyData>(json);
            if (data == null)
                return new DailyBountyData();
            data.Save();
            return data;
        }
        catch (Exception ex)
        {
            Module.ModuleLogger.Warn(ex, "Could not download daily bounties data file");
            return new DailyBountyData();
        }
    }

    public static void Save(this DailyBountyData data)
    {
        var configFileInfo = GetConfigFileInfo();
        var serialized = JsonConvert.SerializeObject(data, Formatting.None);
        using var writer = new StreamWriter(configFileInfo.FullName, false, Encoding.UTF8);
        writer.Write(serialized);
        writer.Close();
    }

}
