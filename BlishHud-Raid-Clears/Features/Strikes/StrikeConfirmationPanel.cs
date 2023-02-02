using RaidClears.Utils;
using RaidClears.Features.Shared.Services;
using RaidClears.Features.Shared.Controls;
using RaidClears.Features.Strikes.Models;
using RaidClears.Settings.Models;
using Blish_HUD;
using Blish_HUD.Controls;
using RaidClears.Features.Strikes.Services;
using RaidClears.Localization;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using RaidClears.Features.Shared.Enums;
using RaidClears.Features.Shared.Enums.Extensions;
using System;

namespace RaidClears.Features.Strikes;

public class StrikeConfirmationPanel : StandardWindow
{
    private static Texture2D? Background => Service.Textures?.SettingWindowBackground;

    //Where on the background texture should the panel render
    private static Rectangle PanelRegion => new()
    {
        Location = new Point(38, 25),
        //Location = new Point(-7, +25),
        //Size = new Point(Background!.Width, Background!.Height),
        Size = new Point(1100, 705),
    };

    private static Rectangle PanelContentRegion => new()
    {
        Location = PanelRegion.Location + new Point(10, 15),
        Size = PanelRegion.Size - PanelRegion.Location,
    };
    private static Point PanelWindowSize => new(150, 180);

    private Label _prompt = new();
    private string _strikeApi = string.Empty;
    private string _strike = string.Empty;
    private Action<string>? _callbackAction;

    public StrikeConfirmationPanel() : base(Background, PanelRegion, PanelContentRegion, PanelWindowSize)
    {
        Id = $"{nameof(Module)}_Strike_96b38a83-4163-4d97-b894-282406b29a48";
        Emblem = Service.Textures?.SettingWindowEmblem;
        Parent = GameService.Graphics.SpriteScreen;
        Title = "";
        //Subtitle = Strings.SettingsPanel_Subtitle;
        SavesPosition = true;
        //_backgroundColor = new Color(10, 10, 10);

        Service.Settings.SettingsPanelKeyBind.Value.Activated += (_, _) => ToggleWindow();

        Build();

    }

    public void AskComplete(string boss, string apiLabel, Action<string>? callback)
    {
        _callbackAction = callback;
        _strikeApi = apiLabel;
        _strike= boss;
        _prompt.Text = boss;
        Show();
    }

    protected void Build()
    {
        var panel = new FlowPanel() {
            Parent = this,
            HeightSizingMode = SizingMode.Fill,
            WidthSizingMode = SizingMode.Fill,
            FlowDirection = ControlFlowDirection.TopToBottom
        };
        //.BeginFlow(this)
        _prompt = new()
        {
            AutoSizeWidth = true,
            Height = 40,
            WrapText = true,
            Parent = panel,
            Text = "mark cleared?",
        };


        
        panel.AddFlowControl(new StandardButton()
        {
            Text = "CLEARED!",           
        }, out var yes)
        .AddFlowControl(new StandardButton()
        {
            Text = "cancel"
        }, out var nope);
        yes.Click += (_, _) => { Hide(); _callbackAction?.Invoke(_strikeApi); };
        nope.Click += (_, _) => { Hide(); };
            
    }
}
