﻿using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using RaidClears.Utils;
using Settings.Enums;


namespace RaidClears.Features.Shared.Controls
{
    public class GridGroup : FlowPanel
    {
        public GridGroup(
            Container parent,
            SettingEntry<Layout> layout
        )
        { 
            Parent = parent;
            ControlPadding = new Vector2(2, 2);
            HeightSizingMode = SizingMode.AutoSize;
            WidthSizingMode = SizingMode.AutoSize;
            this.LayoutChange(layout,1);
        }


    }
}