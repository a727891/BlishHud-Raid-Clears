using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework.Graphics;
using static Blish_HUD.ContentService;
using RaidClears.Localization;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace RaidClears.Features.Shared.Services;

public class CornerIconTooltipView : Blish_HUD.Controls.Tooltip
{
    private readonly Blish_HUD.Controls.Image _emblemIcon;
    private readonly Label _moduleName;
    private readonly Label _accountName;
    private readonly Label _motdMessage;

    public CornerIconTooltipView()
    {
        _emblemIcon = new()
        {
            Parent = this,
            Height = 48,
            Width = 48,
            Texture = Service.Textures?.SettingWindowEmblem ?? ContentService.Textures.Pixel,
            Location = new() { X = 0, Y = 0 }
        };

        _moduleName = new()
        {
            Parent = this,
            Height = Content.DefaultFont16.LineHeight,
            AutoSizeWidth = true,
            Location = new(_emblemIcon.Right + 5, _emblemIcon.Top + 5),
            Font = Content.DefaultFont16,
            TextColor = Colors.Chardonnay,
            Text = Strings.Module_Title
        };

        _accountName = new()
        {
            Parent = this,
            Height = Content.DefaultFont14.LineHeight,
            AutoSizeWidth = true,
            Location = new(_moduleName.Left, _moduleName.Bottom + 2),
            Font = Content.DefaultFont14,
            TextColor = Color.White * 0.8F,
            Text = $"Account: {Service.CurrentAccountName}"
        };

        _motdMessage = new()
        {
            Parent = this,
            AutoSizeHeight = true,
            Width = 220,
            Location = new(_emblemIcon.Left, _emblemIcon.Bottom + 10),
            Font = Content.DefaultFont14,
            TextColor = Color.White * 0.9F,
            WrapText = true,
            Visible = false
        };
    }

    public string? MotdMessage
    {
        get => _motdMessage.Text;
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                _motdMessage.Text = value;
                _motdMessage.Visible = true;
            }
            else
            {
                _motdMessage.Visible = false;
            }
            Invalidate();
        }
    }

    public void UpdateAccountName(string accountName)
    {
        _accountName.Text = $"Account: {accountName}";
        Invalidate();
    }

    public override void RecalculateLayout()
    {
        int height = 58; // Base height for icon + padding
        int contentWidth = 220;

        // If MOTD is visible, add its height
        if (_motdMessage != null && _motdMessage.Visible && !string.IsNullOrEmpty(_motdMessage.Text))
        {
            // Set the label width first so it can calculate its wrapped height
            _motdMessage.Width = contentWidth;
            _motdMessage.RecalculateLayout();
            
            // Label with WrapText and AutoSizeHeight will calculate its own height
            // Add the label's height plus padding (use LineHeight as fallback if height is 0)
            var motdHeight = _motdMessage.Height > 0 ? _motdMessage.Height : Content.DefaultFont14.LineHeight;
            height += motdHeight + 10;
        }

        base.Size = new(contentWidth + 10, height);
        base.ContentRegion = new Rectangle(5, 5, contentWidth, height - 10);
    }

    protected override void DisposeControl()
    {
        base.DisposeControl();
    }
}
