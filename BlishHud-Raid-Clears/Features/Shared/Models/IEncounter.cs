namespace RaidClears.Features.Shared.Models;

/// <summary>
/// Common abstraction for individual encounters (raid bosses, strike missions, etc.).
/// </summary>
public interface IEncounter
{
    /// <summary>Stable identifier for the encounter (raid API id, strike id, etc.).</summary>
    string Id { get; }

    /// <summary>Display name (localized).</summary>
    string Name { get; }

    /// <summary>Short label / abbreviation (localized).</summary>
    string Abbriviation { get; }

    /// <summary>Icon asset id used for rendering.</summary>
    int IconAssetId { get; }

    /// <summary>Optional daily bounty achievement id for encounters that are bounties.</summary>
    int? DailyBountyAchievementId { get; }
}

