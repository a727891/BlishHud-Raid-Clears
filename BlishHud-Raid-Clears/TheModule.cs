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

using GatheringTools.Settings;
using GatheringTools.Raids.Controls;
using GatheringTools.Raids.Model;
using GatheringTools.Raids.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GatheringTools
{
    [Export(typeof(Module))]
    public class TheModule : Module
    {
        private static readonly Logger Logger = Logger.GetLogger<TheModule>();
        internal SettingsManager SettingsManager => ModuleParameters.SettingsManager;
        internal ContentsManager ContentsManager => ModuleParameters.ContentsManager;
        internal DirectoriesManager DirectoriesManager => ModuleParameters.DirectoriesManager;
        internal Gw2ApiManager Gw2ApiManager => ModuleParameters.Gw2ApiManager;

        [ImportingConstructor]
        public TheModule([Import("ModuleParameters")] ModuleParameters moduleParameters) : base(moduleParameters)
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

        #region LifeCycle
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        protected override async Task LoadAsync()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            _textureService    = new TextureService(ContentsManager);
    
            _raidsPanel = new RaidsPanel(_settingService, _textureService);
            

            _settingService.RaidPanelIsVisibleKeyBind.Value.Activated += (s, e) => _settingService.ToggleRaidPanelVisibility();

            _cornerIconService = new CornerIconService(
                _settingService.ShowRaidsCornerIconSetting,
                "Click to show/hide the Raid Clears window.\nIcon can be hidden by module settings.",
                (s, e) => _settingService.ToggleRaidPanelVisibility(),
                _textureService);
        }

        protected override void Unload()
        {
            _raidsPanel?.Dispose();
            _textureService?.Dispose();
            _cornerIconService?.Dispose();
        }


        protected override void Update(GameTime gameTime)
        {
            //_logoutButton?.ShowOrHide();
            _raidsPanel?.ShowOrHide();
            //HideReminderWhenDurationEnds(gameTime);
        }
        #endregion





        private double _runningTime;
        //private ToolSearchStandardWindow _toolSearchStandardWindow;
        //private ReminderContainer _reminderContainer;
        //private KeyBinding _escKeyBinding;
        //private KeyBinding _enterKeyBinding;
        private SettingService _settingService;
        private TextureService _textureService;
        private CornerIconService _cornerIconService;
        //private readonly List<GatheringTool> _allGatheringTools = new List<GatheringTool>();
        //private LogoutButton _logoutButton;
        private RaidsPanel _raidsPanel;
    }
}