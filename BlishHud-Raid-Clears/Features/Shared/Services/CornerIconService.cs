using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework.Graphics;

namespace RaidClears.Features.Shared.Services;

public class CornerIconToggleMenuItem: ContextMenuStripItem
{ 
    public CornerIconToggleMenuItem(SettingEntry<bool> setting, string displayLabel) : base(displayLabel)
    {
        var baseText = displayLabel;
        Text = (setting.Value ? "Hide " : "Show ") + baseText;

        Click += delegate { setting.Value = !setting.Value; };
        setting.SettingChanged += delegate
        {
            Text = (setting.Value ? "Hide " : "Show ") + baseText;
        };
    }

    public CornerIconToggleMenuItem(Control control, string displayLabel) : base(displayLabel)
    {
        Click += delegate { control.Show(); };
    }
}
public class CornerIconService : IDisposable
{
    public CornerIconService(SettingEntry<bool> cornerIconIsVisibleSetting,
                             string tooltip,
                             Texture2D defaultTexture,
                             Texture2D hoverTexture,
                             IEnumerable<CornerIconToggleMenuItem> contextMenuItems
                            // Func<object,MouseEventArgs> leftClickDelegate
    )
    {
        _tooltip                     = tooltip;
        _cornerIconIsVisibleSetting  = cornerIconIsVisibleSetting;
        _cornerIconTexture           = defaultTexture;
        _cornerIconHoverTexture      = hoverTexture;
        _contextMenuItems = contextMenuItems;
        //_leftClickDelegate = leftClickDelegate;
        cornerIconIsVisibleSetting.SettingChanged += OnCornerIconIsVisibleSettingChanged;

        if (cornerIconIsVisibleSetting.Value)
            CreateCornerIcon();
    }

    public void Dispose()
    {
        _cornerIconIsVisibleSetting.SettingChanged -= OnCornerIconIsVisibleSettingChanged;

        RemoveCornerIcon();
    }

    private void CreateCornerIcon()
    {
        RemoveCornerIcon();

        _cornerIcon = new CornerIcon
        {
            Icon             = _cornerIconTexture,
            HoverIcon        = _cornerIconHoverTexture,
            BasicTooltipText = _tooltip,
            Parent           = GameService.Graphics.SpriteScreen,
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

    private void OnCornerIconIsVisibleSettingChanged(object sender, ValueChangedEventArgs<bool> e)
    {
        if (e.NewValue)
            CreateCornerIcon();
        else
            RemoveCornerIcon();
    }
    private void OnCornerIconClicked(object sender, MouseEventArgs e)
    {
        //_leftClickDelegate(sender, e);
    }
    //private Func<object, MouseEventArgs> _leftClickDelegate;
    private readonly IEnumerable<CornerIconToggleMenuItem> _contextMenuItems;
    private readonly Texture2D _cornerIconTexture;
    private readonly Texture2D _cornerIconHoverTexture;
    private readonly SettingEntry<bool> _cornerIconIsVisibleSetting;
    private readonly string _tooltip;
    private CornerIcon _cornerIcon;
}