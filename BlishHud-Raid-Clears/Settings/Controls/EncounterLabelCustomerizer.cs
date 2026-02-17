using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using RaidClears.Features.Raids.Models;
using RaidClears.Features.Shared.Models;
using RaidClears.Features.Raids.Services;
using RaidClears.Features.Strikes.Models;
using RaidClears.Localization;

namespace RaidClears.Settings.Controls;

public  class EncounterLabelCustomerizer : Panel
{
    private Labelable _labelable;
    private Label title = new Label();
    private TextBox input = new TextBox();
    private Label abbrivLabel = new Label();
    private StandardButton resetBtn = new StandardButton();
    public EncounterLabelCustomerizer(FlowPanel parent, Labelable labelable, EncounterInterface encounter, Color? labelColor = null) : base()
    {
        _labelable = labelable;
        Parent = parent;
        Width = parent.Width-10;
        Padding = new Thickness(0, 10);

        Build(encounter.Name, encounter.Abbriviation, encounter.Id, labelColor);
    }
    public EncounterLabelCustomerizer(FlowPanel parent, Labelable labelable, BossEncounter encounter, Color? labelColor = null) : base()
    {
        _labelable = labelable;
        Parent = parent;
        Width = parent.Width - 10;
        Padding = new Thickness(0, 10);
        Build(encounter.Name, encounter.Abbriviation, encounter.EncounterId, labelColor);
    }
    public EncounterLabelCustomerizer(FlowPanel parent, Labelable labelable, RaidWing encounter, Color? labelColor = null) : base()
    {
        _labelable = labelable;
        Parent = parent;
        Width = parent.Width - 10;
        Padding = new Thickness(0, 10);

        Build(encounter.Name, encounter.Abbriviation, encounter.Id, labelColor);
    }
    public EncounterLabelCustomerizer(FlowPanel parent, Labelable labelable, ExpansionStrikes encounter, Color? labelColor = null) : base()
    {
        _labelable = labelable;
        Parent = parent;
        Width = parent.Width - 10;
        Padding = new Thickness(0, 10);

        Build(encounter.Name, encounter.Abbriviation, encounter.Id, labelColor);
    }
    public EncounterLabelCustomerizer(FlowPanel parent, Labelable labelable, ExpansionRaid encounter, Color? labelColor = null) : base()
    {
        _labelable = labelable;
        Parent = parent;
        Width = parent.Width - 10;
        Padding = new Thickness(0, 10);

        Build(encounter.Name, encounter.Abbriviation, encounter.Id, labelColor);
    }

    protected void Build(string Name, string abbriv, string id, Color? color = null)
    {
        if (color == null)
        {
            color = Color.White;
        }
        var userLabel = _labelable.GetEncounterLabel(id);
        var col1 = (this.Width - 30) / 3;
        var remainingWidth = (2 * col1) - 5;
        var inputWidth = remainingWidth / 3;
        var resetBtnWidth = (2 * remainingWidth) / 3;
        title = new Label()
        {
            Text = Name,
            Parent = this,
            Location = new(0, 0),
            Width = col1,
            TextColor= (Color)color
        };
        input = new TextBox()
        {
            Text = userLabel,
            Parent = this,
            Location = new(col1 + 5, 0),
            Width = inputWidth,
        
        };
        resetBtn = new StandardButton()
        {
            Text = string.Format(Strings.EncounterLabelCustomerizer_ResetTo, abbriv),
            Parent = this,
            Location = new(col1 + inputWidth + 10, 0),
            Width = resetBtnWidth,
        };
        if(abbriv == userLabel)
        {
            resetBtn.Hide();
        }

        input.TextChanged += (s, e) => {
            _labelable.SetEncounterLabel(id, input.Text);
            if(input.Text == abbriv)
            {
                resetBtn.Hide();
            }
            {
                resetBtn.Show();
            }
        };
        resetBtn.Click += (s, e) => {
            input.Text = abbriv;
            _labelable.SetEncounterLabel(id, abbriv); 
            resetBtn.Hide();
        };
    }

}
