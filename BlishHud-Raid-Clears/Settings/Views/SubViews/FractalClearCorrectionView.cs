using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using RaidClears.Features.Fractals.Services;
using RaidClears.Features.Shared.Enums;
using RaidClears.Features.Shared.Enums.Extensions;
using RaidClears.Localization;
using RaidClears.Settings.Models;
using RaidClears.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RaidClears.Settings.Views.SubViews;

public class FractalClearCorrectionView : View
{
    public FractalClearCorrectionView()
    {
  
    }

    protected override void Build(Container buildPanel)
    {
        base.Build(buildPanel);

        var panel = new FlowPanel()
            .BeginFlow(buildPanel);

        Dictionary<string, DateTime> clears = new();

        if (!Service.FractalPersistance.AccountClears.TryGetValue(Service.CurrentAccountName, out clears))
        {
            clears = new();
        }


        foreach (KeyValuePair<string, DateTime> entry in clears.OrderBy(p => p.Key))
        {

            //panel.AddString(entry.Key.GetLabel() + ": " + entry.Value.ToString());
            panel.AddEncounterClearStatus(Service.FractalMapData.GetFractalByApiName(entry.Key), entry.Value);


        }
        panel.AddString("Last Fractal Clears");

    }
}