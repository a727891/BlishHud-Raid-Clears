using Blish_HUD.Controls;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using RaidClears.Raids.Controls;
using RaidClears.Settings;
using RaidClears.Localization;
using RaidClears.Utils;
using RaidClears.Features.Shared.Services;
using RaidClears.Raids.Services;
using RaidClears.Features.Raids.Models;
using System.Threading.Tasks;

using RaidClears.Fearures.Raids.Services;
using RaidClears.Features.Dungeons.Models;
using RaidClears.Fearures.Dungeons.Services;

namespace RaidClears.Features.Dungeons
{

    public static class DungeonPanelFactory
    {
        public static DungeonPanel Create()
        {
            SettingService _settings = Module.ModuleInstance.SettingsService;
            DungeonPanel panel = new DungeonPanel(
                _settings.DungeonPanelLocationPoint,
                _settings.DungeonPanelIsVisible,
                _settings.DungeonPanelDragWithMouseIsEnabled,
                _settings.DungeonPanelAllowTooltips
            );

            panel.LayoutChange(_settings.DungeonPanelLayout);
            panel.BackgroundColorChange(_settings.DungeonPanelBgOpacity, _settings.DungeonPanelColorBG);

            panel.RegisterCornerIconService(
                new CornerIconService(
                    _settings.DungeonCornerIconEnabled,
                    _settings.DungeonPanelIsVisible, 
                    Strings.CornerIcon_Raid, 
                    Module.ModuleInstance.TexturesService.CornerIconTexture,
                    Module.ModuleInstance.TexturesService.CornerIconHoverTexture
                )
            );
            panel.RegisterKeybindService(
                new KeybindHandlerService(
                    _settings.DungeonPanelIsVisibleKeyBind,
                    _settings.DungeonPanelIsVisible
                )
            );

            return panel;
        }
        
    }

    public class DungeonPanel : GridPanel
    {
        private readonly Dungeon[] Dungeons;
        private readonly DungeonsClearsService DungeonClearsService;
        
        public DungeonPanel(
            SettingEntry<Point> locationSetting, 
            SettingEntry<bool> visibleSetting,
            SettingEntry<bool> allowMouseDragSetting, 
            SettingEntry<bool> allowTooltipSetting
        ) : base(locationSetting, visibleSetting, allowMouseDragSetting, allowTooltipSetting)
        {

            DungeonClearsService = new DungeonsClearsService();
            //BackgroundColor = Color.Orange
            WeeklyWings weeklyWings = WingRotationService.GetWeeklyWings();
           
            Dungeons =  DungeonFactory.Create(this);

            Module.ModuleInstance.ApiPollingService.ApiPollingTrigger += (s, e) =>
            {
                Task.Run(async () =>
                {
                    var weeklyClears = await DungeonClearsService.GetClearsFromApi();
                    var freqPaths = await DungeonClearsService.GetFrequenterPaths();

                    foreach (var dungeon in Dungeons)
                    {
                        foreach (var encounter in dungeon.boxes as Path[])
                        {
                            encounter.SetCleared(weeklyClears.Contains(encounter.id));
                            encounter.SetFrequenter(freqPaths.Contains(encounter.id));
                            
                        }
                    }
                   
                    Invalidate();
                });
            };
        }

    }
}
