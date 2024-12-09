using RaidClears.Utils;
using RaidClears.Features.Shared.Controls;
using RaidClears.Features.Shared.Services;
using RaidClears.Features.Raids.Models;
using System.Threading.Tasks;
using RaidClears.Features.Raids.Services;
using RaidClears.Settings.Models;
using Blish_HUD;
using Blish_HUD.Controls;
using System.Collections.Generic;

namespace RaidClears.Features.Raids;

public class RaidPanel : GridPanel
{
    private readonly IEnumerable<Wing> Wings = new List<Wing>();
    private static RaidSettings Settings => Service.Settings.RaidSettings;
    public RaidPanel(
    ) : base(Settings.Generic, GameService.Graphics.SpriteScreen)
    {
        Wings = WingFactory.Create(this);

        Service.ApiPollingService!.ApiPollingTrigger += (_, _) =>
        {
            Task.Run(async () =>
            {
                var weeklyClears = await GetCurrentClearsService.GetClearsFromApi();

                foreach (var wing in Wings)
                {
                    foreach (var encounter in wing.boxes)
                    {
                        encounter.SetCleared(weeklyClears.Contains(encounter.id));
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

    public void UpdateEncounterLabel(string encounterApiId, string newLabel) {
        foreach (var wing in Wings)
        {
            if (wing.id == encounterApiId)
            {
                wing.GroupLabel.Text = newLabel;
                return;
            }
            foreach (var encounter in wing.boxes)
            {
                if(encounter.id == encounterApiId)
                {
                    encounter.SetLabel(newLabel);
                    return;
                }
                
            }
        }
    }

}
