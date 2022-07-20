using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using GatheringTools.Raids.Services;
using GatheringTools.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace GatheringTools.Raids.Controls
{
    class RaidsPanel : FlowPanel
    {
        private readonly SettingService _settingService;
        private bool _isDraggedByMouse;
        private Point _mousePressedLocationInsideControl = Point.Zero;

        public RaidsPanel(SettingService settingService, TextureService textureService)
        {
            _settingService = settingService;
            /**  FlowDirection based on Orientation 
             * V             H 1  2  3 L>R
             * T  1 B1 B2 B3   B1 B1 B1
             * v  2 B1 B2 B3   B2 B2 B2
             * B  3 B1 B2 B3   B3 B3 B3
             */
            FlowDirection = settingService.RaidPanelOrientationSetting.Value == Orientation.Vertical ?
                ControlFlowDirection.SingleTopToBottom : ControlFlowDirection.SingleLeftToRight;
            IgnoreMouseInput = !(settingService.RaidPanelDragWithMouseIsEnabledSetting.Value || settingService.RaidPanelAllowTooltipsSetting.Value);
            Location = settingService.RaidPanelLocationPoint.Value;
            Visible = settingService.LogoutButtonIsVisible.Value;
            Parent = GameService.Graphics.SpriteScreen;

            settingService.RaidPanelIsVisible.SettingChanged += (s, e) => Visible = e.NewValue;
            settingService.RaidPanelLocationPoint.SettingChanged += (s, e) => Location = e.NewValue;

            GameService.Input.Mouse.LeftMouseButtonReleased += OnLeftMouseButtonReleased;
        }
        

        protected override void DisposeControl()
        {
            GameService.Input.Mouse.LeftMouseButtonReleased -= OnLeftMouseButtonReleased;
            base.DisposeControl();
        }

#pragma warning disable CS0114 // Member hides inherited member; missing override keyword
        public virtual void DoUpdate(GameTime gameTime)
#pragma warning restore CS0114 // Member hides inherited member; missing override keyword
        {
            if (_isDraggedByMouse && _settingService.RaidPanelDragWithMouseIsEnabledSetting.Value)
            {
                // done via settings instead of directly updating location
                // because otherwise the reset position button would stop working.
                _settingService.RaidPanelLocationPoint.Value = Input.Mouse.Position - _mousePressedLocationInsideControl;
            }
        }

        protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
        {
            if (_settingService.RaidPanelDragWithMouseIsEnabledSetting.Value)
            {
                _isDraggedByMouse = true;
                _mousePressedLocationInsideControl = Input.Mouse.Position - Location;
            }

            base.OnLeftMouseButtonPressed(e);
        }

        // not using the override on purpose because it does not register the release when clicking fast (workaround suggested by freesnow)
        private void OnLeftMouseButtonReleased(object sender, MouseEventArgs e)
        {
            if (_settingService.LogoutButtonDragWithMouseIsEnabledSetting.Value)
                _isDraggedByMouse = false;
        }

        #region MouseClickThrough
        private bool _ignoreMouseInput = false;
  
        public bool IgnoreMouseInput
        {
            get
            {
                return _ignoreMouseInput;
            }
            set
            {
                SetProperty(ref _ignoreMouseInput, value, invalidateLayout: false, "IgnoreMouseInput");
            }
        }

        public override Control TriggerMouseInput(MouseEventType mouseEventType, MouseState ms)
        {
            if (_ignoreMouseInput)
            {
                return null;
            }
            else
            {
                return base.TriggerMouseInput(mouseEventType, ms);

            }
        }
        #endregion

        public void ShowOrHide()
        {
            /*var shouldBeVisible = VisibilityService.ShouldBeVisible(
                _settingService.LogoutButtonIsVisible.Value,
                _settingService.LogoutButtonIsVisibleOnWorldMap.Value,
                _settingService.LogoutButtonIsVisibleOnCutScenesAndCharacterSelect.Value,
                GameService.GameIntegration.Gw2Instance.IsInGame,
                GameService.Gw2Mumble.UI.IsMapOpen == false);*/

           /* if (Visible == false && shouldBeVisible)
                Show();
            else if (Visible && shouldBeVisible == false)
                Hide();*/
        }

        public void Reflow(Orientation orientation, WingLabel label)
        {

        }

        public void OpacityChange(float labelOpacity, float encounterOpacity)
        {

        }

        public void UpdateClearedStatus(string[] clears)
        {

        }

    }
}
