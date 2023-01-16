
using Blish_HUD.Settings;
using RaidClears.Features.Shared.Models;

namespace RaidClears.Utils
{
    internal static class BoxModelExtension
    {
        public static void WatchColorSettings(
            this BoxModel box,
            SettingEntry<string> clearedColor,
            SettingEntry<string> notClearedColor
        )
        {
            box.SetClearColors(clearedColor.Value.HexToXNAColor(), notClearedColor.Value.HexToXNAColor());
            clearedColor.SettingChanged += (s,e)=> box.SetClearColors(e.NewValue.HexToXNAColor(), notClearedColor.Value.HexToXNAColor());
            notClearedColor.SettingChanged += (s, e) => box.SetClearColors(clearedColor.Value.HexToXNAColor(), e.NewValue.HexToXNAColor());
        }





    }

}
