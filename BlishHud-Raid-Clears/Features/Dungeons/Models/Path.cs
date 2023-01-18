using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using RaidClears.Features.Shared.Models;
using RaidClears.Utils;

namespace RaidClears.Features.Dungeons.Models;

public class Path : BoxModel
{
    private bool _isFrequented;
    private Color _freqColor = Color.White;
    private Color _normalTextColor = Color.White;
    
    public Path(string id, string name, string shortName) : base(id, name, shortName)
    {
    }

    public void SetFrequenter(bool freqStatus)
    {
        _isFrequented = freqStatus;
        Box.TextColor = freqStatus ? _freqColor : _normalTextColor;
    }

    public void ApplyTextColor()
    {
        Box.TextColor = _isFrequented ? _freqColor : _normalTextColor;
        Box.Invalidate();
    }

    public void RegisterFrequenterSettings(
        SettingEntry<bool> highlightFreq,
        SettingEntry<string> freqColor,
        SettingEntry<string> normalTextColor)
    {
        _freqColor = highlightFreq.Value ?
               freqColor.Value.HexToXnaColor():
               normalTextColor.Value.HexToXnaColor();
        ApplyTextColor();
        freqColor.SettingChanged += (_, e) =>
        {
            _freqColor = e.NewValue.HexToXnaColor();
            ApplyTextColor();
        };
        normalTextColor.SettingChanged += (_, e) =>
        {
            _normalTextColor = e.NewValue.HexToXnaColor();
            ApplyTextColor();
        };
        highlightFreq.SettingChanged += (_, e) =>
        {
            _freqColor = e.NewValue ? freqColor.Value.HexToXnaColor() : normalTextColor.Value.HexToXnaColor();
            ApplyTextColor();
        };
    }
}
