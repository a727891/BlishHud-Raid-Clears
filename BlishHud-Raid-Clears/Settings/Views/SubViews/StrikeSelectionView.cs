using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using RaidClears.Localization;
using RaidClears.Settings.Models;
using RaidClears.Utils;

namespace RaidClears.Settings.Views.SubViews;

public class StrikeSelectionView : View
{
    private readonly StrikeSettings _settings;
    
    public StrikeSelectionView(StrikeSettings settings)
    {
        _settings = settings;
    }
    
    protected override void Build(Container buildPanel)
    {
        base.Build(buildPanel);

        new FlowPanel()
            .BeginFlow(buildPanel)
            .AddSetting(_settings.StrikeVisiblePriority)
            .AddSpace()
            .AddSetting(_settings.StrikeVisibleIbs)
            .AddString(Strings.Settings_Strike_IBS_Heading)
            .AddSetting(_settings.IbsMissions)
            .AddSpace()
            .AddSetting(_settings.StrikeVisibleEod)
            .AddString(Strings.Settings_Strike_EOD_Heading)
            .AddSetting(_settings.EodMissions);
    }
}