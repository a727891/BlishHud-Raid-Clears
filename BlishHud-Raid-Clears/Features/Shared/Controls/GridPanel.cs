using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;

using RaidClears.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Blish_HUD.Settings;
using RaidClears.Features.Shared.Services;

namespace RaidClears.Raids.Controls
{

    public class GridPanel : FlowPanel
    {

        protected bool _isDraggedByMouse = false;
        protected Point _dragStart = Point.Zero;

        protected SettingEntry<Point> _locationSetting;
        protected SettingEntry<bool> _visibleSetting;
        protected SettingEntry<bool> _allowMouseDragSetting;
        protected SettingEntry<bool> _allowTooltipSetting;


        protected CornerIconService _cornerIconService;
        protected KeybindHandlerService _keybindService;
        

        public GridPanel(
            SettingEntry<Point> locationSetting,
            SettingEntry<bool> visibleSetting,
            SettingEntry<bool> allowMouseDragSetting,
            SettingEntry<bool> allowTooltipSetting)
        {

            _locationSetting = locationSetting;
            _visibleSetting = visibleSetting;
            _allowMouseDragSetting = allowMouseDragSetting;
            _allowTooltipSetting = allowTooltipSetting;
            

            ControlPadding = new Vector2(2, 2);
            IgnoreMouseInput = ShouldIgnoreMouse();
            Location = _locationSetting.Value;
            Visible = _visibleSetting.Value;
            Parent = GameService.Graphics.SpriteScreen;
            HeightSizingMode = SizingMode.AutoSize;
            WidthSizingMode = SizingMode.AutoSize;
            
            AddDragDelegates();
            _locationSetting.SettingChanged += (s, e) => Location = e.NewValue;
            _allowMouseDragSetting.SettingChanged += (s, e) => IgnoreMouseInput = ShouldIgnoreMouse();
            _allowTooltipSetting.SettingChanged += (s, e) => IgnoreMouseInput = ShouldIgnoreMouse();


        }

        protected override void DisposeControl()
        {
            base.DisposeControl();
            _cornerIconService?.Dispose();
            _keybindService?.Dispose();
        }

        #region Mouse Stuff
        public virtual void DoUpdate()

        {
            if (_isDraggedByMouse && _allowMouseDragSetting.Value)
            {
                var nOffset = InputService.Input.Mouse.Position - _dragStart;
                this.Location += nOffset;

                _dragStart = InputService.Input.Mouse.Position;
            }
        }
        protected void AddDragDelegates()
        {
            this.LeftMouseButtonPressed += delegate
            {
                if (_allowMouseDragSetting.Value)
                {
                    _isDraggedByMouse = true;
                    _dragStart = InputService.Input.Mouse.Position;
                }
            };
            this.LeftMouseButtonReleased += delegate
            {
                if (_allowMouseDragSetting.Value)
                {
                    _isDraggedByMouse = false;
                    _locationSetting.Value = this.Location;
                }
            };
        }

        protected bool ShouldIgnoreMouse()
        {
            return !(
                _allowMouseDragSetting.Value ||
                _allowTooltipSetting.Value
            );
        }

        protected bool _ignoreMouseInput = false;
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


        #region event handlers
        public void RegisterCornerIconService(CornerIconService service)
        {
            _cornerIconService = service;
        }
        public void RegisterKeybindService(KeybindHandlerService service)
        {
            _keybindService= service;
        }
        #endregion

        public void Update()
        {
            var shouldBeVisible =
              _visibleSetting.Value &&
              GameService.GameIntegration.Gw2Instance.Gw2IsRunning &&
              GameService.GameIntegration.Gw2Instance.IsInGame &&
              GameService.Gw2Mumble.IsAvailable &&
              !GameService.Gw2Mumble.UI.IsMapOpen;
           
            if(shouldBeVisible && _allowMouseDragSetting.Value)
                DoUpdate();

            if (!Visible && shouldBeVisible)
                Show();
            else if (Visible && !shouldBeVisible)
                Hide();
        }


    }
}
