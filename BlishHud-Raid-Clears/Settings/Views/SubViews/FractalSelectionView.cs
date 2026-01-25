using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using RaidClears.Localization;
using RaidClears.Settings.Models;
using RaidClears.Utils;
using System.Diagnostics;
using System.Collections.Generic;
using Blish_HUD.Settings;
using RaidClears.Features.Fractals.Services;

namespace RaidClears.Settings.Views.SubViews;

public class FractalSelectionView : View
{
    private readonly FractalSettings _settings;
    private readonly FractalSettingsPersistance _fractalSettings;

    public FractalSelectionView(FractalSettings settings)
    {
        _settings = settings;
        _fractalSettings = Service.FractalSettings;
    }

    protected override void Build(Container buildPanel)
    {
        base.Build(buildPanel);

        var panel = new FlowPanel()
           .BeginFlow(buildPanel)
           .AddString(Strings.Fractals_Selection_Prompt)
           .AddSetting(_settings.ChallengeMotes);

        // Add individual challenge mote checkboxes
        List<SettingEntry<bool>> challengeMotes = new();
        foreach (var scale in Service.FractalMapData.ChallengeMotes)
        {
            var fractal = Service.FractalMapData.GetFractalForScale(scale);
            if (fractal.ApiLabel != "undefined")
            {
                challengeMotes.Add(_fractalSettings.GetChallengeMoteVisible(fractal));
            }
        }
        
        panel
            .AddFlowControl(
                new FlowPanel()
                {
                    FlowDirection = ControlFlowDirection.SingleTopToBottom,
                    Width = panel.Width - 40,
                    HeightSizingMode = SizingMode.AutoSize,
                    OuterControlPadding = new(20, 0),
                }
                .AddHorizontalSpace(20)
                .AddSetting(challengeMotes)
            );

        panel
            .AddSpace()
           .AddSetting(_settings.DailyTierN)
           .AddSetting(_settings.DailyRecs)
           .AddSpace()
           .AddSetting(_settings.TomorrowTierN);
            

        new Image()
        {
            Texture = Service.Textures!.BaseLogo,
            Parent = buildPanel,
            Location = new(300, 65),
            Size = new Microsoft.Xna.Framework.Point(400, 278).Scale(0.5f)

        };

        var thanksInvisButton = new Label()
        {
            Parent = buildPanel,
            Location = new(10, buildPanel.Bottom - 60),
            //TextColor=Color.Blue,
            AutoSizeWidth=true,
            Height=50,
            WrapText=true,
            Text = Strings.FractalSelection_ThanksInvisi
        };
        thanksInvisButton.Click += (s, e) =>
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://github.com/Invisi/gw2-fotm-instabilities",
                UseShellExecute = true
            });
        };
    }
}