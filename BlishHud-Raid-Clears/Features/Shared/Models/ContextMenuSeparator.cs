using Blish_HUD.Controls;
using Blish_HUD;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace RaidClears.Features.Shared.Models;

public class ContextMenuStripItemSeparator : ContextMenuStripItem
{
    public ContextMenuStripItemSeparator() : base()
    {
        Enabled= false;
        base.EffectBehind = null;
        
    }
    protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
    {
        spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(0, bounds.Height / 2, bounds.Width, 1), Color.White * 0.8f);
    }
}