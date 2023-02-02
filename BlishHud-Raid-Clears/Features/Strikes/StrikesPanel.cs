using RaidClears.Utils;
using RaidClears.Features.Shared.Services;
using RaidClears.Features.Shared.Controls;
using RaidClears.Features.Strikes.Models;
using RaidClears.Settings.Models;
using Blish_HUD;
using Blish_HUD.Controls;
using RaidClears.Features.Strikes.Services;

namespace RaidClears.Features.Strikes;

public class StrikesPanel : GridPanel
{
    private static StrikeSettings Settings => Service.Settings.StrikeSettings;

    private readonly Strike[] _strikes;
    private readonly MapWatcherService _mapService;
    
    public StrikesPanel() : base(Settings.Generic, GameService.Graphics.SpriteScreen)
    {

        _mapService = Service.MapWatcher;
        _strikes = StrikeMetaData.Create(this);

        _mapService.LeftStrikeMap += _mapService_LeftStrikeMapWithCombatStartAndEnd;

        (this as FlowPanel).LayoutChange(Settings.Style.Layout);
        (this as GridPanel).BackgroundColorChange(Settings.Style.BgOpacity, Settings.Style.Color.Background);

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
        _mapService.LeftStrikeMap -= _mapService_LeftStrikeMapWithCombatStartAndEnd;
        _mapService.Dispose();
    }
}
