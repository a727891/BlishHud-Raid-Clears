using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using RaidClears.Localization;
using RaidClears.Utils;
using RaidClears.Features.Shared.Controls;
using RaidClears.Features.Shared.Services;
using RaidClears.Features.Raids.Models;
using System.Threading.Tasks;
using RaidClears.Settings.Services;
using RaidClears.Features.Raids.Services;

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
            panel.BackgroundColorChange(_settings.RaidPanelBgOpacity, _settings.RaidPanelColorBG);

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
        private readonly Wing[] Wings;
        private readonly GetCurrentClearsService CurrentClearsService;
        
        public RaidPanel(
            SettingEntry<Point> locationSetting, 
            SettingEntry<bool> visibleSetting,
            SettingEntry<bool> allowMouseDragSetting, 
            SettingEntry<bool> allowTooltipSetting
        ) : base(locationSetting, visibleSetting, allowMouseDragSetting, allowTooltipSetting)
        {

            CurrentClearsService = new GetCurrentClearsService();
            //BackgroundColor = Color.Orange;
            WeeklyWings weeklyWings = WingRotationService.GetWeeklyWings();
           
            Wings =  WingFactory.Create(this, weeklyWings);

            Module.ModuleInstance.ApiPollingService.ApiPollingTrigger += (s, e) =>
            {
                Task.Run(async () =>
                {
                    var weeklyClears = await CurrentClearsService.GetClearsFromApi();

                    foreach (var wing in Wings)
                    {
                        foreach (var encounter in wing.boxes)
                        {
                            encounter.SetCleared(weeklyClears.Contains(encounter.id));
                        }
                    }
                    Invalidate();
                });
            };
        }

        public void AddStrikes()
        {

        }

        public void RemoveStrikes()
        {

        }

    }
}
