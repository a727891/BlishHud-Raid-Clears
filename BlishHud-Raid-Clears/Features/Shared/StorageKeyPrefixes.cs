using System;

namespace RaidClears.Features.Shared;

/// <summary>Prefixes used for encounter/bounty keys in settings. Stripped when persisting to JSON so only base ids are stored.</summary>
public static class StorageKeyPrefixes
{
    public const string Priority = "priority_";
    public const string Tomorrow = "tomorrow_";

    /// <summary>Storage key for labels/visibility: never persist prefix so JSON stays clean. Preserve "priority" and "priority_tomorrow" as-is.</summary>
    public static string NormalizeStorageKey(string key)
    {
        if (key == "priority" || key == "priority_tomorrow") return key;
        if (key.StartsWith(Priority, StringComparison.Ordinal)) return key.Substring(Priority.Length);
        if (key.StartsWith(Tomorrow, StringComparison.Ordinal)) return key.Substring(Tomorrow.Length);
        return key;
    }
}
