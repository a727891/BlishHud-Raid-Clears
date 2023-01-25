using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using RaidClears.Localization;
using RaidClears.Utils;
using RaidClears.Features.Shared.Services;
using System.Threading.Tasks;
using RaidClears.Features.Dungeons.Models;
using System.Linq;
using RaidClears.Features.Shared.Controls;
using RaidClears.Features.Dungeons.Services;
using RaidClears.Features.Raids.Services;
using RaidClears.Settings.Models;

namespace RaidClears.Features.Dungeons;

/*public static class DungeonPanelFactory
{
    private static DungeonSettings Settings => Service.Settings.DungeonSettings;
    
    public static DungeonPanel Create()
    {
        var panel = new DungeonPanel(
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
                Strings.CornerIcon_Dungeon, 
                Service.TexturesService.DungeonsCornerIconTexture,
                Service.TexturesService.DungeonsCornerIconHoverTexture
            )
        );
        panel.RegisterKeyBindService(
            new KeyBindHandlerService(
                Settings.Generic.ShowHideKeyBind,
                Settings.Generic.Visible
            )
        );

        return panel;
    }
}*/

/*public class DungeonPanel : GridPanel
{
    public DungeonPanel(
        SettingEntry<Point> locationSetting, 
        SettingEntry<bool> visibleSetting,
        SettingEntry<bool> allowMouseDragSetting, 
        SettingEntry<bool> allowTooltipSetting
    ) : base(locationSetting, visibleSetting, allowMouseDragSetting, allowTooltipSetting)
    {

        var dungeonClearsService = new DungeonsClearsService();
        //BackgroundColor = Color.Orange
        WingRotationService.GetWeeklyWings();
       
        var dungeons = DungeonFactory.Create(this);

        Service.ApiPollingService.ApiPollingTrigger += (_, _) =>
        {
            Task.Run(async () =>
            {
                var weeklyClears = await dungeonClearsService.GetClearsFromApi();
                var freqPaths = await dungeonClearsService.GetFrequenterPaths();

                foreach (var dungeon in dungeons)
                {
                    
                    foreach (var encounter in dungeon.boxes.OfType<Path>())
                    {
                        encounter.SetCleared(weeklyClears.Contains(encounter.id));
                        encounter.SetFrequenter(freqPaths.Contains(encounter.id));
                        if (dungeon.index == DungeonFactory.FrequenterIndex && encounter.id.Equals(DungeonFactory.FrequenterID))
                        {
                            encounter.SetFrequenter(true);
                            encounter.Box.Text = $"{freqPaths.Count()}/8";
                            encounter.ApplyTextColor();
                        }
                        
                    }
                }
               
                Invalidate();
            });
        };
    }
}*/
