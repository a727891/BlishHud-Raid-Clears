
using Blish_HUD.Settings;
using RaidClears.Features.Shared.Models;

namespace RaidClears.Utils;

internal static class BoxModelExtension
{
    public static void WatchColorSettings(
        this BoxModel box,
        SettingEntry<string> clearedColor,
        SettingEntry<string> notClearedColor
    )
    {
        box.SetClearColors(clearedColor.Value.HexToXnaColor(), notClearedColor.Value.HexToXnaColor());
        clearedColor.SettingChanged += (s,e)=> box.SetClearColors(e.NewValue.HexToXnaColor(), notClearedColor.Value.HexToXnaColor());
        notClearedColor.SettingChanged += (s, e) => box.SetClearColors(clearedColor.Value.HexToXnaColor(), e.NewValue.HexToXnaColor());
    }





}
