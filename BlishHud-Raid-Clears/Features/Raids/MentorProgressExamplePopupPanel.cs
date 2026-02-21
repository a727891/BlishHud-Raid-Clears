using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using RaidClears;
using RaidClears.Localization;
using static Blish_HUD.ContentService;

namespace RaidClears.Features.Raids;

/// <summary>
/// Draggable example popup used to set the position for mentor progress popups. Shown when "Reposition mentor progress popup" is enabled.
/// </summary>
public class MentorProgressExamplePopupPanel : Panel
{
    private const int IconSize = 48;
    private const int ContentPadding = 8;
    private bool _isDragging;
    private Point _dragStart;

    public MentorProgressExamplePopupPanel() : base()
    {
        Size = new Point(300, 72);
        BackgroundTexture = Service.Textures?.DatAsset(156112);
        ShowBorder = false;
        ShowTint = false;

        var icon = new Image
        {
            Parent = this,
            Size = new Point(IconSize, IconSize),
            Location = new Point(ContentPadding, (Height - IconSize) / 2),
            Texture = Service.Textures?.DatAsset(1203237) ?? Textures.Pixel
        };

        int contentLeft = ContentPadding + IconSize + ContentPadding;
        var bossNameLabel = new Label
        {
            Parent = this,
            Location = new Point(contentLeft, ContentPadding),
            AutoSizeWidth = true,
            AutoSizeHeight = true,
            Font = Content.DefaultFont16,
            TextColor = Color.LightGoldenrodYellow,
            Text = Strings.MentorProgress_ExamplePopup_Title
        };
        var deltaLabel = new Label
        {
            Parent = this,
            Location = new Point(contentLeft, bossNameLabel.Bottom + 2),
            AutoSizeWidth = true,
            Font = Content.DefaultFont18,
            TextColor = new Color(218, 165, 32),
            Text = "+1"
        };
        var progressLabel = new Label
        {
            Parent = this,
            Location = new Point(deltaLabel.Right + 4, bossNameLabel.Bottom + 4),
            AutoSizeWidth = true,
            Font = Content.DefaultFont14,
            TextColor = Color.LightGoldenrodYellow,
            Text = "99 / 1000"
        };

        LeftMouseButtonPressed += (_, _) =>
        {
            _isDragging = true;
            _dragStart = GameService.Input.Mouse.Position;
        };
        LeftMouseButtonReleased += (_, _) => _isDragging = false;
    }

    /// <summary>
    /// Call from Module.Update when this panel is visible so dragging works (Container.DoUpdate is sealed).
    /// </summary>
    public void UpdateDrag()
    {
        if (!_isDragging)
            return;
        var current = GameService.Input.Mouse.Position;
        var delta = current - _dragStart;
        Location += delta;
        _dragStart = current;
    }
}
