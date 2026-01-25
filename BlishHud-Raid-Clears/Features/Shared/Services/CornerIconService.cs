using System;
using System.Collections.Generic;
using Blish_HUD;
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
                             Texture2D notificationTexture,
                             Texture2D notificationHoverTexture,
                             IEnumerable<ContextMenuStripItem> contextMenuItems
    )
    {
        _tooltip                     = tooltip;
        _cornerIconIsVisibleSetting  = cornerIconIsVisibleSetting;
        _cornerIconTexture           = defaultTexture;
        _cornerIconHoverTexture      = hoverTexture;
        _cornerIconNotificationTexture = notificationTexture;
        _cornerIconNotificationHoverTexture = notificationHoverTexture;
        _contextMenuItems = contextMenuItems;
        _hasNotification = false;
        cornerIconIsVisibleSetting.SettingChanged += OnCornerIconIsVisibleSettingChanged;
        Service.Settings.CornerIconPriority.SettingChanged += CornerIconPriority_SettingChanged;

        if (cornerIconIsVisibleSetting.Value)
            CreateCornerIcon();
    }

    public void UpdateAccountName(string name)
    {
        if(_tooltipView is not null)
        {
            _tooltipView.UpdateAccountName(name);
        }
    }

    public void SetNotificationState(bool hasNotification, string? motdMessage = null)
    {
        _hasNotification = hasNotification;
        _motdMessage = motdMessage;
        
        if (_cornerIcon is not null)
        {
            UpdateIconTextures();
            UpdateTooltip();
        }
        
        if (_tooltipView is not null)
        {
            _tooltipView.MotdMessage = motdMessage;
        }
    }

    private void UpdateIconTextures()
    {
        if (_cornerIcon is null) return;

        if (_hasNotification)
        {
            _cornerIcon.Icon = _cornerIconNotificationTexture;
            _cornerIcon.HoverIcon = _cornerIconNotificationHoverTexture;
        }
        else
        {
            _cornerIcon.Icon = _cornerIconTexture;
            _cornerIcon.HoverIcon = _cornerIconHoverTexture;
        }
    }

    private void UpdateTooltip()
    {
        if (_cornerIcon is null || _tooltipView is null) return;

        // Use custom tooltip view instead of BasicTooltipText
        _cornerIcon.BasicTooltipText = null;
        _cornerIcon.Tooltip = _tooltipView;
        
        // Update tooltip content
        _tooltipView.MotdMessage = _motdMessage;
        _tooltipView.UpdateAccountName(Service.CurrentAccountName);
    }

    public void Dispose()
    {
        _cornerIconIsVisibleSetting.SettingChanged -= OnCornerIconIsVisibleSettingChanged;
        Service.Settings.CornerIconPriority.SettingChanged -= CornerIconPriority_SettingChanged;
        RemoveCornerIcon();
        _tooltipView?.Dispose();
    }

    private void CreateCornerIcon()
    {
        RemoveCornerIcon();
        
        // Initialize tooltip view if not already created
        if (_tooltipView == null)
        {
            _tooltipView = new CornerIconTooltipView();
            _tooltipView.UpdateAccountName(Service.CurrentAccountName);
            if (!string.IsNullOrEmpty(_motdMessage))
            {
                _tooltipView.MotdMessage = _motdMessage;
            }
        }
        
        _cornerIcon = new CornerIcon
        {
            Parent = GameService.Graphics.SpriteScreen,
            Priority = (int)(Int32.MaxValue * ((1000.0f - Service.Settings.CornerIconPriority.Value) /1000.0f)) -1
        };
        
        UpdateIconTextures();
        UpdateTooltip();
        
        _cornerIcon.Click += OnCornerIconClicked;
        _cornerIcon.MouseEntered += OnCornerIconMouseEntered;
        _cornerIcon.Menu = new ContextMenuStrip(() => _contextMenuItems);
    }

    private void RemoveCornerIcon()
    {
        if (_cornerIcon is not null)
        {
            _cornerIcon.Click -= OnCornerIconClicked;
            _cornerIcon.MouseEntered -= OnCornerIconMouseEntered;
            _cornerIcon.Dispose();
        }
    }
    private void CornerIconPriority_SettingChanged(object sender, ValueChangedEventArgs<int> e)
    {
        if (Service.Settings.GlobalCornerIconEnabled.Value && _cornerIcon != null)
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

    private void OnCornerIconMouseEntered(object sender, MouseEventArgs e)
    {
        // When user hovers over the icon (tooltip is shown), hide the notification dot
        // but keep the MOTD message in the tooltip
        if (_hasNotification)
        {
            _hasNotification = false;
            UpdateIconTextures();
            // Don't call UpdateTooltip() here - keep the MOTD in the tooltip
            
            // Mark this message as seen
            if (!string.IsNullOrEmpty(_currentMotdId) && Service.Settings != null)
            {
                Service.Settings.LastShownMotdId.Value = _currentMotdId;
            }
        }
    }

    public void SetCurrentMotdId(string? motdId)
    {
        _currentMotdId = motdId;
    }

    private readonly IEnumerable<ContextMenuStripItem> _contextMenuItems;
    private readonly Texture2D _cornerIconTexture;
    private readonly Texture2D _cornerIconHoverTexture;
    private readonly Texture2D _cornerIconNotificationTexture;
    private readonly Texture2D _cornerIconNotificationHoverTexture;
    private readonly SettingEntry<bool> _cornerIconIsVisibleSetting;
    private readonly string _tooltip;
    private CornerIcon? _cornerIcon;
    private CornerIconTooltipView? _tooltipView;
    private bool _hasNotification;
    private string? _motdMessage;
    private string? _currentMotdId;
}