using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using RaidClears.Localization;
using RaidClears.Utils;
using RaidClears.Features.Shared.Services;
using RaidClears.Features.Shared.Controls;
using System.Threading.Tasks;
using RaidClears.Features.Strikes.Models;

namespace RaidClears.Features.Strikes;

public static class StrikesPanelFactory
{
    public static StrikesPanel Create()
    {
        var settings = Module.moduleInstance.SettingsService.StrikeSettings;
        var panel = new StrikesPanel(
            settings.Generic.Location,
            settings.Generic.Visible,
            settings.Generic.PositionLock,
            settings.Generic.Tooltips
        );

        panel.LayoutChange(settings.Style.Layout);
        panel.BackgroundColorChange(settings.Style.BgOpacity, settings.Style.Color.Background);

        panel.RegisterCornerIconService(
            new CornerIconService(
                settings.Generic.ToolbarIcon,
                settings.Generic.Visible, 
                Strings.CornerIcon_Strike, 
                Module.moduleInstance.TexturesService.StrikesCornerIconTexture,
                Module.moduleInstance.TexturesService.StrikesCornerIconHoverTexture
            )
        );
        panel.RegisterKeyBindService(
            new KeyBindHandlerService(
                settings.Generic.ShowHideKeyBind,
                settings.Generic.Visible
            )
        );

        return panel;
    }
}

public class StrikesPanel : GridPanel
{
    private readonly Strike[] _strikes;
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
        _strikes = StrikeFactory.Create(this);

        Module.moduleInstance.ApiPollingService.ApiPollingTrigger += (_, _) =>
        {
            Task.Run(() =>
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
                return Task.CompletedTask;
            });
        };
    }

    public void ForceInvalidate()
    {
        foreach(var strike in _strikes)
        {
            if(strike.boxes.Length> 0)
            {
                strike.boxes[0].Box?.Parent.Invalidate();
            }
        }
    }
}
