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

using RaidClears.Settings;
using RaidClears.Raids.Controls;
using RaidClears.Raids.Model;
using RaidClears.Raids.Services;
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
            return new ModuleSettingsView(_settingService);
        }
        #endregion

        #region StaticData
        private static Wing[] wingInfo = new Wing[] {
            new Wing("Spirit Vale", 1, "SV",
                new Encounter[] {
                    new Encounter("vale_guardian", "Vale Guardian", "VG"),
                    new Encounter("spirit_woods", "Spirit Run", "SR"),
                    new Encounter("gorseval", "Gorseval", "G"),
                    new Encounter("sabetha", "Sabetha", "S"),
                }),
            new Wing("Salvation Pass", 2, "SP",
                new Encounter[] {
                    new Encounter("slothasor", "Slothasor", "S"),
                    new Encounter("bandit_trio", "Bandit Trio", "B3"),
                    new Encounter("matthias", "Matthias Gabrel", "M"),
                }),
            new Wing("Stronghold of the Faithful", 3, "SotF",
                new Encounter[] {
                    new Encounter("escort", "Escort", "E"),
                    new Encounter("keep_construct", "Keep Construct", "KC"),
                    new Encounter("twisted_castle", "Twisted Castel", "TC"),
                    new Encounter("xera", "Xera", "X"),
                }),
            new Wing("Bastion of the Penitent", 4, "BotP",
                new Encounter[] {
                    new Encounter("cairn", "Cairn the Indominable", "C"),
                    new Encounter("mursaat_overseer", "Mursaat Overseer", "MO"),
                    new Encounter("samarog", "Samarog", "S"),
                    new Encounter("deimos", "Deimos", "D"),
                }),
            new Wing("Hall of Chains", 5, "HoC",
                new Encounter[] {
                    new Encounter("soulless_horror", "Soulless Horror", "SH"),
                    new Encounter("river_of_souls", "River of Souls", "R"),
                    new Encounter("statues_of_grenth", "Statues of Grenth", "S"),
                    new Encounter("voice_in_the_void", "Dhuum", "D"),
                }),
            new Wing("Mythwright Gambit", 6, "MG",
                new Encounter[] {
                    new Encounter("conjured_amalgamate", "Conjured Amalgamate", "CA"),
                    new Encounter("twin_largos", "Twin Largos", "TL"),
                    new Encounter("qadim", "Qadim", "Q1"),
                }),
            new Wing("The Key of Ahdashim", 7, "TKoA",
                new Encounter[] {
                    new Encounter("gate", "Gate", "G"),
                    new Encounter("adina", "Cardinal Adina", "A"),
                    new Encounter("sabir", "Cardinal Sabir", "S"),
                    new Encounter("qadim_the_peerless", "Qadim the Peerless", "Q2"),
                })
        };
        #endregion

        #region Setup/Teardown
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        protected override async Task LoadAsync()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            _textureService = new TextureService(ContentsManager);
    
            _raidsPanel = new RaidsPanel(Logger, _settingService, _textureService, wingInfo);


            _settingService.RaidPanelIsVisibleKeyBind.Value.Activated += OnRaidPanelDisplayKeybindActivated;

            _cornerIconService = new CornerIconService(
                _settingService.ShowRaidsCornerIconSetting,
                "Click to show/hide the Raid Clears window.\nIcon can be hidden by module settings.",
                (s, e) => _settingService.ToggleRaidPanelVisibility(),
                _textureService);
        }

        protected override void Unload()
        {
            _settingService.RaidPanelIsVisibleKeyBind.Value.Activated -= OnRaidPanelDisplayKeybindActivated;

            _raidsPanel?.Dispose();
            _textureService?.Dispose();
            _cornerIconService?.Dispose();
        }



        #endregion
        protected override void Update(GameTime gameTime)
        {
            //_logoutButton?.ShowOrHide();
            _raidsPanel?.ShowOrHide();

            //HideReminderWhenDurationEnds(gameTime);
        }

        private void OnRaidPanelDisplayKeybindActivated(object sender, EventArgs e)
        {
            _settingService.ToggleRaidPanelVisibility();
        }


        private SettingService _settingService;
        private TextureService _textureService;
        private CornerIconService _cornerIconService;
        private RaidsPanel _raidsPanel;
    }
}