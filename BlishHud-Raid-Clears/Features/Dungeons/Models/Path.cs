using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using RaidClears.Features.Shared.Models;
using RaidClears.Utils;

namespace RaidClears.Features.Dungeons.Models;

public class Path : BoxModel
{

    protected bool isFrequented = false;
    public Color FreqColor = Color.White;
    public Color NormalTextColor = Color.White;
    
    public Path(string id, string name, string short_name) : base(id, name, short_name)
    {
    }

    public void SetFrequenter(bool freqStatus)
    {
        isFrequented = freqStatus;
        Box.TextColor = freqStatus ? FreqColor : NormalTextColor;

    }

    public void ApplyTextColor()
    {
        Box.TextColor = isFrequented ? FreqColor : NormalTextColor;
        Box.Invalidate();
    }


    public void RegisterFrequenterSettings(
        SettingEntry<bool> highlightFreq,
        SettingEntry<string> freqColor,
        SettingEntry<string> normalTextColor)
    {
        FreqColor = highlightFreq.Value ?
               freqColor.Value.HexToXNAColor():
               normalTextColor.Value.HexToXNAColor();
        ApplyTextColor();
        freqColor.SettingChanged += (s, e) =>
        {
            FreqColor = e.NewValue.HexToXNAColor();
            ApplyTextColor();
        };
        normalTextColor.SettingChanged += (s, e) =>
        {
            NormalTextColor = e.NewValue.HexToXNAColor();
            ApplyTextColor();
        };
        highlightFreq.SettingChanged += (s, e) =>
        {
            if (e.NewValue)
            {
                FreqColor = freqColor.Value.HexToXNAColor();
            }
            else
            {
                FreqColor = normalTextColor.Value.HexToXNAColor();

            }
            ApplyTextColor();
        };
    }
}
