using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework.Graphics;
namespace RaidClears.Features.Shared.Services;

public class TextureService : IDisposable
{
    public TextureService(ContentsManager contentsManager)
    {
        _downloadTextures = new DownloadTextureService();
        GridBoxBackgroundTexture = new();
        for (int i = 1; i <=7; i++)
        {
            GridBoxBackgroundTexture.Add(contentsManager.GetTexture($"white{i}.png"));
        }

        CornerIconTexture = contentsManager.GetTexture(@"raids\textures\raidIconDark.png");
        CornerIconHoverTexture = contentsManager.GetTexture(@"raids\textures\raidIconBright.png");

        SettingWindowBackground = GetDynamicTexture("texture_background.png");
        SettingWindowEmblem = contentsManager.GetTexture(@"module_profile_hero_icon.png");

        SettingTabRaid = contentsManager.GetTexture(@"controls/tab_icons/raid.png");
        SettingTabDungeon = contentsManager.GetTexture(@"controls/tab_icons/dungeon.png");
        SettingTabGeneral = contentsManager.GetTexture(@"controls/tab_icons/cog.png");
        SettingTabStrikes = contentsManager.GetTexture(@"controls/tab_icons/strikes.png");
        SettingTabFractals = contentsManager.GetTexture(@"controls/tab_icons/fotm.png");

        JWLogo = GetDynamicTexture("texture_raids_jw.png");
        PoFLogo = GetDynamicTexture("texture_raids_pof.png");
        HoTLogo = GetDynamicTexture("texture_raids_hot.png");
        BaseLogo = GetDynamicTexture("texture_base_logo.png");

    }

    public AsyncTexture2D GetDynamicTexture(string path)
    {
        return _downloadTextures.GetDynamicTexture(path);
    }

    public void Dispose()
    {
        foreach(var t in GridBoxBackgroundTexture)
        {
            t.Dispose();
        }
        CornerIconTexture.Dispose();
        CornerIconHoverTexture.Dispose();
        SettingWindowBackground.Dispose();
        SettingWindowEmblem.Dispose();
        SettingTabRaid.Dispose();
        SettingTabDungeon.Dispose();
        SettingTabGeneral.Dispose();
        SettingTabStrikes.Dispose();
        SettingTabFractals.Dispose();
        JWLogo.Dispose();
        PoFLogo.Dispose();
        HoTLogo.Dispose();
        BaseLogo.Dispose();

        _downloadTextures.Dispose();

    }
    protected DownloadTextureService _downloadTextures { get; set; }
    public Texture2D JWLogo { get; }
    public Texture2D PoFLogo { get; }
    public Texture2D HoTLogo { get; }
    public Texture2D BaseLogo { get; }
    public Texture2D SettingWindowBackground { get; }
    public Texture2D SettingWindowEmblem { get; }
    public Texture2D SettingTabRaid { get; }
    public Texture2D SettingTabDungeon { get; }
    public Texture2D SettingTabGeneral { get; }
    public Texture2D SettingTabStrikes { get; }
    public Texture2D SettingTabFractals { get; }
    public Texture2D CornerIconTexture { get; }
    public Texture2D CornerIconHoverTexture { get; }
    public List<Texture2D> GridBoxBackgroundTexture { get; }

}