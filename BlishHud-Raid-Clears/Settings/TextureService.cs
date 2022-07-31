using System;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework.Graphics;

namespace RaidClears.Settings
{
    public class TextureService : IDisposable
    {
        public TextureService(ContentsManager contentsManager)
        {

            CornerIconTexture = contentsManager.GetTexture(@"raids\textures\raidIconDark.png");
            CornerIconHoverTexture = contentsManager.GetTexture(@"raids\textures\raidIconBright.png");


            DungeonsCornerIconTexture = contentsManager.GetTexture(@"raids\textures\dungeonIconDark.png");
            DungeonsCornerIconHoverTexture = contentsManager.GetTexture(@"raids\textures\dungeonIconBright.png");

        }

        public void Dispose()
        {
            CornerIconTexture?.Dispose();
            CornerIconHoverTexture?.Dispose();
            DungeonsCornerIconTexture?.Dispose();
            DungeonsCornerIconHoverTexture?.Dispose();
        }
        
        public Texture2D CornerIconTexture { get; }
        public Texture2D CornerIconHoverTexture { get; }

        public Texture2D DungeonsCornerIconTexture { get; }
        public Texture2D DungeonsCornerIconHoverTexture { get; }

    }
}