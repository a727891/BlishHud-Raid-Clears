using System;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework.Graphics;

namespace RaidClears.Features.Shared.Services;

public class TextureService : IDisposable
{

    public static TextureService instance;

    public TextureService(ContentsManager contentsManager)
    {
        instance = this;

        CornerIconTexture = contentsManager.GetTexture(@"raids\textures\raidIconDark.png");
        CornerIconHoverTexture = contentsManager.GetTexture(@"raids\textures\raidIconBright.png");

        DungeonsCornerIconTexture = contentsManager.GetTexture(@"raids\textures\dungeonIconDark.png");
        DungeonsCornerIconHoverTexture = contentsManager.GetTexture(@"raids\textures\dungeonIconBright.png");

        StrikesCornerIconTexture = contentsManager.GetTexture(@"raids\textures\strikes_dark.png");
        StrikesCornerIconHoverTexture = contentsManager.GetTexture(@"raids\textures\strikes_bright.png");



        SettingWindowBackground = contentsManager.GetTexture(@"controls/window/background.png");
        SettingWindowEmblem = contentsManager.GetTexture(@"module_profile_hero_icon.png");

        SettingTabRaid = contentsManager.GetTexture(@"controls/tab_icons/raid.png");
        SettingTabDungeon = contentsManager.GetTexture(@"controls/tab_icons/dungeon.png");
        SettingTabGeneral = contentsManager.GetTexture(@"controls/tab_icons/cog.png");
        SettingTabStrikes = contentsManager.GetTexture(@"controls/tab_icons/strikes.png");
    }

    public void Dispose()
    {
        CornerIconTexture.Dispose();
        CornerIconHoverTexture.Dispose();
        DungeonsCornerIconTexture.Dispose();
        DungeonsCornerIconHoverTexture.Dispose();
        SettingWindowBackground.Dispose();
        SettingWindowEmblem.Dispose();
        SettingTabRaid.Dispose();
        SettingTabDungeon.Dispose();
        SettingTabGeneral.Dispose();
        SettingTabStrikes.Dispose();

    }

    public Texture2D SettingWindowBackground { get; }
    public Texture2D SettingWindowEmblem { get; }
    public Texture2D SettingTabRaid { get; }
    public Texture2D SettingTabDungeon { get; }
    public Texture2D SettingTabGeneral { get; }
    public Texture2D SettingTabStrikes { get; }
    public Texture2D CornerIconTexture { get; }
    public Texture2D CornerIconHoverTexture { get; }

    public Texture2D DungeonsCornerIconTexture { get; }
    public Texture2D DungeonsCornerIconHoverTexture { get; }

    public Texture2D StrikesCornerIconTexture { get; }
    public Texture2D StrikesCornerIconHoverTexture { get; }

}