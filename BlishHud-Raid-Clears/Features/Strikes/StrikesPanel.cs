using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using RaidClears.Localization;
using RaidClears.Utils;
using RaidClears.Features.Shared.Services;
using RaidClears.Features.Shared.Controls;
using System.Threading.Tasks;
using RaidClears.Features.Strikes.Models;
using RaidClears.Settings.Models;
using Blish_HUD;
using Blish_HUD.Controls;
using System.Runtime;

namespace RaidClears.Features.Strikes;

public class StrikesPanel : GridPanel
{
    private static StrikeSettings Settings => Service.Settings.StrikeSettings;

    private readonly Strike[] _strikes;
    //private readonly GetCurrentClearsService CurrentClearsService;
    
    public StrikesPanel() : base(Settings.Generic, GameService.Graphics.SpriteScreen)
    {

        //CurrentClearsService = new GetCurrentClearsService();
       
        _strikes = StrikeMetaData.Create(this);

        /*Service.ApiPollingService.ApiPollingTrigger += (_, _) =>
        {
            Task.Run(() =>
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
                return Task.CompletedTask;
            });
        };*/
        (this as FlowPanel).LayoutChange(Settings.Style.Layout);
        (this as GridPanel).BackgroundColorChange(Settings.Style.BgOpacity, Settings.Style.Color.Background);

        RegisterCornerIconService(
            new CornerIconService(
                Settings.Generic.ToolbarIcon,
                Settings.Generic.Visible,
                Strings.CornerIcon_Strike,
                Service.TexturesService!.StrikesCornerIconTexture,
                Service.TexturesService!.StrikesCornerIconHoverTexture
            )
        );
        RegisterKeyBindService(
            new KeyBindHandlerService(
                Settings.Generic.ShowHideKeyBind,
                Settings.Generic.Visible
            )
        );
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
