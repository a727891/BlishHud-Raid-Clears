using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using RaidClears.Localization;
using RaidClears.Settings.Models;
using RaidClears.Utils;
using System.Diagnostics;

namespace RaidClears.Settings.Views.SubViews;

public class FractalSelectionView : View
{
    private readonly FractalSettings _settings;

    public FractalSelectionView(FractalSettings settings)
    {
        _settings = settings;
    }

    protected override void Build(Container buildPanel)
    {
        base.Build(buildPanel);

        var panel = new FlowPanel()
           .BeginFlow(buildPanel)
           .AddString(Strings.Fractals_Selection_Prompt)
           .AddSetting(_settings.ChallengeMotes)
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
            Location = new(10, buildPanel.Bottom - 50),
            //TextColor=Color.Blue,
            AutoSizeWidth=true,
            Text = "Special thank you to Invisi for providing challenge mote Instabilities information"
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