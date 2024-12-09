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

public class RaidLabelCustomizationView : View
{
    public RaidLabelCustomizationView()
    {
  
    }

    protected override void Build(Container buildPanel)
    {
        base.Build(buildPanel);

        var panel = new FlowPanel()
            .BeginFlow(buildPanel);
        panel.CanScroll= true;

        Dictionary<string, DateTime> clears = new();

        
        foreach(var expansion in Service.RaidData.Expansions)
        {
            panel.AddString(expansion.Name, Color.Gold);
            foreach(var wing in expansion.Wings) {
                panel.AddControl(new EncounterLabelCustomerizer(panel, wing, Color.Gray));
                foreach(var encounter in wing.Encounters)
                {
                    var customerizer = new EncounterLabelCustomerizer(panel, encounter);
                    panel.AddControl(customerizer);
                }
                panel.AddSpace();
            }
        }
       
        panel.AddString($"Customize Raid Boss Labels");

    }

}