﻿using System;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RaidClears.Fearures.Shared.Services
{
    public class TextureService : IDisposable
    {

        public static TextureService Instance;

        public TextureService(ContentsManager contentsManager)
        {
            Instance = this;

            CornerIconTexture = contentsManager.GetTexture(@"raids\textures\raidIconDark.png");
            CornerIconHoverTexture = contentsManager.GetTexture(@"raids\textures\raidIconBright.png");

            DungeonsCornerIconTexture = contentsManager.GetTexture(@"raids\textures\dungeonIconDark.png");
            DungeonsCornerIconHoverTexture = contentsManager.GetTexture(@"raids\textures\dungeonIconBright.png");


            SettingWindowBackground = contentsManager.GetTexture(@"controls/window/background.png");
            SettingWindowEmblem = contentsManager.GetTexture(@"module_profile_hero_icon.png");

            SettingTabRaid = contentsManager.GetTexture(@"controls/tab_icons/raid.png");
            SettingTabDungeon = contentsManager.GetTexture(@"controls/tab_icons/dungeon.png");
            SettingTabGeneral = contentsManager.GetTexture(@"controls/tab_icons/cog.png");
        }

        public void Dispose()
        {
            CornerIconTexture?.Dispose();
            CornerIconHoverTexture?.Dispose();
            DungeonsCornerIconTexture?.Dispose();
            DungeonsCornerIconHoverTexture?.Dispose();
            SettingWindowBackground?.Dispose();
            SettingWindowEmblem?.Dispose();
            SettingTabRaid?.Dispose();
            SettingTabDungeon?.Dispose();
            SettingTabGeneral?.Dispose();

        }

        public Texture2D SettingWindowBackground { get; }
        public Texture2D SettingWindowEmblem { get; }
        public Texture2D SettingTabRaid { get; }
        public Texture2D SettingTabDungeon { get; }
        public Texture2D SettingTabGeneral { get; }

        public Texture2D CornerIconTexture { get; }
        public Texture2D CornerIconHoverTexture { get; }

        public Texture2D DungeonsCornerIconTexture { get; }
        public Texture2D DungeonsCornerIconHoverTexture { get; }

    }
}