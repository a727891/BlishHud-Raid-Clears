using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using RaidClears.Dungeons.Model;
using RaidClears.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace RaidClears.Dungeons.Controls
{
    class DungeonsPanel : FlowPanel
    {
        private Logger _logger;
        private Model.Dungeon[] _dungeons;
        private readonly SettingService _settingService;
        private bool _isDraggedByMouse = false;
        private Point _dragStart = Point.Zero;

        public DungeonsPanel(Logger logger, SettingService settingService, Model.Dungeon[] dungeons)
        {
            _logger = logger;
            _dungeons = dungeons;
            _settingService = settingService;

            ControlPadding = new Vector2(2, 2);
            FlowDirection = GetFlowDirection();
            IgnoreMouseInput = ShouldIgnoreMouse();
            Location = settingService.DungeonPanelLocationPoint.Value;
            Visible = settingService.DungeonPanelIsVisible.Value;
            Parent = GameService.Graphics.SpriteScreen;
            HeightSizingMode = SizingMode.AutoSize;
            WidthSizingMode = SizingMode.AutoSize;


            CreateDungeons(dungeons);

            settingService.DungeonPanelLocationPoint.SettingChanged += (s, e) => Location = e.NewValue;

            settingService.DungeonPanelOrientationSetting.SettingChanged += (s, e) => OrientationChanged(e.NewValue);
            settingService.DungeonPanelWingLabelsSetting.SettingChanged += (s, e) => WingLabelDisplayChanged(e.NewValue);
            settingService.DungeonPanelFontSizeSetting.SettingChanged += (s, e) => FontSizeChanged(e.NewValue);

            settingService.DungeonPanelWingLabelOpacity.SettingChanged += (s, e) => WingLabelOpacityChanged(e.NewValue);
            settingService.DungeonPanelEncounterOpacity.SettingChanged += (s, e) => EncounterOpacityChanged(e.NewValue);

            settingService.DragWithMouseIsEnabledSetting.SettingChanged += (s, e) => IgnoreMouseInput = ShouldIgnoreMouse();
            settingService.AllowTooltipsSetting.SettingChanged += (s, e) => IgnoreMouseInput = ShouldIgnoreMouse();

            settingService.D1IsVisibleSetting.SettingChanged += (s, e) => DungeonVisibilityChanged(0, e.PreviousValue, e.NewValue);
            settingService.D2IsVisibleSetting.SettingChanged += (s, e) => DungeonVisibilityChanged(1, e.PreviousValue, e.NewValue);
            settingService.D3IsVisibleSetting.SettingChanged += (s, e) => DungeonVisibilityChanged(2, e.PreviousValue, e.NewValue);
            settingService.D4IsVisibleSetting.SettingChanged += (s, e) => DungeonVisibilityChanged(3, e.PreviousValue, e.NewValue);
            settingService.D5IsVisibleSetting.SettingChanged += (s, e) => DungeonVisibilityChanged(4, e.PreviousValue, e.NewValue);
            settingService.D6IsVisibleSetting.SettingChanged += (s, e) => DungeonVisibilityChanged(5, e.PreviousValue, e.NewValue);
            settingService.D7IsVisibleSetting.SettingChanged += (s, e) => DungeonVisibilityChanged(6, e.PreviousValue, e.NewValue);
            settingService.D8IsVisibleSetting.SettingChanged += (s, e) => DungeonVisibilityChanged(7, e.PreviousValue, e.NewValue);

            WingLabelOpacityChanged(settingService.DungeonPanelWingLabelOpacity.Value);
            EncounterOpacityChanged(settingService.DungeonPanelEncounterOpacity.Value);

            AddDragDelegates();


        }

        protected void DungeonVisibilityChanged(int wingIndex, bool was, bool now)
        {
            _dungeons[wingIndex].GetPanelReference().ShowHide(now);

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
            switch (_settingService.DungeonPanelOrientationSetting.Value)
            {
                case DungeonOrientation.Horizontal: return ControlFlowDirection.SingleLeftToRight;
                case DungeonOrientation.Vertical: return ControlFlowDirection.SingleTopToBottom;
                case DungeonOrientation.SingleRow: return ControlFlowDirection.SingleLeftToRight;
                default: return ControlFlowDirection.SingleTopToBottom;
            }
        }


        #region Settings changed handlers
        protected void OrientationChanged(DungeonOrientation orientation)
        {
            FlowDirection = GetFlowDirection();
            foreach(var wing in _dungeons)
            {
                wing.GetPanelReference().SetOrientation(orientation);
            }

        }

        protected void WingLabelDisplayChanged(DungeonLabel labelDisplay)
        {
            foreach (var wing in _dungeons)
            {
                wing.GetPanelReference().SetLabelDisplay(labelDisplay);
            }
        }

        protected void FontSizeChanged(ContentService.FontSize fontSize)
        {
            foreach (var wing in _dungeons)
            {
                wing.GetPanelReference().SetFontSize(fontSize);
            }
        }

        protected void WingLabelOpacityChanged(float opacity)
        {
            foreach (var wing in _dungeons)
            {
                wing.GetPanelReference().SetWingLabelOpacity(opacity);
            }
        }

        protected void EncounterOpacityChanged(float opacity)
        {
            foreach(var wing in _dungeons)
            {
                wing.GetPanelReference().SetEncounterOpacity(opacity);
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
            if (_isDraggedByMouse && _settingService.DragWithMouseIsEnabledSetting.Value)
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
                if (_settingService.DragWithMouseIsEnabledSetting.Value)
                {
                    _isDraggedByMouse = true;
                    _dragStart = InputService.Input.Mouse.Position;
                }
            };
            this.LeftMouseButtonReleased += delegate
            {
                if (_settingService.DragWithMouseIsEnabledSetting.Value)
                {
                    _isDraggedByMouse = false;
                    _settingService.DungeonPanelLocationPoint.Value = this.Location;
                }
            };
        }

        protected bool ShouldIgnoreMouse()
        {
            return !(
                _settingService.DragWithMouseIsEnabledSetting.Value ||
                _settingService.AllowTooltipsSetting.Value
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
            var shouldBeVisible =
              _settingService.DungeonPanelIsVisible.Value &&
              GameService.GameIntegration.Gw2Instance.Gw2IsRunning &&
              GameService.GameIntegration.Gw2Instance.IsInGame &&
              GameService.Gw2Mumble.IsAvailable &&
              !GameService.Gw2Mumble.UI.IsMapOpen;
            /*var shouldBeVisible = VisibilityService.ShouldBeVisible(
                _settingService.LogoutButtonIsVisible.Value,
                _settingService.LogoutButtonIsVisibleOnWorldMap.Value,
                _settingService.LogoutButtonIsVisibleOnCutScenesAndCharacterSelect.Value,
                GameService.GameIntegration.Gw2Instance.IsInGame,
                GameService.Gw2Mumble.UI.IsMapOpen == false);*/

            if(shouldBeVisible && _settingService.DragWithMouseIsEnabledSetting.Value)
                DoUpdate();

            if (Visible == false && shouldBeVisible)
                Show();
            else if (Visible && shouldBeVisible == false)
                Hide();
        }

        protected void CreateDungeons(Dungeon[] dungeons)
        {
            var wingVis = _settingService.GetDungeonVisibilitySettings();
            foreach(var dungeon in dungeons)
            {
                var wingPanel = new PathsPanel(
                    this, 
                    dungeon, 
                    _settingService.DungeonPanelOrientationSetting.Value, 
                    _settingService.DungeonPanelWingLabelsSetting.Value,
                    _settingService.DungeonPanelFontSizeSetting.Value
                    );
                dungeon.SetPanelReference(wingPanel);
                wingPanel.ShowHide(wingVis[dungeon.index - 1]);
                AddChild(wingPanel);
                
            }

        }

        public void UpdateClearedStatus(ApiDungeons apidungeons)
        {
            //_logger.Info(apidungeons.Clears.ToString());
            foreach(var dungeon in _dungeons)
            {
                foreach(var path in dungeon.paths)
                {
                    //var isCleared = apidungeons.Clears.Contains(path.id);
                    //_logger.Info("'{0}' - '{1}'", path.id, isCleared.ToString());
                    path.SetCleared(apidungeons.Clears.Contains(path.id));
                    path.SetFrequenter(apidungeons.Frequenter.Contains(path.id));
                }
            }
            Invalidate();

        }

    }
}
