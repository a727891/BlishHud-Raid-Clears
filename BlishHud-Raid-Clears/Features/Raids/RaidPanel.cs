﻿using Blish_HUD.Settings;
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

        var wings = WingFactory.Create(this, weeklyWings);

        Service.ApiPollingService!.ApiPollingTrigger += (_, _) =>
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

        (this as FlowPanel).LayoutChange(Settings.Style.Layout);
        (this as GridPanel).BackgroundColorChange(Settings.Style.BgOpacity, Settings.Style.Color.Background);
/*
        RegisterCornerIconService(
            new CornerIcon(
                Settings.Generic.ToolbarIcon,
                Settings.Generic.Visible,
                "removed string resource",
                Service.Textures!.CornerIconTexture,
                Service.Textures!.CornerIconHoverTexture
            )
        );*/
        RegisterKeyBindService(
            new KeyBindHandlerService(
                Settings.Generic.ShowHideKeyBind,
                Settings.Generic.Visible
            )
        );
    }

   /* protected void DoUpdate()
    {
        base.DoUpdate();
    }*/

}
