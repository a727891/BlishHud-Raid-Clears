using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using RaidClears.Localization;
using RaidClears.Utils;
using RaidClears.Features.Shared.Controls;
using RaidClears.Features.Shared.Services;
using RaidClears.Features.Raids.Models;
using System.Threading.Tasks;
using RaidClears.Features.Raids.Services;
using RaidClears.Settings.Models;

namespace RaidClears.Features.Raids;

public static class RaidPanelFactory
{
    private static RaidSettings Settings => Module.moduleInstance.SettingsService.RaidSettings;
    
    public static RaidPanel Create()
    {
        var panel = new RaidPanel(
            Settings.Generic.Location,
            Settings.Generic.Visible,
            Settings.Generic.PositionLock,
            Settings.Generic.Tooltips
        );

        panel.LayoutChange(Settings.Style.Layout);
        panel.BackgroundColorChange(Settings.Style.BgOpacity, Settings.Style.Color.Background);

        panel.RegisterCornerIconService(
            new CornerIconService(
                Settings.Generic.ToolbarIcon,
                Settings.Generic.Visible, 
                Strings.CornerIcon_Raid, 
                Module.moduleInstance.TexturesService.CornerIconTexture,
                Module.moduleInstance.TexturesService.CornerIconHoverTexture
            )
        );
        panel.RegisterKeyBindService(
            new KeybindHandlerService(
                Settings.Generic.ShowHideKeyBind,
                Settings.Generic.Visible
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
        var weeklyWings = WingRotationService.GetWeeklyWings();
       
        var wings = WingFactory.Create(this, weeklyWings);

        Module.moduleInstance.ApiPollingService.ApiPollingTrigger += (_, _) =>
        {
            Task.Run(async () =>
            {
                var weeklyClears = await GetCurrentClearsService.GetClearsFromApi();

                foreach (var wing in wings)
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

}
