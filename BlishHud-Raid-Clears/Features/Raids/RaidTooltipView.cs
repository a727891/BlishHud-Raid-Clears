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
using Blish_HUD;
using System.Collections.Generic;
using Blish_HUD.Content;
using System;
using System.Drawing;
using RaidClears.Features.Raids.Models;
using SharpDX.Direct2D1.Effects;
using RaidClears.Features.Strikes.Models;

namespace RaidClears.Features.Raids;

public class RaidTooltipView : Blish_HUD.Controls.Tooltip
{
    //private readonly DetailedTexture _image = new(){ TextureRegion = new(14, 14, 100, 100), };
    private readonly Label _title;
    private readonly Label _id;
    private readonly Blish_HUD.Controls.Image _icon;
 

    private RaidEncounter _encounter;
    private StrikeMission _strikeMission;

    public RaidTooltipView()
    {
       
        Rectangle imageBounds;
        // _image.Texture = Service.Textures!.SettingTabFractals;
        // _image.Bounds = imageBounds = new(4, 4, 48, 48);

        _icon = new()
        {
            Parent = this,
            Height = 48,
            Width = 48,
            Texture = ContentService.Textures.Pixel,
            Location = new() { X = 0, Y = 0 }
        };
        _title = new()
        {
            Parent = this,
            Height = Content.DefaultFont16.LineHeight,
            AutoSizeWidth=true,
            Location = new(_icon.Right+5, _icon.Top+5),
            Font = Content.DefaultFont16,
            TextColor = Colors.Chardonnay
        };

        _id = new()
        {
            Parent = this,
            Height = Content.DefaultFont12.LineHeight,
            AutoSizeWidth=true,
            Location = new(_title.Left, _title.Bottom),
            Font = Content.DefaultFont12,
            TextColor = Color.White * 0.8F,
        };
        

    }

    public RaidEncounter Encoutner
    {
        get => _encounter; set => Common.SetProperty(ref _encounter, value, ApplyEncounter);
    }
    private void ApplyEncounter(object sender, Utils.Kenedia.ValueChangedEventArgs<RaidEncounter> e)
    {
        if(e.NewValue ==null) { 
            return;
        }
        _title.Text = $"{e.NewValue.Name}";
        _id.Text = $"({Service.RaidSettings.GetEncounterLabel(e.NewValue.ApiId)})";
        if(e.NewValue.AssetId > 0)
        {
            _icon.Texture = Service.Textures!.DatAsset(e.NewValue.AssetId);
        }
    }

    public StrikeMission StrikeMission
    {
        get => _strikeMission; set => Common.SetProperty(ref _strikeMission, value, ApplyStrikeMission);
    }
    private void ApplyStrikeMission(object sender, Utils.Kenedia.ValueChangedEventArgs<StrikeMission> e)
    {
        if (e.NewValue == null)
        {
            return;
        }
        _title.Text = $"{e.NewValue.Name}";
        _id.Text = $"({Service.StrikeSettings.GetEncounterLabel(e.NewValue.Id)})";
        if (e.NewValue.AssetId > 0)
        {
            _icon.Texture = Service.Textures!.DatAsset(e.NewValue.AssetId);
        }
    }

    public override void Draw(SpriteBatch spriteBatch, Rectangle drawBounds, Rectangle scissor)
    {
        //if (Skill == null) return;
        base.Draw(spriteBatch, drawBounds, scissor);
    }

    public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
    {
        base.PaintBeforeChildren(spriteBatch, bounds);

        //_image.Draw(this, spriteBatch);
        
    }

    public override void RecalculateLayout()
    {
        base.Size = new(230, 58);
        base.ContentRegion = new Rectangle(5, 5, 220, 48);
    }

    protected override void DisposeControl()
    {
        //Skill = null;
        //_image.Texture = null;

        base.DisposeControl();
    }
}
