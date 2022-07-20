using System;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework.Graphics;

namespace GatheringTools.Raids.Services
{
    public class TextureService : IDisposable
    {
        public TextureService(ContentsManager contentsManager)
        {
            
            CornerIconTexture                = contentsManager.GetTexture(@"raids\textures\cornerIcon.png");
            CornerIconHoverTexture           = contentsManager.GetTexture(@"raids\textures\cornerIcon_hover.png");
           
        }

        public void Dispose()
        {
            CornerIconTexture?.Dispose();
            CornerIconHoverTexture?.Dispose();
        }

        public Texture2D CornerIconTexture { get; }
        public Texture2D CornerIconHoverTexture { get; }
        
    }
}