using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using RaidClears.Settings.Controls;
using RaidClears.Features.Raids;
using RaidClears.Features.Dungeons;
using RaidClears.Features.Strikes;
using RaidClears.Settings.Services;
using RaidClears.Features.Shared.Services;

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
        public DungeonPanel DungeonsPanel { get; private set; }
        public StrikesPanel StrikesPanel { get; private set; }

        public TextureService TexturesService { get; private set; }

        public ApiPollService ApiPollingService { get; private set; }


        [ImportingConstructor]
        public Module([Import("ModuleParameters")] ModuleParameters moduleParameters) : base(moduleParameters)
        {
            ModuleInstance = this;
        }

        protected override void DefineSettings(SettingCollection settings) => SettingsService = new SettingService(settings);

        public override IView GetSettingsView() => new Settings.Views.ModuleSettingsView();

        #region Setup/Teardown
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        protected override async Task LoadAsync()
        #pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            ApiPollingService = new ApiPollService(SettingsService.ApiPollingPeriod);
            TexturesService = new TextureService(ContentsManager);

            SettingsWindow = SettingPanelFactory.Create();
            RaidsPanel = RaidPanelFactory.Create();
            DungeonsPanel = DungeonPanelFactory.Create();
            StrikesPanel = StrikesPanelFactory.Create();

            Gw2ApiManager.SubtokenUpdated += Gw2ApiManager_SubtokenUpdated;
           
            /*GameService.Overlay.UserLocaleChanged += (s, e) =>
            {
               //todo: refresh views
            };*/
        }

        protected override void Unload()
        {
            Gw2ApiManager.SubtokenUpdated -= Gw2ApiManager_SubtokenUpdated;
            StrikesPanel?.Dispose();
            DungeonsPanel?.Dispose();
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
            DungeonsPanel?.Update();
            StrikesPanel?.Update();

        }

        private void Gw2ApiManager_SubtokenUpdated(object sender, ValueEventArgs<IEnumerable<TokenPermission>> e) => ApiPollingService?.Invoke();


    }
}