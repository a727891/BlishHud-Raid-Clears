using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Settings;
using RaidClears.Features.Raids.Models;
using RaidClears.Features.Shared.Controls;
using RaidClears.Features.Shared.Enums;
using RaidClears.Features.Shared.Models;
using RaidClears.Features.Fractals.Services;
using RaidClears.Settings.Models;
using RaidClears.Utils;

namespace RaidClears.Features.Fractals.Models;

public class CMInterface
{
    public FractalMap Map { get; set; }
    public int Scale { get; set; }
    public int DayOfyear { get; set; }
    public CMInterface(FractalMap map, int scale, int dayOfYear)
    {
        Map= map;
        Scale= scale;
        DayOfyear= dayOfYear;
    }
}