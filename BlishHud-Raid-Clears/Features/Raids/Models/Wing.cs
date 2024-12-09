using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Settings;
using RaidClears.Features.Raids.Services;
using RaidClears.Features.Shared.Controls;
using RaidClears.Features.Shared.Enums;
using RaidClears.Features.Shared.Models;
using RaidClears.Features.Strikes.Models;
using RaidClears.Localization;
using RaidClears.Settings.Models;
using RaidClears.Utils;

namespace RaidClears.Features.Raids.Models;

public class Wing : GroupModel
{ 
    public Wing(string name, string id, int index, string shortName, IEnumerable<BoxModel> boxes) : base(name, id, index, Service.RaidSettings.GetEncounterLabel(id), boxes)
    {
    }
}
