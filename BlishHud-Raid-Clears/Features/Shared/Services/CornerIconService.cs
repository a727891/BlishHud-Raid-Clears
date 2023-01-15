using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework.Graphics;

namespace RaidClears.Features.Shared.Services
{
    public class CornerIconService : IDisposable
    {
        public CornerIconService(SettingEntry<bool> cornerIconIsVisibleSetting,
                                 SettingEntry<bool> toggleControlSetting,
                                 string tooltip,
                                 Texture2D defaultTexture,
                                 Texture2D hoverTexture)
        {
            _tooltip                     = tooltip;
            _cornerIconIsVisibleSetting  = cornerIconIsVisibleSetting;
            _toggleControlSetting        = toggleControlSetting;
            _cornerIconTexture           = defaultTexture;
            _cornerIconHoverTexture      = hoverTexture;

            cornerIconIsVisibleSetting.SettingChanged += OnCornerIconIsVisibleSettingChanged;

            if (cornerIconIsVisibleSetting.Value)
                CreateCornerIcon();
        }

        public void Dispose()
        {
            _cornerIconIsVisibleSetting.SettingChanged -= OnCornerIconIsVisibleSettingChanged;

            if (_cornerIcon != null)
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
        }

        private void RemoveCornerIcon()
        {
            if (_cornerIcon != null)
            {
                _cornerIcon.Click -= OnCornerIconClicked;
                _cornerIcon.Dispose();
                _cornerIcon = null;
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
            _toggleControlSetting.Value = !_toggleControlSetting.Value;
        }

        private readonly Texture2D _cornerIconTexture;
        private readonly Texture2D _cornerIconHoverTexture;
        private readonly SettingEntry<bool> _cornerIconIsVisibleSetting;
        private readonly SettingEntry<bool> _toggleControlSetting;
        private readonly string _tooltip;
        private CornerIcon _cornerIcon;
    }
}