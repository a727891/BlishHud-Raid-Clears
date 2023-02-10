using RaidClears.Utils;
using RaidClears.Features.Shared.Services;
using System.Threading.Tasks;
using RaidClears.Features.Dungeons.Models;
using System.Linq;
using RaidClears.Features.Shared.Controls;
using RaidClears.Features.Dungeons.Services;
using RaidClears.Settings.Models;
using Blish_HUD;
using System.Collections.Generic;
using Blish_HUD.Controls;

namespace RaidClears.Features.Dungeons;

public class DungeonPanel : GridPanel
{
    private static DungeonSettings Settings => Service.Settings.DungeonSettings;

    private IEnumerable<Dungeon> _dungeons;
    public DungeonPanel() : base(Settings.Generic, GameService.Graphics.SpriteScreen)
    {
        
        var dungeonClearsService = new DungeonsClearsService();
       
        _dungeons = DungeonFactory.Create(this);

        Service.ApiPollingService!.ApiPollingTrigger += (_, _) =>
        {
            Task.Run(async () =>
            {
                var weeklyClears = await dungeonClearsService.GetClearsFromApi();
                var freqPaths = await dungeonClearsService.GetFrequenterPaths();

                foreach (var dungeon in _dungeons)
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


        (this as FlowPanel).LayoutChange(Settings.Style.Layout);
        (this as GridPanel).BackgroundColorChange(Settings.Style.BgOpacity, Settings.Style.Color.Background);
        
        RegisterKeyBindService(
            new KeyBindHandlerService(
                Settings.Generic.ShowHideKeyBind,
                Settings.Generic.Visible
            )
        );

    }
}
