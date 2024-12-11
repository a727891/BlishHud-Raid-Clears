using RaidClears.Utils.Kenedia;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Blish_HUD.ContentService;
using Gw2Sharp.WebApi;
using RaidClears.Features.Fractals.Services;
using Gw2Sharp.WebApi.V2.Models;
using RaidClears.Localization;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using RaidClears.Features.Fractals.Models;
using System.Linq;

namespace RaidClears.Features.Fractals;

public class CmTooltip : Blish_HUD.Controls.Tooltip
{
    private readonly DetailedTexture _image = new() { TextureRegion = new(14, 14, 100, 100), };
    private readonly Label _title;
    private readonly Label _id;
    private readonly Label _instabsTitle;
    private readonly Label _tomorrowInstabsTitle;
    private readonly Label _instabs;
    private readonly Label _tomorrowInstabs;

    //private Skill _skill;
    private CMInterface _cmInterface;

    public CmTooltip()
    {
        WidthSizingMode = Blish_HUD.Controls.SizingMode.AutoSize;
        HeightSizingMode = Blish_HUD.Controls.SizingMode.AutoSize;
        AutoSizePadding = new(5);

        Rectangle imageBounds;
        _image.Bounds = imageBounds = new(4, 4, 48, 48);

        _title = new()
        {
            Parent = this,
            Height = Content.DefaultFont16.LineHeight,
            AutoSizeWidth = true,
            Location = new(imageBounds.Right, imageBounds.Top),
            Font = Content.DefaultFont16,
            TextColor = Colors.Chardonnay
        };

        _id = new()
        {
            Parent = this,
            Height = Content.DefaultFont12.LineHeight,
            AutoSizeWidth = true,
            Location = new(imageBounds.Right, _title.Bottom),
            Font = Content.DefaultFont12,
            TextColor = Color.White * 0.8F,
        };
        _instabsTitle = new()
        {
            Parent = this,
            Width = 125,
            AutoSizeHeight = true,
            Location = new(imageBounds.Left, imageBounds.Bottom + 5),
            Font = Content.DefaultFont14,
            Text="Instabilities:",
            TextColor= Colors.Chardonnay
        };
        _tomorrowInstabsTitle = new()
        {
            Parent = this,
            Width = 125,
            AutoSizeHeight = true,
            Location = new(_instabsTitle.Right+5, imageBounds.Bottom + 5),
            Font = Content.DefaultFont14,
            Text = "Tomorrow:",
            TextColor = Colors.Chardonnay
        };

        _instabs = new()
        {
            Parent = this,
            Width = 125,
            AutoSizeHeight = true,
            Location = new(_instabsTitle.Left, _instabsTitle.Bottom + 5),
            Font = Content.DefaultFont14,
            WrapText = true,
        };
        _tomorrowInstabs = new()
        {
            Parent = this,
            Width = 125,
            AutoSizeHeight = true,
            Location = new(_instabs.Right+5, _instabsTitle.Bottom + 5),
            Font = Content.DefaultFont14,
            WrapText = true,
        };

    }

    public CMInterface Fractal
    {
        get => _cmInterface; set => Common.SetProperty(ref _cmInterface, value, ApplyFractal);
    }
    private void ApplyFractal(object sender, ValueChangedEventArgs<CMInterface> e)
    {
        var map = e!.NewValue!.Map;
        var scale = e!.NewValue!.Scale;
        var day = e!.NewValue!.DayOfyear;
        var instabs = Service.InstabilitiesData.GetInstabsForLevelOnDay(scale, day);
        var tomorrowInstabs = Service.InstabilitiesData.GetInstabsForLevelOnDay(scale, (day + 1) % 366);
        _title.Text = $"{map.Label} ({map.ShortLabel})";
        _id.Text = $"Scale: {scale}";
        _instabs.Text = string.Join("\n",instabs);
        _tomorrowInstabs.Text = string.Join("\n",tomorrowInstabs);
        _image.Texture = Service.Textures!.SettingWindowEmblem;
    }


    /*private void ApplySkill(object sender, ValueChangedEventArgs<Skill> e)
    {
        _title.TextColor = Colors.Chardonnay;
        _title.Text = Skill?.Name;
        _id.Text = $"{strings.SkillId}: {Skill?.Id}";
        _description.Text = Skill?.Description.InterpretItemDescription();
        _image.Texture = Skill?.Icon;
    }*/


    public override void Draw(SpriteBatch spriteBatch, Rectangle drawBounds, Rectangle scissor)
    {
        //if (Skill == null) return;
        base.Draw(spriteBatch, drawBounds, scissor);
    }

    public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
    {
        base.PaintBeforeChildren(spriteBatch, bounds);

        _image.Draw(this, spriteBatch);
    }

    protected override void DisposeControl()
    {
        //Skill = null;
        _image.Texture = null;

        base.DisposeControl();
    }
}
