using System;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using RaidClears;
using static Blish_HUD.ContentService;

namespace RaidClears.Features.Raids;

/// <summary>
/// Celebratory popup shown when mentor progress increases. Displays boss name, icon, current/max, and point delta with a close button.
/// </summary>
public class MentorProgressPopupPanel : Panel
{
    private const int IconSize = 48;
    private const int Padding = 8;
    private readonly Image _icon;
    private readonly Label _bossNameLabel;
    private readonly Label _progressLabel;
    private readonly Label _deltaLabel;

    public MentorProgressPopupPanel(string bossName, int current, int max, int delta, int iconAssetId) : base()
    {
        Size = new Point(300, 72);
        //BackgroundColor = Color.Black;
        BackgroundTexture = Service.Textures?.DatAsset(156112);
        ShowBorder = false;
        ShowTint = false;

        _icon = new Image
        {
            Parent = this,
            Size = new Point(IconSize, IconSize),
            Location = new Point(Padding, (Height - IconSize) / 2),
            Texture = iconAssetId > 0 && Service.Textures != null
                ? Service.Textures.DatAsset(iconAssetId)
                : Textures.Pixel
        };

        int contentLeft = Padding + IconSize + Padding;
        _bossNameLabel = new Label
        {
            Parent = this,
            Location = new Point(contentLeft, Padding),
            AutoSizeWidth = true,
            AutoSizeHeight = true,
            Font = Content.DefaultFont16,
            TextColor = Color.LightGoldenrodYellow,
            Text = bossName
        };
         _deltaLabel = new Label
        {
            Parent = this,
            Location = new Point(contentLeft, _bossNameLabel.Bottom + 2),
            AutoSizeWidth = true,
            Font = Content.DefaultFont18,
            TextColor = new Color(218, 165, 32), // gold — bold effect via larger font
            Text = $"+{delta}"
        };
        _progressLabel = new Label
        {
            Parent = this,
            Location = new Point(_deltaLabel.Right + 4, _bossNameLabel.Bottom + 4),
            AutoSizeWidth = true,
            Font = Content.DefaultFont14,
            TextColor = Color.LightGoldenrodYellow,
            Text = $"{current} / {max}"
        };

        // Close on any mouse click on the panel or its children.
        Click += OnAnyClick;
        // Auto-dismiss after 5 seconds.
        _ = AutoCloseAfterDelayAsync();
    }

    private void OnAnyClick(object sender, MouseEventArgs e)
    {
        Dispose();
    }

    private async Task AutoCloseAfterDelayAsync()
    {
        try
        {
            await Task.Delay(TimeSpan.FromSeconds(5));

            GameService.Graphics.QueueMainThreadRender(_ =>
            {
                try
                {
                    Dispose();
                }
                catch (Exception ex)
                {
                }
            });
        }
        catch
        {
            // Ignore if the module is unloading or the task is cancelled.
        }
    }

    protected override void DisposeControl()
    {
        Click -= OnAnyClick;
        base.DisposeControl();
    }
}
