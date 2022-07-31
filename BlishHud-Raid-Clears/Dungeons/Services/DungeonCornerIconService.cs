using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework.Graphics;
using RaidClears.Settings;

namespace RaidClears.Dungeons.Services
{
    public class DungeonCornerIconService : IDisposable
    {
        public DungeonCornerIconService(SettingEntry<bool> cornerIconIsVisibleSetting,
                                 SettingEntry<bool> dungeonsEnabledSetting,
                                 string tooltip,
                                 EventHandler<MouseEventArgs> cornerIconClickEventHandler,
                                 TextureService textureService)
        {
            _tooltip                     = tooltip;
            _cornerIconIsVisibleSetting  = cornerIconIsVisibleSetting;
            _dungeonEnabledSetting       = dungeonsEnabledSetting;
            _cornerIconClickEventHandler = cornerIconClickEventHandler;
            _cornerIconTexture           = textureService.DungeonsCornerIconTexture;
            _cornerIconHoverTexture      = textureService.DungeonsCornerIconHoverTexture;

            _cornerIconIsVisibleSetting.SettingChanged += OnCornerIconIsVisibleSettingChanged;
            _dungeonEnabledSetting.SettingChanged += OnCornerIconIsVisibleSettingChanged;


            if (dungeonsEnabledSetting.Value && cornerIconIsVisibleSetting.Value )
                CreateCornerIcon();
        }

        public void Dispose()
        {
            _cornerIconIsVisibleSetting.SettingChanged -= OnCornerIconIsVisibleSettingChanged;
            _dungeonEnabledSetting.SettingChanged -= OnCornerIconIsVisibleSettingChanged;

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

            _cornerIcon.Click += _cornerIconClickEventHandler;
        }

        private void RemoveCornerIcon()
        {
            if (_cornerIcon != null)
            {
                _cornerIcon.Click -= _cornerIconClickEventHandler;
                _cornerIcon.Dispose();
                _cornerIcon = null;
            }
        }

        private void OnCornerIconIsVisibleSettingChanged(object sender, ValueChangedEventArgs<bool> e)
        {
            if (_dungeonEnabledSetting.Value && _cornerIconIsVisibleSetting.Value)
                CreateCornerIcon();
            else
                RemoveCornerIcon();
        }

        private readonly Texture2D _cornerIconTexture;
        private readonly Texture2D _cornerIconHoverTexture;
        private readonly SettingEntry<bool> _cornerIconIsVisibleSetting;
        private readonly SettingEntry<bool> _dungeonEnabledSetting;

        private readonly EventHandler<MouseEventArgs> _cornerIconClickEventHandler;
        private readonly string _tooltip;
        private CornerIcon _cornerIcon;
    }
}