using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using RaidClears.Features.Shared.Enums;
using RaidClears.Features.Shared.Enums.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RaidClears.Features.Fractals.Services;
using RaidClears.Features.Strikes.Services;
using RaidClears.Features.Strikes.Models;

namespace RaidClears.Settings.Controls;

public  class EncounterClearStatus : Panel
{
    private bool IsFractal = false;
    private FractalMap fractal;
    private StrikeMission mission;

    private Label title = new Label();
    private Label clearDate = new Label();
    public EncounterClearStatus(FlowPanel parent, FractalMap encounter, DateTime lastClear) : base()
    {

        IsFractal = true;
        fractal= encounter;

        Parent = parent;
        Width = parent.Width-10;

        Build(encounter.Label, lastClear);
    }
    public EncounterClearStatus(FlowPanel parent, StrikeMission encounter, DateTime lastClear) : base()
    {
        IsFractal = false;
        mission= encounter;

        Parent= parent;
        Width = parent.Width-10;
        Build(encounter.Name, lastClear);
    }

    protected void MarkClear()
    {
        if (IsFractal)
        {
            Service.FractalMapWatcher.MarkCompleted(fractal);
        }
        else
        {
            Service.MapWatcher.MarkStrikeCompleted(mission);
        }
        clearDate.Text = DateTime.UtcNow.ToShortDateString();
    }
    protected void RemoveClear()
    {
        if (IsFractal)
        {
            Service.FractalMapWatcher.MarkNotCompleted(fractal);
        }
        else
        {
            Service.MapWatcher.MarkStrikeNotCompleted(mission);
        }
        clearDate.Text = "----------";

    }

    protected void Build(string Name, DateTime datetime)
    {
        var col1 = (this.Width-30) / 3;
        var colN = ((2 * col1) - 5) / 3;
        title = new Label()
        {
            Text = Name,
            Parent = this,
            Location = new(0, 0),
            Width = col1
        };
        clearDate = new Label()
        {
            Text = datetime.Year==1 ? "----------" : datetime.ToShortDateString(),
            Parent = this,
            Location = new(col1+5, 0),
            Width = colN
        };
        var complete = new StandardButton()
        {
            Text = "Mark Complete",
            Parent = this,
            Location = new(col1+colN + 5, 0),
            Width = colN,
        
        };
        var clear = new StandardButton()
        {
            Text = "Remove Clear",
            Parent = this,
            Location = new(col1 + colN +colN + 5, 0),
            Width = colN
        };

        complete.Click += (s, e) => { MarkClear(); };
        clear.Click += (s, e) => { RemoveClear(); };
    }

}
