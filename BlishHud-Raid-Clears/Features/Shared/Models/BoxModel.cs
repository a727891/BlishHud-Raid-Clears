﻿using Microsoft.Xna.Framework;
using RaidClears.Features.Shared.Controls;

namespace RaidClears.Features.Shared.Models;

public class BoxModel
{
    public string id;
    public string name;
    public string shortName;
    private bool _isCleared;

    public GridBox Box { get; private set; }

    private readonly Color _colorUnknown = new(64, 64, 64);
    private Color _colorNotCleared = new(120, 20, 20);
    private Color _colorCleared = new(20, 120, 20);

    public BoxModel(string id, string name, string shortName) // warning
    {
        this.id = id;
        this.name = name;
        this.shortName = shortName;
    }

    public void SetClearColors(Color cleared, Color notCleared)
    {
        _colorCleared = cleared;
        _colorNotCleared = notCleared;

        Box.BackgroundColor = _isCleared ? _colorCleared : _colorNotCleared;
        Box.Invalidate();
    }

    public void SetGridBoxReference(GridBox box)
    {
        Box = box;
        Box.BackgroundColor = _colorUnknown;
    }

    public void SetCleared(bool cleared)
    {
        Box.BackgroundColor = cleared ? _colorCleared : _colorNotCleared;
        _isCleared = cleared;
    }
}
