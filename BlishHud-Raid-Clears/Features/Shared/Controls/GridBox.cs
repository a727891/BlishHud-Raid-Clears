using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Effects;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using RaidClears.Settings.Enums;
using System;
using System.Threading.Tasks;

namespace RaidClears.Features.Shared.Controls;

public class MyEffect : ControlEffect
{
    private readonly Texture2D texture;
    private readonly Rectangle boundChange;
    public Color Tint { get; set; } = Color.Transparent;
    public MyEffect(Control assignedControl) : base(assignedControl)
    {
        texture = Service.Textures!.GetRandomGridBoxMask();
        boundChange = new Rectangle(
            Service.Random.Next(0, 3),
            Service.Random.Next(0, 2),
            Service.Random.Next(-3, 0),
            Service.Random.Next(-2,0)
            );
    }

    protected override SpriteBatchParameters GetSpriteBatchParameters()
    {
        return new SpriteBatchParameters();
    }
    public override void PaintEffect(SpriteBatch spriteBatch, Rectangle bounds)
    {
        if (Service.Settings.OrganicGridBoxBackgrounds.Value)
        {
            
            spriteBatch.DrawOnCtrl(AssignedControl, texture, bounds.Add(boundChange), Tint);
        }
        else
        {
            spriteBatch.DrawOnCtrl(AssignedControl, ContentService.Textures.Pixel, bounds, Tint);
        }
    }
}

public class GridBox : Label
{
    protected MyEffect bgtexture { get; set; }
    public GridBox(
        Container parent,
        string title,
        string tooltip,
        SettingEntry<float> opacity,
        SettingEntry<ContentService.FontSize> fontSize
    )
    {
        Parent = parent;

        Text = title;
        BasicTooltipText = tooltip;
        HorizontalAlignment = HorizontalAlignment.Center;
        AutoSizeHeight = true;

        OpacityChange(opacity);
        FontSizeChange(fontSize);
        bgtexture = new MyEffect(this);
        EffectBehind = (ControlEffect)bgtexture;
    }

    new public Color BackgroundColor
    {
        get
        {
            return bgtexture.Tint;
            //return _backgroundColor;
        }
        set
        {
            bgtexture.Tint = value;
            //SetProperty(ref _backgroundColor, value, invalidateLayout: false, "BackgroundColor");
        }
    }

    private void OpacityChange(SettingEntry<float> opacity )
    {
        opacity.SettingChanged += (_, e) =>
        {
            Opacity = e.NewValue;
        };
        Opacity = opacity.Value;
    }
    
    public void LayoutChange(SettingEntry<Layout> layout)
    {
        layout.SettingChanged += (_, e) =>
        {
            HorizontalAlignment = LabelAlignment(e.NewValue);
        };
        HorizontalAlignment = LabelAlignment(layout.Value);
    }

    private static HorizontalAlignment LabelAlignment(Layout layout)
    {
        return layout switch
        {
            Layout.Vertical or Layout.SingleRow => HorizontalAlignment.Right,
            Layout.Horizontal or Layout.SingleColumn => HorizontalAlignment.Center,
            _ => HorizontalAlignment.Center,
        };
    }

    private void FontSizeChange(SettingEntry<ContentService.FontSize> fontSize)
    {
        fontSize.SettingChanged += (_, e) =>
        {
            SetFontSize(e.NewValue, this);
        };
        SetFontSize(fontSize.Value, this);

    }
    private void SetFontSize(ContentService.FontSize fontSize, Label label)
    {
        var font = GameService
            .Content
            .GetFont(
                ContentService.FontFace.Menomonia,
                fontSize,
                ContentService.FontStyle.Regular
           );
        var width = GetLabelWidthForFontSize(fontSize);

        label.Font = font;
        label.Width = width;
    }
    
    private static int GetLabelWidthForFontSize(ContentService.FontSize size)
    {
        switch (size)
        {
            case ContentService.FontSize.Size36:
                return 100;
            case ContentService.FontSize.Size34:
            case ContentService.FontSize.Size32:
                return 80;
            case ContentService.FontSize.Size24:
                return 55;
            case ContentService.FontSize.Size22:
            case ContentService.FontSize.Size20:
                return 50;
            case ContentService.FontSize.Size18:
                return 45;
            case ContentService.FontSize.Size16:
            case ContentService.FontSize.Size14:
                return 40;
            case ContentService.FontSize.Size12:
            case ContentService.FontSize.Size11:
                return 35;
            case ContentService.FontSize.Size8:
                return 39;
            default: return 40;
        }
    }


}
