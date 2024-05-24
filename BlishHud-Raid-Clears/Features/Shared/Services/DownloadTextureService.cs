using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework.Graphics;
using RaidClears.Features.Dungeons.Models;

namespace RaidClears.Features.Shared.Services;

public class DownloadTextureService : IDisposable
{
    public DownloadTextureService()
    {

    }

    public Texture2D GetDynamicTexture(string path)
    {
        if(DynamicTextures.ContainsKey(path)) return DynamicTextures[path];

        var asyncTex = LoadTexture(path, ContentService.Textures.Pixel);
        DynamicTextures.Add(path, asyncTex);
        return asyncTex;
    }

    public bool ValidateTextureCache(string fileName)
    {
        if (GetFileInfo(fileName) is { Exists: false } configFileInfo)
        {
            return DownloadFile(Module.STATIC_HOST_URL, fileName);
        }
        return true;
        
    }

    protected Texture2D LoadTexture(string fileName, Texture2D fallbackTexture)
    {
        //DynamicTextures[fileName].SwapTexture(fallbackTexture);

        if (GetFileInfo(fileName) is { Exists: true } configFileInfo)
        {
            using FileStream stream = new FileStream(configFileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.Read);
            return TextureUtil.FromStreamPremultiplied(stream);
            
        }
        else
        {
            if (DownloadFile(Module.STATIC_HOST_URL, fileName))
            {
                return LoadTexture(fileName, fallbackTexture);
            }
            else
            {
                return ContentService.Textures.Error;
            }
        }
        
    }

    private FileInfo GetFileInfo(string fileName)
    {
        var moduleDirectory = Service.DirectoriesManager.GetFullDirectoryPath(Module.DIRECTORY_PATH);

        return new FileInfo($@"{moduleDirectory}\{fileName}");
    }

    private bool DownloadFile(string url, string fileName)
    {
        try
        {
            using (var webClient = new System.Net.WebClient())
            {
                var moduleDirectory = Service.DirectoriesManager.GetFullDirectoryPath(Module.DIRECTORY_PATH);

                var savePath = $@"{moduleDirectory}\{fileName}";
                webClient.DownloadFile($"{url}/{fileName}",savePath);
                return true;
            }
        }
        catch (Exception r)
        {
        }
        return false;
    }

    public void Dispose()
    {
        DynamicTextures.ToList().ForEach(t => t.Value.Dispose());

    }
    protected Dictionary<string, AsyncTexture2D> DynamicTextures { get; } = new();

}