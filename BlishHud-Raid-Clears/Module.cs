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
using Blish_HUD.Modules.Managers;
using Blish_HUD.GameIntegration;
namespace RaidClears;


[Export(typeof(Blish_HUD.Modules.Module))]
public class Module : Blish_HUD.Modules.Module
{
    public static string DIRECTORY_PATH = "clearsTracker"; //Defined folder in manifest.json

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
        this.TEMP_FIX_SetTacOAsActive();
    }

    protected override Task LoadAsync()
    {

        Service.StrikePersistance = StrikePersistance.Load();
        Service.FractalPersistance = FractalPersistance.Load();

        Service.ApiPollingService = new ApiPollService(Service.Settings.ApiPollingPeriod);
        Service.Textures = new TextureService(Service.ContentsManager);

        Service.ResetWatcher = new ResetsWatcherService();
        Service.MapWatcher = new MapWatcherService();
        Service.FractalMapWatcher = new FractalMapWatcherService();

        Service.SettingsWindow = new SettingsPanel();
        Service.RaidWindow = new Features.Raids.RaidPanel();
        Service.StrikesWindow = new Features.Strikes.StrikesPanel();
        Service.FractalWindow = new Features.Fractals.FractalsPanel();
        Service.DungeonWindow = new Features.Dungeons.DungeonPanel();


        var refreshApiContextMenu = new ContextMenuStripItem(Strings.Settings_RefreshNow);
        refreshApiContextMenu.Click += (s, e) => Service.ApiPollingService?.Invoke();

        Service.CornerIcon = new CornerIconService(
            Service.Settings.GlobalCornerIconEnabled,
            Strings.Module_Title,
            Service.Textures!.CornerIconTexture,
            Service.Textures!.CornerIconHoverTexture,
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

        Service.CornerIcon.IconLeftClicked += CornerIcon_IconLeftClicked;

        Service.Gw2ApiManager.SubtokenUpdated += Gw2ApiManager_SubtokenUpdated;


        return Task.CompletedTask;

        /*GameService.Overlay.UserLocaleChanged += (s, e) =>
        {
           //todo: refresh views
        };*/


    }

    private void TEMP_FIX_SetTacOAsActive()
    {
        // SOTO Fix
        if (System.DateTime.UtcNow.Date >= new System.DateTime(2023, 8, 22, 0, 0, 0, System.DateTimeKind.Utc) && Program.OverlayVersion < new SemVer.Version(1, 1, 0))
        {
            try
            {
                var tacoActive = typeof(TacOIntegration).GetProperty(nameof(TacOIntegration.TacOIsRunning)).GetSetMethod(true);
                tacoActive?.Invoke(GameService.GameIntegration.TacO, new object[] { true });
            }
            catch { /* NOOP */ }
        }
    }

    protected override void Unload()
    {
        Service.Gw2ApiManager.SubtokenUpdated -= Gw2ApiManager_SubtokenUpdated;
        Service.CornerIcon.IconLeftClicked -= CornerIcon_IconLeftClicked;

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
        Task.Run(async () =>
        {
            Service.CurrentAccountName = await AccountNameService.UpdateAccountName();
            Service.MapWatcher.DispatchCurrentStrikeClears();
            Service.FractalMapWatcher.DispatchCurrentClears();
        });
        Service.ApiPollingService?.Invoke();
    } 
}