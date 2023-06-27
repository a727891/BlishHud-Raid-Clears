using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using RaidClears.Features.Shared.Enums;
using RaidClears.Features.Shared.Enums.Extensions;
using RaidClears.Localization;
using RaidClears.Settings.Models;
using RaidClears.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RaidClears.Settings.Views.SubViews;

public class StrikeClearCorrectionView : View
{
    public StrikeClearCorrectionView()
    {
  
    }

    protected override void Build(Container buildPanel)
    {
        base.Build(buildPanel);

        var panel = new FlowPanel()
            .BeginFlow(buildPanel);

        Dictionary<Encounters.StrikeMission, DateTime> clears = new();

        if (!Service.StrikePersistance.AccountClears.TryGetValue(Service.CurrentAccountName, out clears))
        {
            clears = new();
        }

        foreach (KeyValuePair<Encounters.StrikeMission, DateTime> entry in clears.OrderBy(p => p.Key))
        {

            panel.AddEncounterClearStatus(entry.Key, entry.Value);


        }
        panel.AddString("Last Strike Mission Clears");

    }

}