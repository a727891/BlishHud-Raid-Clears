using Blish_HUD.Controls;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using RaidClears.Raids.Controls;
using RaidClears.Settings;

using RaidClears.Localization;
using RaidClears.Utils;
using RaidClears.Features.Shared.Controls;
using RaidClears.Settings.Models;
using RaidClears.Features.Shared.Services;

namespace RaidClears.Features.Raids
{

    public static class RaidPanelFactory
    {
        public static RaidPanel Create()
        {
            SettingService _settings = Module.ModuleInstance.SettingsService;
            RaidPanel panel = new RaidPanel(
                _settings.RaidPanelLocationPoint,
                _settings.RaidPanelIsVisible,
                _settings.RaidPanelDragWithMouseIsEnabled,
                _settings.RaidPanelAllowTooltips
            );

            panel.LayoutChange(_settings.RaidPanelLayout);

            panel.RegisterCornerIconService(
                new CornerIconService(
                    _settings.RaidCornerIconEnabled,
                    _settings.RaidPanelIsVisible, 
                    Strings.CornerIcon_Raid, 
                    Module.ModuleInstance.TexturesService.CornerIconTexture,
                    Module.ModuleInstance.TexturesService.CornerIconHoverTexture
                )
            );
            panel.RegisterKeybindService(
                new KeybindHandlerService(
                    _settings.RaidPanelIsVisibleKeyBind,
                    _settings.RaidPanelIsVisible
                )
            );

            return panel;
        }
        
    }

    public class RaidPanel : GridPanel
    {
        public RaidPanel(
            SettingEntry<Point> locationSetting, 
            SettingEntry<bool> visibleSetting,
            SettingEntry<bool> allowMouseDragSetting, 
            SettingEntry<bool> allowTooltipSetting
        ) : base(locationSetting, visibleSetting, allowMouseDragSetting, allowTooltipSetting)
        {

            //BackgroundColor = Color.Orange;


            var settings = Module.ModuleInstance.SettingsService;

            var wing1 = new GridGroup(this, settings.RaidPanelLayout);

            var l0 = new GridBox(
                wing1,
                "test",
                "tooltip",
                settings.RaidPanelLabelOpacity,
                settings.RaidPanelLayout,
                settings.RaidPanelFontSize
                );
            l0.LabelDisplayChange(settings.RaidPanelLabelDisplay, "T", "test");
            var l1 = new GridBox(
                wing1,
                "xera",
                "tooltip",
                settings.RaidPanelGridOpacity,
                settings.RaidPanelLayout,
                settings.RaidPanelFontSize
                );
            l1.TextColorSetting(settings.RaidPanelColorText);

            var l2 = new GridBox(
                wing1,
                "mat",
                "tooltip",
                settings.RaidPanelGridOpacity,
                settings.RaidPanelLayout,
                settings.RaidPanelFontSize
                );
            l2.ConditionalTextColorSetting(settings.RaidPanelHighlightCotM, settings.RaidPanelColorCotm, settings.RaidPanelColorText);

            var wing2 = new GridGroup(this, settings.RaidPanelLayout);

            var l3 = new GridBox(
                wing2,
                "test",
                "tooltip",
                settings.RaidPanelLabelOpacity,
                settings.RaidPanelLayout,
                settings.RaidPanelFontSize
                );
            l3.LabelDisplayChange(settings.RaidPanelLabelDisplay, "T", "test");
            l3.ConditionalTextColorSetting(settings.RaidPanelHighlightEmbolden, settings.RaidPanelColorEmbolden, settings.RaidPanelColorText);
            new GridBox(
                wing2,
                "tc",
                "tooltip",
                settings.RaidPanelGridOpacity,
                settings.RaidPanelLayout,
                settings.RaidPanelFontSize
                );
            new GridBox(
                wing2,
                "ca",
                "tooltip",
                settings.RaidPanelGridOpacity,
                settings.RaidPanelLayout,
                settings.RaidPanelFontSize
                );
        }
    }
}
