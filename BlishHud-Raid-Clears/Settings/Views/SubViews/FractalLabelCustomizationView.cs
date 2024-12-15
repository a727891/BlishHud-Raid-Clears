using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using RaidClears.Features.Shared.Enums;
using RaidClears.Features.Shared.Enums.Extensions;
using RaidClears.Localization;
using RaidClears.Settings.Controls;
using RaidClears.Settings.Models;
using RaidClears.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RaidClears.Settings.Views.SubViews;

public class FractalLabelCustomizationView : View
{
    public FractalLabelCustomizationView()
    {
  
    }

    protected override void Build(Container buildPanel)
    {
        base.Build(buildPanel);

        var panel = new FlowPanel()
            .BeginFlow(buildPanel);
        panel.CanScroll= true;

        Dictionary<string, DateTime> clears = new();

        foreach(var category in Service.FractalMapData.Categories)
        {
            panel.AddControl(
                new EncounterLabelCustomerizer(
                    panel, Service.FractalPersistance, 
                    category, 
                    Color.Gold
                )
            );

        }
        panel.AddSpace();

        foreach (var map in Service.FractalMapData.Maps)
        {
            panel.AddControl(new EncounterLabelCustomerizer(panel, Service.FractalPersistance, map.Value.ToEncounterInterface() , Color.White));
        }
       
        panel.AddString($"Customize Fractal Labels");

    }

}