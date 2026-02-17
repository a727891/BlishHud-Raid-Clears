using System.Collections.Generic;

namespace RaidClears.Features.Shared.Models;

/// <summary>
/// Common abstraction for raid/strike expansions that group child encounters.
/// </summary>
/// <typeparam name="TChild">The type of child encounters within the expansion.</typeparam>
public interface IExpansion<TChild> where TChild : EncounterInterface
{
    /// <summary>Internal ID for the expansion (backed by <see cref="EncounterInterface.Id" />).</summary>
    string Id { get; }

    /// <summary>Display name of the expansion (localized).</summary>
    string Name { get; }

    /// <summary>Short label/abbreviation for the expansion (localized).</summary>
    string Abbriviation { get; }

    /// <summary>Static asset path for the expansion icon.</summary>
    string Asset { get; }

    /// <summary>Direct child encounters (e.g. wings or missions) within this expansion.</summary>
    IReadOnlyList<TChild> Children { get; }
}

