using Blish_HUD.Modules.Managers;
using RaidClears.Features.Shared.Services;
using RaidClears.Settings.Controls;
using RaidClears.Settings.Services;

namespace RaidClears;

public static class Service
{
    public static Module ModuleInstance { get; set; } = null!;
    public static SettingService Settings { get; set; } = null!;
    public static ContentsManager ContentsManager { get; set; } = null!;
    public static Gw2ApiManager Gw2ApiManager { get; set; } = null!;
    public static TextureService? TexturesService { get; set; }
    public static ApiPollService? ApiPollingService { get; set; }
    public static SettingsPanel SettingsWindow { get; set; } = null!;
}