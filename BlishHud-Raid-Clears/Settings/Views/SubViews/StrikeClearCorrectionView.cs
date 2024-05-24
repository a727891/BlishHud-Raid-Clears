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
        panel.CanScroll= true;

        Dictionary<string, DateTime> clears = new();

        if (!Service.StrikePersistance.AccountClears.TryGetValue(Service.CurrentAccountName, out clears))
        {
            clears = new();
        }
        foreach(var expansion in Service.StrikeData.Expansions.OrderBy(x=>x.Name))
        {
            panel.AddString(expansion.Name, Color.Gold);
            foreach(var mission in expansion.Missions.OrderBy(x => x.Name)) {
                if (clears.ContainsKey(mission.Id))
                {
                    panel.AddEncounterClearStatus(
                        mission,
                        clears[mission.Id]
                    );

                }
                else
                {
                    panel.AddEncounterClearStatus(
                       mission,
                       new()
                    );
                }
            }
        }
       
        panel.AddString($"Last Strike Mission Clears (Profile: {Service.CurrentAccountName})");

    }

}