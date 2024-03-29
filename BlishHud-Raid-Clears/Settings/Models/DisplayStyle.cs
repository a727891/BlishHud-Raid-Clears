﻿using Blish_HUD;
using Blish_HUD.Settings;
using RaidClears.Settings.Enums;
#pragma warning disable CS8618

namespace RaidClears.Settings.Models;

public class DisplayStyle
{
    public SettingEntry<ContentService.FontSize> FontSize { get; set; }
    public SettingEntry<LabelDisplay> LabelDisplay { get; set; }
    public SettingEntry<Layout> Layout { get; set; }
    public SettingEntry<float> LabelOpacity { get; set; }
    public SettingEntry<float> GridOpacity { get; set; }
    public SettingEntry<float> BgOpacity { get; set; }
    public DisplayColor Color { get; set; }
}