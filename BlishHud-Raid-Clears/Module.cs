using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;

using Gw2Sharp.WebApi.V2.Models;

using RaidClears.Settings;
using RaidClears.Raids.Controls;
using RaidClears.Raids.Model;
using RaidClears.Raids.Services;

using RaidClears.Dungeons.Controls;
using RaidClears.Dungeons.Model;
using RaidClears.Dungeons.Services;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace RaidClears
{
    [Export(typeof(Blish_HUD.Modules.Module))]
    public class Module : Blish_HUD.Modules.Module
    {
        private static readonly Logger Logger = Logger.GetLogger<Module>();
        internal SettingsManager SettingsManager => ModuleParameters.SettingsManager;
        internal ContentsManager ContentsManager => ModuleParameters.ContentsManager;
        internal DirectoriesManager DirectoriesManager => ModuleParameters.DirectoriesManager;
        internal Gw2ApiManager Gw2ApiManager => ModuleParameters.Gw2ApiManager;

        [ImportingConstructor]
        public Module([Import("ModuleParameters")] ModuleParameters moduleParameters) : base(moduleParameters)
        {
        }
        #region Settings
        protected override void DefineSettings(SettingCollection settings)
        {
            _settingService = new SettingService(settings);
        }

        public override IView GetSettingsView()
        {
            return new ModuleSettingsView(_settingService, this, _textureService);
        }
        #endregion


        #region Setup/Teardown
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        protected override async Task LoadAsync()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            _textureService = new TextureService(ContentsManager);
    
            _raidsPanel = new RaidsPanel(Logger, _settingService, Wing.GetWingMetaData());
            _dungeonsPanel = new DungeonsPanel(Logger, _settingService, Dungeons.Model.Dungeon.GetDungeonMetaData());

            SetTimeoutValueInMinutes( (int)_settingService.RaidPanelApiPollingPeriod.Value);

            _settingService.RaidPanelIsVisibleKeyBind.Value.Activated += OnRaidPanelDisplayKeybindActivated;
            _settingService.DungeonPanelIsVisibleKeyBind.Value.Activated += OnDungeonPanelDisplayKeybindActivated;

            _settingService.RaidPanelApiPollingPeriod.SettingChanged += (s,e) => SetTimeoutValueInMinutes((int)e.NewValue) ;

            _cornerIconService = new CornerIconService(
                _settingService.ShowRaidsCornerIconSetting,
                "Click to show/hide the Raid Clears window.\nIcon can be hidden by module settings.",
                (s, e) => _settingService.ToggleRaidPanelVisibility(),
                _textureService);

            _dungeonCornerIconService = new DungeonCornerIconService(
               _settingService.ShowDungeonCornerIconSetting,
               "Click to show/hide the Dungeon Clears window.\nIcon can be hidden by module settings.",
               (s, e) => _settingService.ToggleDungeonPanelVisibility(),
               _textureService);

            Gw2ApiManager.SubtokenUpdated += Gw2ApiManager_SubtokenUpdated;

            //Check if module was reloaded
            if (Gw2ApiManager.HasPermissions(GetCurrentClearsService.NECESSARY_API_TOKEN_PERMISSIONS))
            {
                _lastApiCheck = _API_QUERY_INTERVAL;
            }

        }

        protected override void Unload()
        {
            _settingService.RaidPanelIsVisibleKeyBind.Value.Activated -= OnRaidPanelDisplayKeybindActivated;
            _settingService.DungeonPanelIsVisibleKeyBind.Value.Activated -= OnDungeonPanelDisplayKeybindActivated;
            Gw2ApiManager.SubtokenUpdated -= Gw2ApiManager_SubtokenUpdated;

            _raidsPanel?.Dispose();
            _dungeonsPanel?.Dispose();
            _textureService?.Dispose();
            _cornerIconService?.Dispose();
        }



        #endregion

        protected override void Update(GameTime gameTime)
        {
            _raidsPanel?.ShowOrHide();
            _dungeonsPanel?.ShowOrHide();

            ApiPollTimeout(gameTime.ElapsedGameTime.TotalMilliseconds);

            
        }

        private void SetTimeoutValueInMinutes(int minutes)
        {
            _API_QUERY_INTERVAL = ((minutes * MINUTE_MS) + BUFFER_MS);
        }

        public int GetTimeoutSecondsRemaining()
        {
            if(_lastApiCheck == -1)
            {
                return -1;
            }
            return (int) ((_API_QUERY_INTERVAL - _lastApiCheck) / 1000);
        }

        private void ApiPollTimeout(double elapsedTime)
        {
            if (_lastApiCheck >= 0)
            {
                _lastApiCheck += elapsedTime;

                if (_lastApiCheck >= _API_QUERY_INTERVAL)
                {
                    _lastApiCheck = 0;
                    Task.Run(async () =>
                    {
                        var (weeklyClears, apiAccessFailed) = await GetCurrentClearsService.GetClearsFromApi(Gw2ApiManager, Logger);
                        if (apiAccessFailed)
                        {
                            return;
                        }

                        _raidsPanel.UpdateClearedStatus(weeklyClears);
                    });
                }
            }
        }

        private void Gw2ApiManager_SubtokenUpdated(object sender, ValueEventArgs<IEnumerable<TokenPermission>> e)
        {
            // _settingToggleKey check interval so that we check immediately now that we have a new token.
            _lastApiCheck = _API_QUERY_INTERVAL;
        }


        private void OnRaidPanelDisplayKeybindActivated(object sender, EventArgs e)
        {
            _settingService.ToggleRaidPanelVisibility();
        }

        private void OnDungeonPanelDisplayKeybindActivated(object sender, EventArgs e)
        {
            _settingService.ToggleDungeonPanelVisibility();
        }

        private const int BUFFER_MS = 50;
        private const int MINUTE_MS = 60000; 

        private double _lastApiCheck = -1;
        private double _API_QUERY_INTERVAL = 300100; // 300 seconds + 100ms

        private TextureService _textureService;
        private SettingService _settingService;
        private CornerIconService _cornerIconService;
        private DungeonCornerIconService _dungeonCornerIconService;
        private RaidsPanel _raidsPanel;
        private DungeonsPanel _dungeonsPanel;
    }
}