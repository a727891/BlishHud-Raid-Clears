﻿using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using RaidClears.Features.Shared.Services;
using RaidClears.Settings.Models;

namespace RaidClears.Features.Shared.Controls;

public class GridPanel : FlowPanel
{
    private static Vector2 DefaultPadding = new(2, 2);

    private readonly GenericSettings _settings;
    private readonly SettingEntry<bool> _screenClamp;

    private CornerIconService? _cornerIconService;
    private KeyBindHandlerService? _keyBindService;

    private bool _ignoreMouseInput;
    private bool _isDraggedByMouse;
    private Point _dragStart = Point.Zero;

    private bool IgnoreMouseInput
    {
        set => SetProperty(ref _ignoreMouseInput, value, invalidateLayout: true);
    }

    protected GridPanel(GenericSettings settings, Container parent)
    {
        _settings = settings;
        _screenClamp = Service.Settings.ScreenClamp;

        ControlPadding = DefaultPadding;
        IgnoreMouseInput = ShouldIgnoreMouse();
        Location = _settings.Location.Value;
        Visible = _settings.Visible.Value;
        Parent = parent;
        HeightSizingMode = SizingMode.AutoSize; 
        WidthSizingMode = SizingMode.AutoSize;
        
        AddDragDelegates();
        _settings.Location.SettingChanged += (_, e) => Location = e.NewValue;
        _settings.PositionLock.SettingChanged += (_, _) => IgnoreMouseInput = ShouldIgnoreMouse();
        _settings.Tooltips.SettingChanged += (_, _) => IgnoreMouseInput = ShouldIgnoreMouse();

        _screenClamp.SettingChanged += (_, e) =>
        {
            if (e.NewValue)
            {
                ClampToSpriteScreen();
            }
        };
    }

    protected override void DisposeControl()
    {
        base.DisposeControl();
        _cornerIconService?.Dispose();
        _keyBindService?.Dispose();
    }

    private void DoUpdate()
    {
        if (_isDraggedByMouse && _settings.PositionLock.Value)
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
            if (_settings.PositionLock.Value)
            {
                _isDraggedByMouse = true;
                _dragStart = GameService.Input.Mouse.Position;
            }
        };
        LeftMouseButtonReleased += delegate
        {
            if (_settings.PositionLock.Value)
            {
                _isDraggedByMouse = false;
                ClampToSpriteScreen();
                
            }
        };
    }

    private void ClampToSpriteScreen()
    {
        if (_screenClamp.Value)
        {
            Point screenSize = GameService.Graphics.SpriteScreen.Size;
            if(Location.X < 0)
            {
                Location = new Point(0, Location.Y);
            }
            if ((Location.X+Size.X) > screenSize.X)
            {
                Location = new Point(screenSize.X - Size.X, Location.Y);
            }

            if (Location.Y < 0)
            {
                Location = new Point(Location.X, 0);
            }
            if ((Location.Y+Size.Y) > screenSize.Y)
            {
                Location = new Point(Location.X, screenSize.Y - Size.Y);
            }
        }

        _settings.Location.Value = Location;
    }

    private bool ShouldIgnoreMouse() => !(_settings.PositionLock.Value || _settings.Tooltips.Value);
        

    public override Control? TriggerMouseInput(MouseEventType mouseEventType, MouseState ms) => _ignoreMouseInput ? null : base.TriggerMouseInput(mouseEventType, ms);

    public void RegisterCornerIconService(CornerIconService? service) => _cornerIconService = service;
    
    
    public void RegisterKeyBindService(KeyBindHandlerService? service) => _keyBindService= service;

    public void Update()
    {
        var shouldBeVisible =
          _settings.Visible.Value &&
          GameService.GameIntegration.Gw2Instance.Gw2IsRunning &&
          GameService.GameIntegration.Gw2Instance.IsInGame &&
          GameService.Gw2Mumble.IsAvailable &&
          !GameService.Gw2Mumble.UI.IsMapOpen;
       
        if(shouldBeVisible && _settings.PositionLock.Value)
            DoUpdate();

        if (!Visible && shouldBeVisible)
            Show();
        else if (Visible && !shouldBeVisible)
            Hide();
    }
}
