using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using RaidClears.Raids.Model;
using RaidClears.Raids.Services;
using RaidClears.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace RaidClears.Raids.Controls
{
    class RaidsPanel : FlowPanel
    {
        private Logger _logger;
        private Wing[] _wings;
        private readonly SettingService _settingService;
        private bool _isDraggedByMouse = false;
        private Point _dragStart = Point.Zero;

        public RaidsPanel(Logger logger, SettingService settingService, TextureService textureService, Wing[] wings)
        {
            _logger = logger;
            _wings = wings;
            _settingService = settingService;
            ControlPadding = new Vector2(2, 2);
            FlowDirection = GetFlowDirection();
            IgnoreMouseInput = ShouldIgnoreMouse();
            Location = settingService.RaidPanelLocationPoint.Value;
            Visible = settingService.RaidPanelIsVisible.Value;
            Parent = GameService.Graphics.SpriteScreen;
            HeightSizingMode = SizingMode.AutoSize;
            WidthSizingMode = SizingMode.AutoSize;



            CreateWings(wings);

            settingService.RaidPanelIsVisible.SettingChanged += (s, e) => Visible = e.NewValue;
            settingService.RaidPanelLocationPoint.SettingChanged += (s, e) => Location = e.NewValue;

            settingService.RaidPanelOrientationSetting.SettingChanged += (s, e) => OrientationChanged(e.NewValue);
            settingService.RaidPanelWingLabelsSetting.SettingChanged += (s, e) => WingLabelDisplayChanged(e.NewValue);
            settingService.RaidPanelFontSizeSetting.SettingChanged += (s, e) => FontSizeChanged(e.NewValue);

            settingService.RaidPanelWingLabelOpacity.SettingChanged += (s, e) => WingLabelOpacityChanged(e.NewValue);
            settingService.RaidPanelEncounterOpacity.SettingChanged += (s, e) => EncounterOpacityChanged(e.NewValue);

            settingService.RaidPanelDragWithMouseIsEnabledSetting.SettingChanged += (s, e) => IgnoreMouseInput = ShouldIgnoreMouse();
            settingService.RaidPanelAllowTooltipsSetting.SettingChanged += (s, e) => IgnoreMouseInput = ShouldIgnoreMouse();

            settingService.W1IsVisibleSetting.SettingChanged += (s, e) => WingVisibilityChanged(0, e.PreviousValue, e.NewValue);
            settingService.W2IsVisibleSetting.SettingChanged += (s, e) => WingVisibilityChanged(1, e.PreviousValue, e.NewValue);
            settingService.W3IsVisibleSetting.SettingChanged += (s, e) => WingVisibilityChanged(2, e.PreviousValue, e.NewValue);
            settingService.W4IsVisibleSetting.SettingChanged += (s, e) => WingVisibilityChanged(3, e.PreviousValue, e.NewValue);
            settingService.W5IsVisibleSetting.SettingChanged += (s, e) => WingVisibilityChanged(4, e.PreviousValue, e.NewValue);
            settingService.W6IsVisibleSetting.SettingChanged += (s, e) => WingVisibilityChanged(5, e.PreviousValue, e.NewValue);
            settingService.W7IsVisibleSetting.SettingChanged += (s, e) => WingVisibilityChanged(6, e.PreviousValue, e.NewValue);


            AddDragDelegates();


        }

        protected void WingVisibilityChanged(int wingIndex, bool was, bool now)
        {
            _wings[wingIndex].GetWingPanelReference().ShowHide(now);

            Invalidate();
           
        }
       

        protected ControlFlowDirection GetFlowDirection()
        {
            /**  FlowDirection based on Orientation 
             * V             H 1  2  3 L>R
             * T  1 B1 B2 B3   B1 B1 B1
             * v  2 B1 B2 B3   B2 B2 B2
             * B  3 B1 B2 B3   B3 B3 B3
             */
            switch (_settingService.RaidPanelOrientationSetting.Value)
            {
                case Orientation.Horizontal: return ControlFlowDirection.SingleLeftToRight;
                case Orientation.Vertical: return ControlFlowDirection.SingleTopToBottom;
                case Orientation.SingleRow: return ControlFlowDirection.SingleLeftToRight;
                default: return ControlFlowDirection.SingleTopToBottom;
            }
        }


        #region Settings changed handlers
        protected void OrientationChanged(Orientation orientation)
        {
            FlowDirection = GetFlowDirection();
            foreach(var wing in _wings)
            {
                wing.GetWingPanelReference().SetOrientation(orientation);
            }

        }

        protected void WingLabelDisplayChanged(WingLabel labelDisplay)
        {
            foreach (var wing in _wings)
            {
                wing.GetWingPanelReference().SetLabelDisplay(labelDisplay);
            }
        }

        protected void FontSizeChanged(ContentService.FontSize fontSize)
        {
            foreach (var wing in _wings)
            {
                wing.GetWingPanelReference().SetFontSize(fontSize);
            }
        }

        protected void WingLabelOpacityChanged(float opacity)
        {
            foreach (var wing in _wings)
            {
                wing.GetWingPanelReference().SetWingLabelOpacity(opacity);
            }
        }

        protected void EncounterOpacityChanged(float opacity)
        {
            foreach(var wing in _wings)
            {
                wing.GetWingPanelReference().SetEncounterOpacity(opacity);
            }

        }

        #endregion

        protected override void DisposeControl()
        {
            base.DisposeControl();
        }

        #region Mouse Stuff
        public virtual void DoUpdate()

        {
            if (_isDraggedByMouse && _settingService.RaidPanelDragWithMouseIsEnabledSetting.Value)
            {
                var nOffset = InputService.Input.Mouse.Position - _dragStart;
                this.Location += nOffset;

                _dragStart = InputService.Input.Mouse.Position;
            }
        }
        private void AddDragDelegates()
        {
            this.LeftMouseButtonPressed += delegate
            {
                if (_settingService.RaidPanelDragWithMouseIsEnabledSetting.Value)
                {
                    _isDraggedByMouse = true;
                    _dragStart = InputService.Input.Mouse.Position;
                }
            };
            this.LeftMouseButtonReleased += delegate
            {
                if (_settingService.RaidPanelDragWithMouseIsEnabledSetting.Value)
                {
                    _isDraggedByMouse = false;
                    _settingService.RaidPanelLocationPoint.Value = this.Location;
                }
            };
        }

        protected bool ShouldIgnoreMouse()
        {
            return !(
                _settingService.RaidPanelDragWithMouseIsEnabledSetting.Value ||
                _settingService.RaidPanelAllowTooltipsSetting.Value
            );
        }

        private bool _ignoreMouseInput = false;
        public bool IgnoreMouseInput
        {
            get
            {
                return _ignoreMouseInput;
            }
            set
            {
                SetProperty(ref _ignoreMouseInput, value, invalidateLayout: true, "IgnoreMouseInput");
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
            DoUpdate();
           /* if (Visible == false && shouldBeVisible)
                Show();
            else if (Visible && shouldBeVisible == false)
                Hide();*/
        }

        protected void CreateWings(Wing[] wings)
        {
            var wingVis = _settingService.GetWingVisibilitySettings();
            foreach(var wing in wings)
            {
                var wingPanel = new WingPanel(
                    this, 
                    wing, 
                    _settingService.RaidPanelOrientationSetting.Value, 
                    _settingService.RaidPanelWingLabelsSetting.Value,
                    _settingService.RaidPanelFontSizeSetting.Value
                    );
                wing.SetWingPanelReference(wingPanel);
                wingPanel.ShowHide(wingVis[wing.index - 1]);
                AddChild(wingPanel);
                
            }

        }

        public void UpdateClearedStatus(ApiRaids apiraids)
        {
            _logger.Info(apiraids.Clears.ToString());
            foreach(var wing in _wings)
            {
                foreach(var encounter in wing.encounters)
                {
                    var isCleared = apiraids.Clears.Contains(encounter.id);
                    _logger.Info("'{0}' - '{1}'", encounter.id, isCleared.ToString());
                    encounter.SetCleared(apiraids.Clears.Contains(encounter.id));
                }
            }
            Invalidate();

        }

    }
}
