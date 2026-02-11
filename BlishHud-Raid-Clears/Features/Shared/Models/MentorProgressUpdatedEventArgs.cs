using System;
using System.Collections.Generic;

namespace RaidClears.Features.Shared.Models;

/// <summary>
/// One mentor achievement that gained progress (current increased).
/// </summary>
public sealed class MentorProgressChange
{
    public int AchievementId { get; set; }
    public int PreviousCurrent { get; set; }
    public int NewCurrent { get; set; }
    public int Delta => NewCurrent - PreviousCurrent;
}

/// <summary>
/// Event args when mentor achievement progress has been updated and at least one achievement gained progress.
/// </summary>
public sealed class MentorProgressUpdatedEventArgs : EventArgs
{
    public IReadOnlyList<MentorProgressChange> Changes { get; set; } = Array.Empty<MentorProgressChange>();
}
