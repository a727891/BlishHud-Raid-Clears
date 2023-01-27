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
using RaidClears.Features.Strikes.Services;

namespace RaidClears.Features.Strikes;

public class StrikesPanel : GridPanel
{
    private static StrikeSettings Settings => Service.Settings.StrikeSettings;

    private readonly Strike[] _strikes;
    private readonly MapWatcherService _mapService;
    
    public StrikesPanel() : base(Settings.Generic, GameService.Graphics.SpriteScreen)
    {

        _mapService = new MapWatcherService();
        _strikes = StrikeMetaData.Create(this);

        _mapService.LeftStrikeMapWithCombatStartAndEnd += _mapService_LeftStrikeMapWithCombatStartAndEnd;

        (this as FlowPanel).LayoutChange(Settings.Style.Layout);
        (this as GridPanel).BackgroundColorChange(Settings.Style.BgOpacity, Settings.Style.Color.Background);

        /*RegisterCornerIconService(
            new CornerIconService(
                Settings.Generic.ToolbarIcon,
                Settings.Generic.Visible,
                Strings.CornerIcon_Strike,
                Service.TexturesService!.StrikesCornerIconTexture,
                Service.TexturesService!.StrikesCornerIconHoverTexture
            )
        );*/
        RegisterKeyBindService(
            new KeyBindHandlerService(
                Settings.Generic.ShowHideKeyBind,
                Settings.Generic.Visible
            )
        );
    }

    private void _mapService_LeftStrikeMapWithCombatStartAndEnd(object sender, string encounterId)
    {
        foreach (var group in _strikes)
        {
            foreach (var encounter in group.boxes)
            {
                if(encounter.id == encounterId)
                {
                    encounter.SetCleared(true);
                }
            }
        }
        Invalidate();
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

    protected override void DisposeControl()
    {
        base.DisposeControl();
        _mapService.LeftStrikeMapWithCombatStartAndEnd -= _mapService_LeftStrikeMapWithCombatStartAndEnd;
        _mapService.Dispose();
    }
}
