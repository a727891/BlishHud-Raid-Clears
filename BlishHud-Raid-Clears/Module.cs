﻿using System.Collections.Generic;
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
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework.Input;
using RaidClears.Localization;
using Blish_HUD.Modules.Managers;
using RaidClears.Features.Shared.Enums;
using System;
using Newtonsoft.Json;
using RaidClears.Features.Strikes.Models;

namespace RaidClears;


[Export(typeof(Blish_HUD.Modules.Module))]
public class Module : Blish_HUD.Modules.Module
{
    public static string STRIKE_DIRECTORY = "strikes";
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

    protected override Task LoadAsync()
    {
        Service.ApiPollingService = new ApiPollService(Service.Settings.ApiPollingPeriod);
        Service.TexturesService = new TextureService(Service.ContentsManager);

        Service.SettingsWindow = new SettingsPanel();
        Service.RaidWindow = new();
        Service.StrikesWindow = new();
        Service.DungeonWindow = new();


        Service.CornerIconService = new CornerIconService(
            Service.Settings.GlobalCornerIconEnabled,
            Strings.CornerIcon_Raid,
            Service.TexturesService!.CornerIconTexture,
            Service.TexturesService!.CornerIconHoverTexture,
            new List<CornerIconToggleMenuItem>()
            {
                new CornerIconToggleMenuItem(Service.SettingsWindow, "Open Settings"),
                new CornerIconToggleMenuItem(Service.Settings.RaidSettings.Generic.Visible, "Raids"),
                new CornerIconToggleMenuItem(Service.Settings.StrikeSettings.Generic.Visible, "Strikes"),
                new CornerIconToggleMenuItem(Service.Settings.DungeonSettings.Generic.Visible, "Dungeons"),

            }
        );
        Service.CornerIconService.IconLeftClicked += (_, _) =>
        {
            Service.Settings.RaidSettings.Generic.ToggleVisible();
            Service.Settings.DungeonSettings.Generic.ToggleVisible();
            Service.Settings.StrikeSettings.Generic.ToggleVisible();

        };

        Service.Gw2ApiManager.SubtokenUpdated += Gw2ApiManager_SubtokenUpdated;

/*        var eventsDirectory = Service.DirectoriesManager.GetFullDirectoryPath(STRIKE_DIRECTORY);
        var data = new Dictionary<Encounters.StrikeMission, DateTime>
        {
            { Encounters.StrikeMission.ColdWar, DateTime.Now },
            { Encounters.StrikeMission.Fraenir, DateTime.Now },
            { Encounters.StrikeMission.ShiverpeaksPass, DateTime.Now },
            { Encounters.StrikeMission.VoiceAndClaw, DateTime.Now },
            { Encounters.StrikeMission.Whisper, DateTime.Now },
            { Encounters.StrikeMission.Boneskinner, DateTime.Now },
            { Encounters.StrikeMission.AetherbladeHideout, DateTime.Now },
            { Encounters.StrikeMission.Junkyard, DateTime.Now },
            { Encounters.StrikeMission.Overlook, DateTime.Now },
            { Encounters.StrikeMission.HarvestTemple, DateTime.Now },
            { Encounters.StrikeMission.OldLionsCourt, DateTime.Now }
        };
        var model = new Features.Strikes.Models.StrikePersistance();
        model.AccountClears.Add("soeed.4160", data);

        ModuleLogger.Info("test message");
        var contents = model.Save();
        ModuleLogger.Info(contents);

        var newPersistance = JsonConvert.DeserializeObject<StrikePersistance>(contents);
        ModuleLogger.Info("deserializedone");*/
        return Task.CompletedTask;

        /*GameService.Overlay.UserLocaleChanged += (s, e) =>
        {
           //todo: refresh views
        };*/

     
    }

    protected override void Unload()
    {
        Service.Gw2ApiManager.SubtokenUpdated -= Gw2ApiManager_SubtokenUpdated;
        
        Service.ContentsManager?.Dispose();
        Service.TexturesService?.Dispose();
        Service.ApiPollingService?.Dispose();

        Service.StrikesWindow?.Dispose();
        Service.DungeonWindow?.Dispose();
        Service.RaidWindow?.Dispose();
        Service.SettingsWindow?.Dispose();
        Service.CornerIconService?.Dispose();
    }

    protected override void Update(GameTime gameTime)
    {
        Service.ApiPollingService?.Update(gameTime);
        Service.RaidWindow?.Update();
        Service.DungeonWindow?.Update();
        Service.StrikesWindow?.Update();
    }

    private void Gw2ApiManager_SubtokenUpdated(object sender, ValueEventArgs<IEnumerable<TokenPermission>> e) => Service.ApiPollingService?.Invoke();
}