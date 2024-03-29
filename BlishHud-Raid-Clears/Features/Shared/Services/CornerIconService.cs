﻿using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework.Graphics;
using RaidClears.Localization;

namespace RaidClears.Features.Shared.Services;

public class CornerIconToggleMenuItem: ContextMenuStripItem
{ 
    public CornerIconToggleMenuItem(SettingEntry<bool> setting, string displayLabel) : base(displayLabel)
    {
        var baseText = displayLabel;
        Text = (setting.Value ? Strings.VisibleHide : Strings.VisibleShow) + " " + baseText;

        Click += delegate { setting.Value = !setting.Value; };
        setting.SettingChanged += delegate
        {
            Text = (setting.Value ? Strings.VisibleHide : Strings.VisibleShow) + " " + baseText;
        };
    }

    public CornerIconToggleMenuItem(Control control, string displayLabel) : base(displayLabel)
    {
        Click += delegate { control.Show(); };
    }
}
public class CornerIconService : IDisposable
{
    public event EventHandler<bool>? IconLeftClicked;

    public CornerIconService(SettingEntry<bool> cornerIconIsVisibleSetting,
                             string tooltip,
                             Texture2D defaultTexture,
                             Texture2D hoverTexture,
                             IEnumerable<ContextMenuStripItem> contextMenuItems
    )
    {
        _tooltip                     = tooltip;
        _cornerIconIsVisibleSetting  = cornerIconIsVisibleSetting;
        _cornerIconTexture           = defaultTexture;
        _cornerIconHoverTexture      = hoverTexture;
        _contextMenuItems = contextMenuItems;
        cornerIconIsVisibleSetting.SettingChanged += OnCornerIconIsVisibleSettingChanged;
        Service.Settings.CornerIconPriority.SettingChanged += CornerIconPriority_SettingChanged;

        if (cornerIconIsVisibleSetting.Value)
            CreateCornerIcon();
    }

    public void UpdateAccountName(string name)
    {
        if(_cornerIcon is not null)
        {
            _cornerIcon.BasicTooltipText = $"{_tooltip}\n\nProfile: {name}";
        }
    }

    public void Dispose()
    {
        _cornerIconIsVisibleSetting.SettingChanged -= OnCornerIconIsVisibleSettingChanged;
        Service.Settings.CornerIconPriority.SettingChanged -= CornerIconPriority_SettingChanged;
        RemoveCornerIcon();
    }

    private void CreateCornerIcon()
    {
        RemoveCornerIcon();
        _cornerIcon = new CornerIcon
        {
            Icon = _cornerIconTexture,
            HoverIcon = _cornerIconHoverTexture,
            BasicTooltipText = $"{_tooltip}\n\nAccount: {Service.CurrentAccountName}",
            Parent = GameService.Graphics.SpriteScreen,
            Priority = (int)(Int32.MaxValue * ((1000.0f - Service.Settings.CornerIconPriority.Value) /1000.0f)) -1
        };
        
        _cornerIcon.Click += OnCornerIconClicked;
        _cornerIcon.Menu = new ContextMenuStrip(() => _contextMenuItems);
    }

    private void RemoveCornerIcon()
    {
        if (_cornerIcon is not null)
        {
            _cornerIcon.Click -= OnCornerIconClicked;
            _cornerIcon.Dispose();
        }
    }
    private void CornerIconPriority_SettingChanged(object sender, ValueChangedEventArgs<int> e)
    {
        if (Service.Settings.GlobalCornerIconEnabled.Value)
        {
            _cornerIcon.Priority = (int)(Int32.MaxValue * ((1000.0f - e.NewValue) / 1000.0f))-1;
        }
        
    }

    private void OnCornerIconIsVisibleSettingChanged(object sender, ValueChangedEventArgs<bool> e)
    {
        if (e.NewValue)
            CreateCornerIcon();
        else
            RemoveCornerIcon();
    }
    private void OnCornerIconClicked(object sender, MouseEventArgs e)
    {
        IconLeftClicked?.Invoke(this,true);
    }
    private readonly IEnumerable<ContextMenuStripItem> _contextMenuItems;
    private readonly Texture2D _cornerIconTexture;
    private readonly Texture2D _cornerIconHoverTexture;
    private readonly SettingEntry<bool> _cornerIconIsVisibleSetting;
    private readonly string _tooltip;
    private CornerIcon _cornerIcon;
}