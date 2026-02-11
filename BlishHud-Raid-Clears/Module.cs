using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Modules;
using Blish_HUD.Settings;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using RaidClears.Settings.Services;
using RaidClears.Features.Shared.Services;
using RaidClears.Settings.Controls;
using RaidClears.Localization;
using RaidClears.Features.Strikes.Services;
using Blish_HUD.Controls;
using RaidClears.Features.Shared.Models;
using RaidClears.Features.Fractals.Services;
using RaidClears.Shared.Services;
using RaidClears.Features.Raids.Services;
using System;

namespace RaidClears;


[Export(typeof(Blish_HUD.Modules.Module))]
public class Module : Blish_HUD.Modules.Module
{
    public static string DIRECTORY_PATH = "clearsTracker"; //Defined folder in manifest.json
#if DEBUG
    public static string STATIC_HOST_URL = "http://localhost:3000/";
    public static string STATIC_HOST_API_VERSION = "v2/";
#else
    public static string STATIC_HOST_URL = "https://bhm.blishhud.com/Soeed.RaidClears/static/";
    public static string STATIC_HOST_API_VERSION = "v2/";
#endif


    internal static readonly Logger ModuleLogger = Logger.GetLogger<Module>();

    [ImportingConstructor]
    public Module([Import("ModuleParameters")] ModuleParameters moduleParameters) : base(moduleParameters)
    {
        Service.ModuleInstance = this;
        Service.ContentsManager = moduleParameters.ContentsManager;
        Service.Gw2ApiManager = moduleParameters.Gw2ApiManager;
        Service.DirectoriesManager = moduleParameters.DirectoriesManager;
    }

    protected override void DefineSettings(SettingCollection settings) => Service.Settings = new SettingService(settings);

    public override IView GetSettingsView() => new Settings.Views.ModuleMainSettingsView();

    protected override void Initialize()
    {
    }

    protected override async Task LoadAsync()
    {
        Service.Textures = new TextureService(Service.ContentsManager);
        var metadata = ModuleMetaDataService.CheckVersions();

        Service.RaidData = RaidData.Load();
        Service.MentorAchievementProgress = new MentorAchievementProgressService(Service.RaidData);
        Service.MentorAchievementProgress.LoadCache();
        Service.StrikeData = StrikeData.Load();
        Service.DailyBountyData = DailyBountyDataService.Load();
        Service.RaidSettings = RaidSettingsPersistance.Load();
        Service.StrikeSettings = StrikeSettingsPersistance.Load();
        Service.FractalMapData = FractalMapData.Load();
        Service.InstabilitiesData = InstabilitiesData.Load();
        Service.StrikePersistance = StrikePersistance.Load();
        Service.FractalPersistance = FractalPersistance.Load();
        Service.FractalSettings = FractalSettingsPersistance.Load();

        Service.ApiPollingService = new ApiPollService(Service.Settings.ApiPollingPeriod);

        Service.ResetWatcher = new ResetsWatcherService();
        Service.MapWatcher = new MapWatcherService();
        Service.FractalMapWatcher = new FractalMapWatcherService();

        Service.SettingsWindow = new SettingsPanel();
        Service.RaidWindow = new Features.Raids.RaidPanel();
        Service.StrikesWindow = new Features.Strikes.StrikesPanel();
        Service.FractalWindow = new Features.Fractals.FractalsPanel();
        Service.DungeonWindow = new Features.Dungeons.DungeonPanel();

        try
        {
            var refreshApiContextMenu = new ContextMenuStripItem(Strings.Settings_RefreshNow);
            refreshApiContextMenu.Click += (s, e) => Service.ApiPollingService?.Invoke();

            Service.CornerIcon = new CornerIconService(
                Service.Settings.GlobalCornerIconEnabled,
                Strings.Module_Title,
                Service.Textures!.CornerIconTexture,
                Service.Textures!.CornerIconHoverTexture,
                Service.Textures!.CornerIconNotificationTexture,
                Service.Textures!.CornerIconNotificationHoverTexture,
                new List<ContextMenuStripItem>()
                {
                new CornerIconToggleMenuItem(Service.SettingsWindow, Strings.ModuleSettings_OpenSettings),
                new ContextMenuStripItemSeparator(),
                new CornerIconToggleMenuItem(Service.Settings.RaidSettings.Generic.Visible, Strings.SettingsPanel_Tab_Raids),
                new CornerIconToggleMenuItem(Service.Settings.StrikeSettings.Generic.Visible, Strings.SettingsPanel_Tab_Strikes),
                new CornerIconToggleMenuItem(Service.Settings.FractalSettings.Generic.Visible, "Fractals"),
                new CornerIconToggleMenuItem(Service.Settings.DungeonSettings.Generic.Visible, Strings.SettingsPanel_Tab_Dunegons),
                new ContextMenuStripItemSeparator(),
                refreshApiContextMenu

                }
            );
            if (Service.CornerIcon != null)
            {
                Service.CornerIcon.IconLeftClicked += CornerIcon_IconLeftClicked;
                // Check for MOTD after corner icon is created
                CheckMotd(metadata);
            }

            Service.Gw2ApiManager.SubtokenUpdated += Gw2ApiManager_SubtokenUpdated;
            Service.MentorAchievementProgress.ProgressUpdated += MentorAchievementProgress_ProgressUpdated;
            DispatchClears();
        }
        catch (System.Exception e)
        {
            ModuleLogger.Error(e, "Error loading module");
        }


        /*GameService.Overlay.UserLocaleChanged += (s, e) =>
        {
           //todo: refresh views
        };*/


    }

    private void DispatchClears()
    {
        Task.Run(async () =>
        {
            Service.CurrentAccountName = await AccountNameService.UpdateAccountName();
            Service.MapWatcher.DispatchCurrentStrikeClears();
            Service.FractalMapWatcher.DispatchCurrentClears();
            Service.CornerIcon?.UpdateAccountName(Service.CurrentAccountName);
            if (Service.Settings.RaidSettings.RaidPanelMentorProgress.Value)
                await Service.MentorAchievementProgress.RefreshFromApiAsync();
        });
    }

    protected override void Unload()
    {
        Service.MentorAchievementProgress.ProgressUpdated -= MentorAchievementProgress_ProgressUpdated;
        Service.Gw2ApiManager.SubtokenUpdated -= Gw2ApiManager_SubtokenUpdated;
        if (Service.CornerIcon != null)
        {
            Service.CornerIcon.IconLeftClicked -= CornerIcon_IconLeftClicked;
        }

        Service.ContentsManager?.Dispose();
        Service.Textures?.Dispose();
        Service.ApiPollingService?.Dispose();

        Service.FractalWindow?.Dispose();
        Service.StrikesWindow?.Dispose();
        Service.DungeonWindow?.Dispose();
        Service.RaidWindow?.Dispose();
        Service.SettingsWindow?.Dispose();
        Service.CornerIcon?.Dispose();

        Service.MapWatcher?.Dispose();
        Service.FractalMapWatcher.Dispose();
        Service.ResetWatcher?.Dispose();
    }

    protected override void Update(GameTime gameTime)
    {
        Service.ApiPollingService?.Update(gameTime);
        Service.RaidWindow?.Update();
        Service.DungeonWindow?.Update();
        Service.StrikesWindow?.Update();
        Service.FractalWindow?.Update();
        Service.ResetWatcher?.Update(gameTime);
    }
    private void CornerIcon_IconLeftClicked(object sender, bool e)
    {
        Service.Settings.RaidSettings.Generic.ToggleVisible();
        Service.Settings.DungeonSettings.Generic.ToggleVisible();
        Service.Settings.StrikeSettings.Generic.ToggleVisible();
        Service.Settings.FractalSettings.Generic.ToggleVisible();
    }

    private void Gw2ApiManager_SubtokenUpdated(object sender, ValueEventArgs<IEnumerable<TokenPermission>> e)
    {
        DispatchClears();
        Service.ApiPollingService?.Invoke();
    }

    private void MentorAchievementProgress_ProgressUpdated(object? sender, MentorProgressUpdatedEventArgs e)
    {
        if (Service.Settings?.RaidSettings?.RaidPanelMentorProgress?.Value != true
            || Service.Settings?.RaidSettings?.RaidPanelMentorProgressPopup?.Value != true)
            return;
        if (e.Changes.Count == 0)
            return;
        var raidData = Service.RaidData;
        var progress = Service.MentorAchievementProgress?.Progress;
        if (raidData == null || progress == null)
            return;
        var changes = new List<MentorProgressChange>(e.Changes);
        const int popupWidth = 300;
        const int popupHeight = 72;
        const int gap = 8;
        const int margin = 20;
        GameService.Graphics.QueueMainThreadRender(_ =>
        {
            var screenSize = GameService.Graphics.SpriteScreen.Size;
            int x = screenSize.X - popupWidth - margin;
            int y = margin + 60;
            foreach (var change in changes)
            {
                if (change.Delta <= 0)
                    continue;
                var encounter = raidData.GetEncounterByMentorAchievementId(change.AchievementId);
                string bossName = encounter?.Name ?? $"Achievement {change.AchievementId}";
                int max = progress.TryGetValue(change.AchievementId, out var entry) ? entry.Max : change.NewCurrent;
                int iconAssetId = (encounter != null && encounter.AssetId > 0) ? encounter.AssetId : raidData.MentorAssetId;
                var popup = new Features.Raids.MentorProgressPopupPanel(bossName, change.NewCurrent, max, change.Delta, iconAssetId)
                {
                    Parent = GameService.Graphics.SpriteScreen,
                    Location = new Point(x, y)
                };
                y += popupHeight + gap;
            }
        });
    }

    private void CheckMotd(ModuleMetaDataService metadata)
    {
        try
        {
            // Check if there's a new MOTD
            if (!string.IsNullOrEmpty(metadata.Motd) && !string.IsNullOrEmpty(metadata.MotdId))
            {
                var lastShownId = Service.Settings.LastShownMotdId.Value;
                
                // If this is a new message, show notification
       
                if (Service.CornerIcon != null)
                {
                    Service.CornerIcon.SetCurrentMotdId(metadata.MotdId);
                    Service.CornerIcon.SetNotificationState(lastShownId != metadata.MotdId, metadata.Motd);
                }
                
            }
        }
        catch (Exception ex)
        {
            ModuleLogger.Warn(ex, "Error checking MOTD");
        }
    }
}