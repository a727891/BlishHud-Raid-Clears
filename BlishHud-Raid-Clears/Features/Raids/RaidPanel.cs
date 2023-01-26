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
using Blish_HUD;
using Blish_HUD.Controls;

namespace RaidClears.Features.Raids;

public class RaidPanel : GridPanel
{
    private static RaidSettings Settings => Service.Settings.RaidSettings;
    public RaidPanel(
    ) : base(Settings.Generic, GameService.Graphics.SpriteScreen)
    {
        var weeklyWings = WingRotationService.GetWeeklyWings();

        //var wings = WingFactory.Create(this, weeklyWings);
        new GridBox(this, "L", "wings", Settings.Style.LabelOpacity, Settings.Style.FontSize);
        new GridBox(this, "R", "tooltip1", Settings.Style.GridOpacity,Settings.Style.FontSize);
        new GridBox(this, "S", "tooltip2", Settings.Style.GridOpacity,Settings.Style.FontSize);
        new GridBox(this, "T", "tooltip3", Settings.Style.GridOpacity,Settings.Style.FontSize);

        Service.ApiPollingService!.ApiPollingTrigger += (_, _) =>
        {
            Task.Run(async () =>
            {
                var weeklyClears = await GetCurrentClearsService.GetClearsFromApi();

               /* foreach (var wing in wings)
                {
                    foreach (var encounter in wing.boxes)
                    {
                        encounter.SetCleared(weeklyClears.Contains(encounter.id));
                    }
                }*/
                Invalidate();
            });
        };

        (this as FlowPanel).LayoutChange(Settings.Style.Layout);
        (this as GridPanel).BackgroundColorChange(Settings.Style.BgOpacity, Settings.Style.Color.Background);

        RegisterCornerIconService(
            new CornerIconService(
                Settings.Generic.ToolbarIcon,
                Settings.Generic.Visible,
                Strings.CornerIcon_Raid,
                Service.TexturesService!.CornerIconTexture,
                Service.TexturesService!.CornerIconHoverTexture
            )
        );
        RegisterKeyBindService(
            new KeyBindHandlerService(
                Settings.Generic.ShowHideKeyBind,
                Settings.Generic.Visible
            )
        );
    }

}
