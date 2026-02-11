using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace RaidClears.Features.Shared.Models;

/// <summary>
/// Progress for a single mentor (boss) achievement from the GW2 API.
/// </summary>
public sealed class MentorAchievementProgressEntry
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("current")]
    public int Current { get; set; }

    [JsonProperty("max")]
    public int Max { get; set; }

    [JsonProperty("done")]
    public bool Done { get; set; }

    public bool Equals(MentorAchievementProgressEntry? other) =>
        other != null && Id == other.Id && Current == other.Current && Max == other.Max && Done == other.Done;
}

/// <summary>
/// Persisted cache of mentor achievement progress for offline use and change detection.
/// </summary>
public sealed class MentorAchievementProgressCache
{
    [JsonProperty("version")]
    public string Version { get; set; } = "1.0";

    [JsonProperty("updated_utc")]
    public string? UpdatedUtc { get; set; }

    [JsonProperty("achievements")]
    public List<MentorAchievementProgressEntry> Achievements { get; set; } = new();
}
