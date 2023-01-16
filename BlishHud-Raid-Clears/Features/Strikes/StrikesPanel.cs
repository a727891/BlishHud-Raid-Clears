
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using RaidClears.Settings;
using RaidClears.Localization;
using RaidClears.Utils;
using RaidClears.Features.Shared.Services;
using RaidClears.Features.Raids.Models;
using RaidClears.Fearures.Raids.Services;
using RaidClears.Features.Shared.Controls;
using System.Threading.Tasks;

namespace RaidClears.Features.Strikes
{

    public static class StrikesPanelFactory
    {
        public static StrikesPanel Create()
        {
            SettingService _settings = Module.ModuleInstance.SettingsService;
            StrikesPanel panel = new StrikesPanel(
                _settings.StrikePanelLocationPoint,
                _settings.StrikePanelIsVisible,
                _settings.StrikePanelDragWithMouseIsEnabled,
                _settings.StrikePanelAllowTooltips
            );

            panel.LayoutChange(_settings.StrikePanelLayout);
            panel.BackgroundColorChange(_settings.StrikePanelBgOpacity, _settings.StrikePanelColorBG);

            panel.RegisterCornerIconService(
                new CornerIconService(
                    _settings.StrikeCornerIconEnabled,
                    _settings.StrikePanelIsVisible, 
                    Strings.CornerIcon_Raid, 
                    Module.ModuleInstance.TexturesService.CornerIconTexture,
                    Module.ModuleInstance.TexturesService.CornerIconHoverTexture
                )
            );
            panel.RegisterKeybindService(
                new KeybindHandlerService(
                    _settings.StrikePanelIsVisibleKeyBind,
                    _settings.StrikePanelIsVisible
                )
            );

            return panel;
        }
        
    }

    public class StrikesPanel : GridPanel
    {
        private readonly Strike[] Strikes;
        //private readonly GetCurrentClearsService CurrentClearsService;
        
        public StrikesPanel(
            SettingEntry<Point> locationSetting, 
            SettingEntry<bool> visibleSetting,
            SettingEntry<bool> allowMouseDragSetting, 
            SettingEntry<bool> allowTooltipSetting
        ) : base(locationSetting, visibleSetting, allowMouseDragSetting, allowTooltipSetting)
        {

            //CurrentClearsService = new GetCurrentClearsService();
            //BackgroundColor = Color.Orange;
            //WeeklyWings weeklyWings = WingRotationService.GetWeeklyWings();

            //Wings =  WingFactory.Create(this, weeklyWings);
            Strikes = StrikeFactory.Create(this);

            Module.ModuleInstance.ApiPollingService.ApiPollingTrigger += (s, e) =>
            {
                Task.Run(async () =>
                {
                    /*var weeklyClears = await CurrentClearsService.GetClearsFromApi();

                    foreach (var wing in Wings)
                    {
                        foreach (var encounter in wing.boxes)
                        {
                            encounter.SetCleared(weeklyClears.Contains(encounter.id));
                        }
                    }*/
                    Invalidate();
                });
            };
        }

    }
}
