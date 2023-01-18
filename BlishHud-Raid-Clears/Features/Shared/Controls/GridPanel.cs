using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Blish_HUD.Settings;
using RaidClears.Features.Shared.Services;

namespace RaidClears.Features.Shared.Controls;

public class GridPanel : FlowPanel
{
    private bool _isDraggedByMouse;
    private Point _dragStart = Point.Zero;

    private readonly SettingEntry<Point> _locationSetting;
    private readonly SettingEntry<bool> _visibleSetting;
    private readonly SettingEntry<bool> _allowMouseDragSetting;
    private readonly SettingEntry<bool> _allowTooltipSetting;

    private CornerIconService? _cornerIconService;
    private KeybindHandlerService? _keyBindService;

    protected GridPanel(
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
        HeightSizingMode = SizingMode.AutoSize; //warning
        WidthSizingMode = SizingMode.AutoSize; //warning
        
        AddDragDelegates();
        _locationSetting.SettingChanged += (_, e) => Location = e.NewValue;
        _allowMouseDragSetting.SettingChanged += (_, _) => IgnoreMouseInput = ShouldIgnoreMouse();
        _allowTooltipSetting.SettingChanged += (_, _) => IgnoreMouseInput = ShouldIgnoreMouse();
    }

    protected override void DisposeControl()
    {
        base.DisposeControl();
        _cornerIconService?.Dispose();
        _keyBindService?.Dispose();
    }

    #region Mouse Stuff
    private void DoUpdate()
    {
        if (_isDraggedByMouse && _allowMouseDragSetting.Value)
        {
            var nOffset = GameService.Input.Mouse.Position - _dragStart;
            Location += nOffset;

            _dragStart = GameService.Input.Mouse.Position;
        }
    }

    private void AddDragDelegates()
    {
        LeftMouseButtonPressed += delegate
        {
            if (_allowMouseDragSetting.Value)
            {
                _isDraggedByMouse = true;
                _dragStart = GameService.Input.Mouse.Position;
            }
        };
        LeftMouseButtonReleased += delegate
        {
            if (_allowMouseDragSetting.Value)
            {
                _isDraggedByMouse = false;
                _locationSetting.Value = Location;
            }
        };
    }

    private bool ShouldIgnoreMouse()
    {
        return !(
            _allowMouseDragSetting.Value ||
            _allowTooltipSetting.Value
        );
    }

    private bool _ignoreMouseInput;

    private bool IgnoreMouseInput
    {
        set => SetProperty(ref _ignoreMouseInput, value, invalidateLayout: true);
    }

    public override Control? TriggerMouseInput(MouseEventType mouseEventType, MouseState ms) => _ignoreMouseInput ? null : base.TriggerMouseInput(mouseEventType, ms);
    #endregion

    #region event handlers
    public void RegisterCornerIconService(CornerIconService? service)
    {
        _cornerIconService = service;
    }
    
    public void RegisterKeyBindService(KeybindHandlerService? service)
    {
        _keyBindService= service;
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
