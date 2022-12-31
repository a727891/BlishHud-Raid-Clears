using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using RaidClears.Raids.Model;
using RaidClears.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Settings.Enums;
using RaidClears.Raids.Services;

namespace RaidClears.Raids.Controls
{
    class RaidsPanel : FlowPanel
    {
        private Logger _logger;
        private Wing[] _wings;

        //private WingPanel _fractalT4Panel;

        private readonly SettingService _settingService;
        private readonly WingRotationService _wingRotation;
        private bool _isDraggedByMouse = false;
        private Point _dragStart = Point.Zero;

        private Color CallOfTheMistColor = new Color(243, 245, 39);
        private Color EmboldenColor = new Color(80, 80, 255);
        private Color TextColor = Color.White;
        private Color NotClearedColor = new Color(120, 20, 20);
        private Color ClearedColor = new Color(20, 120, 20);

        private int _cotmIndex = 0;
        private int _emboldenIndex = 0;

        public RaidsPanel(Logger logger, SettingService settingService, Wing[] wings, WingRotationService wingRotation)
        {
            _logger = logger;
            _wings = wings;
            _settingService = settingService;
            _wingRotation = wingRotation;

            ControlPadding = new Vector2(2, 2);
            FlowDirection = GetFlowDirection();
            IgnoreMouseInput = ShouldIgnoreMouse();
            Location = settingService.RaidPanelLocationPoint.Value;
            Visible = settingService.RaidPanelIsVisible.Value;
            Parent = GameService.Graphics.SpriteScreen;
            HeightSizingMode = SizingMode.AutoSize;
            WidthSizingMode = SizingMode.AutoSize;

            (_emboldenIndex, _cotmIndex) = wingRotation.getHighlightedWingIndices();
            wings[_emboldenIndex].setEmboldened(true);
            wings[_cotmIndex].setCallOfTheMist(true);

            InitColors(settingService);

            CreateWings(wings);

            //settingService.RaidPanelIsVisible.SettingChanged += (s, e) => Visible = e.NewValue;
            settingService.RaidPanelLocationPoint.SettingChanged += (s, e) => Location = e.NewValue;

            settingService.RaidPanelOrientationSetting.SettingChanged += (s, e) => OrientationChanged(e.NewValue);
            settingService.RaidPanelWingLabelsSetting.SettingChanged += (s, e) => WingLabelDisplayChanged(e.NewValue);
            settingService.RaidPanelFontSizeSetting.SettingChanged += (s, e) => FontSizeChanged(e.NewValue);

            settingService.RaidPanelWingLabelOpacity.SettingChanged += (s, e) => WingLabelOpacityChanged(e.NewValue);
            settingService.RaidPanelEncounterOpacity.SettingChanged += (s, e) => EncounterOpacityChanged(e.NewValue);

            settingService.DragWithMouseIsEnabledSetting.SettingChanged += (s, e) => IgnoreMouseInput = ShouldIgnoreMouse();
            settingService.AllowTooltipsSetting.SettingChanged += (s, e) => IgnoreMouseInput = ShouldIgnoreMouse();

            settingService.W1IsVisibleSetting.SettingChanged += (s, e) => WingVisibilityChanged(0, e.PreviousValue, e.NewValue);
            settingService.W2IsVisibleSetting.SettingChanged += (s, e) => WingVisibilityChanged(1, e.PreviousValue, e.NewValue);
            settingService.W3IsVisibleSetting.SettingChanged += (s, e) => WingVisibilityChanged(2, e.PreviousValue, e.NewValue);
            settingService.W4IsVisibleSetting.SettingChanged += (s, e) => WingVisibilityChanged(3, e.PreviousValue, e.NewValue);
            settingService.W5IsVisibleSetting.SettingChanged += (s, e) => WingVisibilityChanged(4, e.PreviousValue, e.NewValue);
            settingService.W6IsVisibleSetting.SettingChanged += (s, e) => WingVisibilityChanged(5, e.PreviousValue, e.NewValue);
            settingService.W7IsVisibleSetting.SettingChanged += (s, e) => WingVisibilityChanged(6, e.PreviousValue, e.NewValue);
            //settingService.FrIsVisibleSetting.SettingChanged += (s, e) => _fractalT4Panel.ShowHide(e.NewValue);

            settingService.RaidPanelHighlightEmbolden.SettingChanged += (s, e) => EmboldenChanged(_emboldenIndex, e.NewValue);
            settingService.RaidPanelHighlightCotM.SettingChanged += (s, e) => CotMChanged(_cotmIndex, e.NewValue);

            //settingService.RaidPanelColorUnknown.SettingChanged     += (s, e) => ColorChanged("unknown", e.NewValue);
            settingService.RaidPanelColorCleared.SettingChanged     += (s, e) => ColorChanged("cleared", e.NewValue);
            settingService.RaidPanelColorNotCleared.SettingChanged  += (s, e) => ColorChanged("notCleared", e.NewValue);
            settingService.RaidPanelColorText.SettingChanged        += (s, e) => ColorChanged("text", e.NewValue);
            settingService.RaidPanelColorCotm.SettingChanged        += (s, e) => ColorChanged("cotm", e.NewValue);
            settingService.RaidPanelColorEmbolden.SettingChanged    += (s, e) => ColorChanged("embolden", e.NewValue);


            WingLabelOpacityChanged(settingService.RaidPanelWingLabelOpacity.Value);
            EncounterOpacityChanged(settingService.RaidPanelEncounterOpacity.Value);

            EmboldenChanged(_emboldenIndex, settingService.RaidPanelHighlightEmbolden.Value);
            CotMChanged(_cotmIndex, settingService.RaidPanelHighlightCotM.Value);

            AddDragDelegates();


        }

        private void InitColors(SettingService _settings)
        {
            var emboldenColor = new ColorHelper(_settings.RaidPanelColorEmbolden.Value);
            EmboldenColor = emboldenColor.XnaColor;

            var cotmColor = new ColorHelper(_settings.RaidPanelColorCotm.Value);
            CallOfTheMistColor = cotmColor.XnaColor;

            var textColor = new ColorHelper(_settings.RaidPanelColorText.Value);
            TextColor = textColor.XnaColor;

            var clearedColor = new ColorHelper(_settings.RaidPanelColorCleared.Value);
            ClearedColor = clearedColor.XnaColor;

            var notClearedColor = new ColorHelper(_settings.RaidPanelColorNotCleared.Value);
            NotClearedColor = notClearedColor.XnaColor;
        }

        protected void ColorChanged(string type, string hexCode)
        {
            var _colorHelper = new ColorHelper(hexCode);
            switch (type)
            {
                case "unknown":
                    break;
                case "cleared":
                    ClearedColor = _colorHelper.XnaColor;
                    break;
                case "notCleared":
                    NotClearedColor = _colorHelper.XnaColor;
                    break;
                case "text":
                    TextColor = _colorHelper.XnaColor;
                    break;
                case "cotm":
                    CallOfTheMistColor = _colorHelper.XnaColor;
                    break;
                case "embolden":
                    EmboldenColor = _colorHelper.XnaColor;
                    break;
                default: break;
            }

            for(var i=0; i < _wings.Length; i++)
            {
                var textColor = TextColor;

                var wing =_wings[i];
                if (wing.isEmboldened && _settingService.RaidPanelHighlightEmbolden.Value)
                {
                    textColor = EmboldenColor;
                }
                if (wing.isCallOfTheMist && _settingService.RaidPanelHighlightCotM.Value)
                {
                    textColor = CallOfTheMistColor;
                }
                wing.GetWingPanelReference().UpdateEncounterColors(ClearedColor, NotClearedColor);
                wing.GetWingPanelReference().SetHighlightColor(textColor);

                foreach( var encounter in wing.encounters)
                {
                    encounter.UpdateColors(ClearedColor, NotClearedColor);
                }
            }
        }

        protected void WingVisibilityChanged(int wingIndex, bool was, bool now)
        {
            _wings[wingIndex].GetWingPanelReference().ShowHide(now);

            Invalidate();
           
        }

        protected void EmboldenChanged(int wingIndex, bool highlight)
        {
            _wings[wingIndex].GetWingPanelReference().SetHighlightColor(highlight ?
                EmboldenColor : TextColor
            );
        }

        protected void CotMChanged(int wingIndex, bool highlight)
        {
            _wings[wingIndex].GetWingPanelReference().SetHighlightColor(highlight ?
               CallOfTheMistColor : TextColor
           );
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
                    _settingService.RaidPanelLocationPoint.Value = this.Location;
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
              _settingService.RaidPanelIsVisible.Value &&
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
                    _settingService.RaidPanelFontSizeSetting.Value,
                    ClearedColor,
                    NotClearedColor
                    );
                wing.SetWingPanelReference(wingPanel);
                wing.GetWingPanelReference().SetHighlightColor(TextColor);
                wingPanel.ShowHide(wingVis[wing.index - 1]);
                AddChild(wingPanel);
                
            }

            /*_fractalT4Panel = new WingPanel(
                this,
                new Wing("Tier 4 Fractals", 0, "T4s", new[]{
                    new Encounter("","","aeth"),
                    new Encounter("","","mai"),
                    new Encounter("","","volc"),
                    }
                ),
                _settingService.RaidPanelOrientationSetting.Value,
                _settingService.RaidPanelWingLabelsSetting.Value,
                _settingService.RaidPanelFontSizeSetting.Value,
                NotClearedColor,
                NotClearedColor
                );
            _fractalT4Panel.ShowHide(_settingService.FrIsVisibleSetting.Value);
            AddChild(_fractalT4Panel);*/

        }

        public void UpdateClearedStatus(ApiRaids apiraids)
        {
            //_logger.Info(apiraids.Clears.ToString());
            foreach(var wing in _wings)
            {
                foreach(var encounter in wing.encounters)
                {
                    //var isCleared = apiraids.Clears.Contains(encounter.id);
                    //_logger.Info("'{0}' - '{1}'", encounter.id, isCleared.ToString());
                    encounter.SetCleared(apiraids.Clears.Contains(encounter.id));
                }
            }
            Invalidate();

        }

    }
}
