using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using RaidClears.Features.Shared.Enums;
using RaidClears.Features.Shared.Enums.Extensions;
using RaidClears.Localization;
using RaidClears.Shared.Controls;
using System;
using System.Threading.Tasks;

namespace RaidClears.Features.Strikes.Services;

public class ResetsWatcherService : IDisposable
{

    public event EventHandler<DateTime>? DailyReset;

    protected DateTime NextDailyReset;

    protected FlowPanel panel = new FlowPanel()
    {
        Parent = GameService.Graphics.SpriteScreen,
        Location = new(800, 0),
        Size = new Point(300, 300),
        ShowBorder = true,
        ShowTint = true,
        FlowDirection = ControlFlowDirection.SingleTopToBottom,
        CanScroll= true,

    };

    public ResetsWatcherService()
    {
        NextDailyReset = GetNextDailyReset();
        new Label()
        {
            Width = 300,
            Parent = panel,
            Text = $"Reset calulated as {NextDailyReset.ToString()}"
        };
    }

    public DateTime GetNextDailyReset()
    {
        var now = DateTime.UtcNow;

        return now.AddSeconds(10);
        //return now.AddDays(1).Date;
    }

    public void Update(GameTime gametime)
    {
        if( DateTime.UtcNow >= NextDailyReset )
        {
            DailyReset?.Invoke(this, NextDailyReset);
            NextDailyReset = GetNextDailyReset();
            new Label()
            {
                Width = 300,
                Parent = panel,
                Text = $"Reset calulated as {NextDailyReset.ToString()}"
            };
        }
    }

    public void Dispose()
    {

    }
}