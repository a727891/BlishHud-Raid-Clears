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

namespace RaidClears.Features.Fractals;

public class CmTooltip : Blish_HUD.Controls.Tooltip
{
    private readonly DetailedTexture _image = new();//{ TextureRegion = new(14, 14, 100, 100), };
    private readonly Label _title;
    private readonly Label _id;
/*    private readonly Label _instabsTitle;
    private readonly Label _tomorrowInstabsTitle;*/
    private readonly Label _instabs;
    private readonly Label _tomorrowInstabs;
    private readonly List<DetailedTexture> _instabIcons = new();
    private readonly List<string> _instabNames = new();

    private readonly Rectangle _instabsTitle = new Rectangle(4,48+5,150,32);
    private readonly Rectangle _tomorrowInstabsTitle = new Rectangle(4 + 150+32, 48 + 5, 150, 32);

    //private Skill _skill;
    private CMInterface _cmInterface;

    public CmTooltip()
    {
       
        Rectangle imageBounds;
        _image.Texture = Service.Textures!.SettingTabFractals;
        _image.Bounds = imageBounds = new(4, 4, 48, 48);

        _title = new()
        {
            Parent = this,
            Height = Content.DefaultFont16.LineHeight,
            Width = 300 - _image.Bounds.X,
            Location = new(imageBounds.Right, imageBounds.Top),
            Font = Content.DefaultFont16,
            TextColor = Colors.Chardonnay
        };

        _id = new()
        {
            Parent = this,
            Height = Content.DefaultFont12.LineHeight,
            Width= 300-_image.Bounds.X,
            Location = new(imageBounds.Right, _title.Bottom),
            Font = Content.DefaultFont12,
            TextColor = Color.White * 0.8F,
        };
 /*       _instabsTitle = new()
        {
            Parent = this,
            Width = 150,
            AutoSizeHeight = true,
            Location = new(imageBounds.Left, imageBounds.Bottom + 5),
            Font = Content.DefaultFont14,
            Text="Instabilities:",
            TextColor= Colors.Chardonnay
        };
        _tomorrowInstabsTitle = new()
        {
            Parent = this,
            Width = 150,
            AutoSizeHeight = true,
            Location = new(_instabsTitle.Right+5+32, imageBounds.Bottom + 5),
            Font = Content.DefaultFont14,
            Text = "Tomorrow:",
            TextColor = Colors.Chardonnay
        };*/

        /*_instabs = new()
        {
            Parent = this,
            Width = 125,
            AutoSizeHeight = true,
            Location = new(_instabsTitle.Left+32, _instabsTitle.Bottom + 5),
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
        };*/
        

    }

    public CMInterface Fractal
    {
        get => _cmInterface; set => Common.SetProperty(ref _cmInterface, value, ApplyFractal);
    }
    private void ApplyFractal(object sender, Utils.Kenedia.ValueChangedEventArgs<CMInterface> e)
    {
        _instabNames.Clear();
        _instabIcons.ForEach(i => i.Dispose());
        _instabIcons.Clear();

        var map = e!.NewValue!.Map;
        var scale = e!.NewValue!.Scale;
        var day = e!.NewValue!.DayOfyear;
        var instabs = Service.InstabilitiesData.GetInstabsForLevelOnDay(scale, day);
        var tomorrowInstabs = Service.InstabilitiesData.GetInstabsForLevelOnDay(scale, (day + 1) % 366);
        _instabNames.AddRange(instabs.Concat(tomorrowInstabs).ToList());
        var assetIds = Service.FractalMapData.GetInstabilityAssetIdByNames(_instabNames);
        var index = 0;
        assetIds.ForEach((id) => {
            var icon = new DetailedTexture(id);
            icon.Bounds = new Rectangle(_image.Bounds.Left+(index>=3?150+32+5:0), _image.Bounds.Bottom+32+(32 * (index%3))+5, 32, 32);
            _instabIcons.Add(icon);
            index++;
        });
        _title.Text = $"{map.Label} ({Service.FractalPersistance.GetEncounterLabel(map.ApiLabel)})";
        _id.Text = $"Scale: {scale}";
       /* _instabs.Text = string.Join("\n",instabs);
        _tomorrowInstabs.Text = string.Join("\n",tomorrowInstabs);*/
        
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
        spriteBatch.DrawStringOnCtrl(
            this, "Instabilities", GameService.Content.DefaultFont14, _instabsTitle, Color.Chartreuse
        );
        spriteBatch.DrawStringOnCtrl(
            this, "Tomorrow", GameService.Content.DefaultFont14, _tomorrowInstabsTitle, Color.Chartreuse
        );
        var i = 0;
        _instabIcons.ForEach(icon => {
            icon.Draw(this, spriteBatch);
            try
            {
                spriteBatch.DrawStringOnCtrl(
                this, _instabNames[i], GameService.Content.DefaultFont14,
                    new Rectangle(icon.Bounds.X + icon.Size.X+5, icon.Bounds.Y, 125, icon.Bounds.Height), Color.White
                );
            }
            catch (Exception e)
            {

            }
            i++;
        });

    }

    public override void RecalculateLayout()
    {
        

        base.Size = new(370,200);
        base.ContentRegion = new Rectangle(5, 5, 360, 190);
    }

    protected override void DisposeControl()
    {
        //Skill = null;
        _image.Texture = null;

        base.DisposeControl();
    }
}
