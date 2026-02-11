using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using RaidClears.Localization;
using RaidClears.Settings.Controls;
using RaidClears.Utils;
using System;
using System.Collections.Generic;

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

        // Daily Raid Bounties and Tomorrow's Raid Bounties category labels
        if (Service.DailyBountyData != null && Service.DailyBountyData.Enabled)
        {
            panel.AddControl(new EncounterLabelCustomerizer(panel, Service.StrikeSettings, Service.StrikeData.Priority, Color.Gold));
            panel.AddControl(new EncounterLabelCustomerizer(panel, Service.StrikeSettings, Service.StrikeData.PriorityTomorrow, Color.Gold));
            panel.AddSpace();
        }

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
       
        panel.AddString(Strings.StrikeLabelCustomization_Heading);

    }

}