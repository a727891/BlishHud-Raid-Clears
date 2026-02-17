using RaidClears.Utils;
using RaidClears.Features.Shared.Controls;
using RaidClears.Features.Shared.Services;
using RaidClears.Features.Raids.Models;
using System.Threading.Tasks;
using RaidClears.Features.Raids.Services;
using RaidClears.Settings.Models;
using Blish_HUD;
using Blish_HUD.Controls;
using System.Collections.Generic;

namespace RaidClears.Features.Raids;

public class RaidPanel : GridPanel
{
    private readonly IEnumerable<Wing> Wings = new List<Wing>();
    private static RaidSettings Settings => Service.Settings.RaidSettings;
    public RaidPanel(
    ) : base(Settings.Generic, GameService.Graphics.SpriteScreen)
    {
        Wings = WingFactory.Create(this);

        Service.ApiPollingService!.ApiPollingTrigger += (_, _) =>
        {
            Task.Run(async () =>
            {
                var weeklyClears = await GetCurrentClearsService.GetClearsFromApi();

                foreach (var wing in Wings)
                {
                    foreach (var encounter in wing.boxes)
                    {
                        encounter.SetCleared(weeklyClears.Contains(encounter.id));
                    }
                }

                ApplyEncounterBackgroundColors();

                if (Settings.RaidPanelMentorProgress.Value)
                    await Service.MentorAchievementProgress.RefreshFromApiAsync();

                Invalidate();
            });
        };

        Settings.Style.Color.Cleared.SettingChanged += (_, _) => ApplyEncounterBackgroundColors();
        Settings.Style.Color.NotCleared.SettingChanged += (_, _) => ApplyEncounterBackgroundColors();
        Settings.RaidPanelColorNonWeeklyBounty.SettingChanged += (_, _) => ApplyEncounterBackgroundColors();
        Settings.RaidPanelHighlightNonWeeklyBounty.SettingChanged += (_, _) => ApplyEncounterBackgroundColors();
        Settings.RaidPanelOmitEventEncounters.SettingChanged += (_, _) => ApplyEncounterBackgroundColors();

        (this as FlowPanel).LayoutChange(Settings.Style.Layout);
        (this as GridPanel).BackgroundColorChange(Settings.Style.BgOpacity, Settings.Style.Color.Background);

        RegisterKeyBindService(
            new KeyBindHandlerService(
                Settings.Generic.ShowHideKeyBind,
                Settings.Generic.Visible
            )
        );
    }

    /// <summary>Applies background colors to all encounter boxes. Cleared uses cleared color; uncleared uses non-weekly bounty color when enabled and not in weekly set, otherwise uncleared color.</summary>
    private void ApplyEncounterBackgroundColors()
    {
        var clearedColor = Settings.Style.Color.Cleared.Value.HexToXnaColor();
        var notClearedColor = Settings.Style.Color.NotCleared.Value.HexToXnaColor();
        var nonWeeklyColor = Settings.RaidPanelColorNonWeeklyBounty.Value.HexToXnaColor();
        var highlightNonWeekly = Settings.RaidPanelHighlightNonWeeklyBounty.Value;
        var weeklyBounties = Service.WeeklyBountyEncounters;

        foreach (var wing in Wings)
        {
            foreach (var encounter in wing.boxes)
            {
                if (encounter.IsCleared)
                    encounter.Box.BackgroundColor = clearedColor;
                else if (highlightNonWeekly && weeklyBounties != null && !weeklyBounties.IsWeeklyBounty(encounter.id)
                    && !(Settings.RaidPanelOmitEventEncounters.Value && Service.RaidData.IsEventEncounter(encounter.id)))
                    encounter.Box.BackgroundColor = nonWeeklyColor;
                else
                    encounter.Box.BackgroundColor = notClearedColor;
            }
        }

        Invalidate();
    }

    public void UpdateEncounterLabel(string encounterApiId, string newLabel)
    {
        foreach (var wing in Wings)
        {
            if (wing.id == encounterApiId)
            {
                wing.GroupLabel.Text = newLabel;
                wing.GroupLabel.Invalidate();
                Invalidate();
                return;
            }
            foreach (var encounter in wing.boxes)
            {
                if (encounter.id == encounterApiId)
                {
                    encounter.SetLabel(newLabel);
                    Invalidate();
                    return;
                }
            }
        }
    }

}
