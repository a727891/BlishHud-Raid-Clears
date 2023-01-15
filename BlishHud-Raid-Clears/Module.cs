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
using Microsoft.Xna.Framework;
using Color = Microsoft.Xna.Framework.Color;
using RaidClears.Settings.Controls;
using RaidClears.Settings;
using RaidClears.Features.Raids;
using RaidClears.Fearures.Shared.Services;
using RaidClears.Raids.Services;

namespace RaidClears
{
    [Export(typeof(Blish_HUD.Modules.Module))]
    public class Module : Blish_HUD.Modules.Module
    {
        internal static readonly Logger Logger = Logger.GetLogger<Module>();
        internal SettingsManager SettingsManager => ModuleParameters.SettingsManager;
        internal ContentsManager ContentsManager => ModuleParameters.ContentsManager;
        internal DirectoriesManager DirectoriesManager => ModuleParameters.DirectoriesManager;
        internal Gw2ApiManager Gw2ApiManager => ModuleParameters.Gw2ApiManager;

        internal static Module ModuleInstance;

        internal SettingsPanel SettingsWindow { get; private set; }

        public SettingService SettingsService { get; private set; }

        public RaidPanel RaidsPanel { get; private set; }

        public TextureService TexturesService { get; private set; }

        public ApiPollService ApiPollingService { get; private set; }



        [ImportingConstructor]
        public Module([Import("ModuleParameters")] ModuleParameters moduleParameters) : base(moduleParameters)
        {
            ModuleInstance = this;
        }

        #region Settings
        protected override void DefineSettings(SettingCollection settings)
        {
            SettingsService = new SettingService(settings);
        }

        public override IView GetSettingsView()
        { 
            return new Settings.Views.ModuleSettingsView();
        }

        public void OpenFullSettingsPanel()
        {

        }
        #endregion


        #region Setup/Teardown
        #pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        protected override async Task LoadAsync()
        #pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            ApiPollingService = new ApiPollService(SettingsService.ApiPollingPeriod);
            TexturesService = new TextureService(ContentsManager);

            SettingsWindow = SettingPanelFactory.Create();
            RaidsPanel = RaidPanelFactory.Create();

            Gw2ApiManager.SubtokenUpdated += Gw2ApiManager_SubtokenUpdated;
        }

        protected override void Unload()
        {
            Gw2ApiManager.SubtokenUpdated -= Gw2ApiManager_SubtokenUpdated;

            RaidsPanel?.Dispose();
            SettingsWindow?.Dispose();
            TexturesService?.Dispose();
            ApiPollingService?.Dispose();

        }



        #endregion

        protected override void Update(GameTime gameTime)
        {
            ApiPollingService?.Update(gameTime);
            RaidsPanel?.Update();

            
        }

        private void Gw2ApiManager_SubtokenUpdated(object sender, ValueEventArgs<IEnumerable<TokenPermission>> e)
        {
            // _settingToggleKey check interval so that we check immediately now that we have a new token.
            ApiPollingService.Invoke();
        }


    }
}