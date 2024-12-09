using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using System;
using RaidClears.Features.Fractals.Services;
using RaidClears.Features.Strikes.Services;
using RaidClears.Features.Raids.Models;

namespace RaidClears.Settings.Controls;

public  class EncounterLabelCustomerizer : Panel
{

    private Label title = new Label();
    private Label abbrivLabel = new Label();
    public EncounterLabelCustomerizer(FlowPanel parent, RaidEncounter encounter, Color? labelColor = null) : base()
    {
        Parent = parent;
        Width = parent.Width-10;
        Padding = new Thickness(0, 10);

        Build(encounter.Name, encounter.Abbriviation, encounter.ApiId, labelColor);
    }
    public EncounterLabelCustomerizer(FlowPanel parent, RaidWing wing, Color? labelColor = null) : base()
    {
        Parent = parent;
        Width = parent.Width - 10;
        Padding = new Thickness(0, 10);

        Build(wing.Name, wing.Abbriviation, wing.Id, labelColor);
    }

    protected void Build(string Name, string abbriv, string apiId, Color? color = null)
    {
        if (color == null)
        {
            color = Color.White;
        }
        var col1 = (this.Width-30) / 3;
        var colN = ((2 * col1) - 5) / 3;
        title = new Label()
        {
            Text = Name,
            Parent = this,
            Location = new(0, 0),
            Width = col1,
            TextColor= (Color)color
        };
        var customized = new TextBox()
        {
            Text = Service.RaidSettings.GetEncounterLabel(apiId),
            Parent = this,
            Location = new(col1 + 5, 0),
            Width = colN,
        
        };
        var defaultbtn = new StandardButton()
        {
            Text = $"Default to '{abbriv}'",
            Parent = this,
            Location = new(col1 + colN + 10, 0),
            Width = colN,
        };

        customized.TextChanged += (s, e) => { Service.RaidSettings.SetEncounterLabel(apiId, customized.Text); };
        defaultbtn.Click += (s, e) => {
            customized.Text = abbriv;
            Service.RaidSettings.SetEncounterLabel(apiId, abbriv); 
        };
    }

}
