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

public class StrikeLabelCustomizationView : View
{
    public StrikeLabelCustomizationView()
    {
  
    }

    protected override void Build(Container buildPanel)
    {
        base.Build(buildPanel);

        var panel = new FlowPanel()
            .BeginFlow(buildPanel);
        panel.CanScroll= true;

        Dictionary<string, DateTime> clears = new();

        
        foreach(var expansion in Service.StrikeData.Expansions)
        {
            panel.AddControl(new EncounterLabelCustomerizer(panel, Service.StrikeSettings, expansion, Color.Gold));
            foreach(var mission in expansion.Missions)
            {
                var customerizer = new EncounterLabelCustomerizer(panel, Service.StrikeSettings, mission);
                panel.AddControl(customerizer);
            }
            panel.AddSpace();
            
        }
       
        panel.AddString($"Customize Strike Mission Labels");

    }

}