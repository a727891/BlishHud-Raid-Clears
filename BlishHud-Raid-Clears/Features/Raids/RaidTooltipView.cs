using RaidClears.Utils.Kenedia;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework.Graphics;
using static Blish_HUD.ContentService;
using RaidClears.Localization;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Blish_HUD;
using RaidClears.Features.Raids.Models;
using RaidClears.Features.Strikes.Models;
using RaidClears;

namespace RaidClears.Features.Raids;

public class RaidTooltipView : Blish_HUD.Controls.Tooltip
{
    //private readonly DetailedTexture _image = new(){ TextureRegion = new(14, 14, 100, 100), };
    private readonly Label _title;
    private readonly Label _id;
    private readonly Blish_HUD.Controls.Image _icon;
    private readonly Blish_HUD.Controls.Image _powerIcon;
    private readonly Label _powerLabel;
    private readonly Blish_HUD.Controls.Image _condiIcon;
    private readonly Label _condiLabel;
    private readonly Blish_HUD.Controls.Image _defianceIcon;
    private readonly Label _defianceLabel;
    private readonly Blish_HUD.Controls.Image _mentorIcon;
    private readonly Label _mentorLabel;

    private RaidEncounter _encounter = new();
    private StrikeMission _strikeMission = new();

    public RaidTooltipView()
    {
        // _image.Texture = Service.Textures!.SettingTabFractals;
        // _image.Bounds = imageBounds = new(4, 4, 48, 48);

        _icon = new()
        {
            Parent = this,
            Height = 48,
            Width = 48,
            Texture = ContentService.Textures.Pixel,
            Location = new() { X = 0, Y = 0 }
        };
        _title = new()
        {
            Parent = this,
            Height = Content.DefaultFont16.LineHeight,
            AutoSizeWidth=true,
            Location = new(_icon.Right+5, _icon.Top+5),
            Font = Content.DefaultFont16,
            TextColor = Colors.Chardonnay
        };

        _id = new()
        {
            Parent = this,
            Height = Content.DefaultFont12.LineHeight,
            AutoSizeWidth=true,
            Location = new(_title.Left, _title.Bottom),
            Font = Content.DefaultFont12,
            TextColor = Color.White * 0.8F,
        };

        _powerIcon = new()
        {
            Parent = this,
            Height = 20,
            Width = 20,
            Texture = ContentService.Textures.Pixel,
            Location = new() { X = 0, Y = 0 },
            Visible = false
        };

        _powerLabel = new()
        {
            Parent = this,
            Height = Content.DefaultFont12.LineHeight,
            AutoSizeWidth = true,
            Location = new() { X = 0, Y = 0 },
            Font = Content.DefaultFont12,
            TextColor = Color.White * 0.9F,
            Text = Strings.Tooltip_PowerDamage,
            Visible = false
        };

        _condiIcon = new()
        {
            Parent = this,
            Height = 20,
            Width = 20,
            Texture = ContentService.Textures.Pixel,
            Location = new() { X = 0, Y = 0 },
            Visible = false
        };

        _condiLabel = new()
        {
            Parent = this,
            Height = Content.DefaultFont12.LineHeight,
            AutoSizeWidth = true,
            Location = new() { X = 0, Y = 0 },
            Font = Content.DefaultFont12,
            TextColor = Color.White * 0.9F,
            Text = Strings.Tooltip_ConditionDamage,
            Visible = false
        };

        _defianceIcon = new()
        {
            Parent = this,
            Height = 20,
            Width = 20,
            Texture = ContentService.Textures.Pixel,
            Location = new() { X = 0, Y = 0 },
            Visible = false
        };

        _defianceLabel = new()
        {
            Parent = this,
            Height = Content.DefaultFont12.LineHeight,
            AutoSizeWidth = true,
            Location = new() { X = 0, Y = 0 },
            Font = Content.DefaultFont12,
            TextColor = new Color(57, 172, 161),
            Text = Strings.Tooltip_DefianceBreak,
            Visible = false
        };

        _mentorIcon = new()
        {
            Parent = this,
            Height = 20,
            Width = 20,
            Texture = ContentService.Textures.Pixel,
            Location = new() { X = 0, Y = 0 },
            Visible = false
        };

        _mentorLabel = new()
        {
            Parent = this,
            Height = Content.DefaultFont12.LineHeight,
            AutoSizeWidth = true,
            Location = new() { X = 0, Y = 0 },
            Font = Content.DefaultFont12,
            TextColor = Color.White * 0.85f,
            Visible = false
        };
    }

    public RaidEncounter Encoutner
    {
        get => _encounter; set => Common.SetProperty(ref _encounter, value, ApplyEncounter);
    }
    private void ApplyEncounter(object sender, Utils.Kenedia.ValueChangedEventArgs<RaidEncounter> e)
    {
        if(e.NewValue ==null) { 
            return;
        }
        _title.Text = $"{e.NewValue.Name}";
        _id.Text = $"({Service.RaidSettings.GetEncounterLabel(e.NewValue.ApiId)})";
        if(e.NewValue.AssetId > 0)
        {
            _icon.Texture = Service.Textures!.DatAsset(e.NewValue.AssetId);
        }

        // Set damage type icons (stacked vertically)
        var raidData = Service.RaidData;
        int yOffset = _icon.Bottom + 5;
        int xOffset = _icon.Left;

        if (raidData != null)
        {
            if (e.NewValue.PowerFavored)
            {
                _powerIcon.Texture = Service.Textures!.DatAsset(raidData.PowerDamageAssetId);
                _powerIcon.Location = new(xOffset, yOffset);
                _powerIcon.Visible = true;
                _powerLabel.Location = new(_powerIcon.Right + 5, yOffset);
                _powerLabel.Visible = true;
                yOffset += 25;
            }
            else
            {
                _powerIcon.Visible = false;
                _powerLabel.Visible = false;
            }

            if (e.NewValue.CondiFavored)
            {
                _condiIcon.Texture = Service.Textures!.DatAsset(raidData.CondiDamageAssetId);
                _condiIcon.Location = new(xOffset, yOffset);
                _condiIcon.Visible = true;
                _condiLabel.Location = new(_condiIcon.Right + 5, yOffset);
                _condiLabel.Visible = true;
                yOffset += 25;
            }
            else
            {
                _condiIcon.Visible = false;
                _condiLabel.Visible = false;
            }

            if (e.NewValue.NeedsDefianceBreak && raidData.DefianceAssetId > 0)
            {
                _defianceIcon.Texture = Service.Textures!.DatAsset(raidData.DefianceAssetId);
                _defianceIcon.Location = new(xOffset, yOffset);
                _defianceIcon.Visible = true;
                _defianceLabel.Location = new(_defianceIcon.Right + 5, yOffset);
                _defianceLabel.Visible = true;
                yOffset += 25;
            }
            else
            {
                _defianceIcon.Visible = false;
                _defianceLabel.Visible = false;
            }

            // Mentor achievement progress (final line; icon + text, blank line above if other callouts present) — only when setting enabled
            var mentorEnabled = Service.Settings?.RaidSettings?.RaidPanelMentorProgress?.Value == true;
            var hasOtherCallouts = _powerIcon.Visible || _condiIcon.Visible || _defianceIcon.Visible;
            if (mentorEnabled && e.NewValue.MentorAchievementId is int mentorId)
            {
                if (hasOtherCallouts)
                    yOffset += 12; // blank line above mentor progress

                var progress = Service.MentorAchievementProgress?.Progress;
                _mentorIcon.Visible = raidData.MentorAssetId > 0;
                if (_mentorIcon.Visible)
                {
                    _mentorIcon.Texture = Service.Textures!.DatAsset(raidData.MentorAssetId);
                    _mentorIcon.Location = new(xOffset, yOffset);
                }

                if (progress != null && progress.TryGetValue(mentorId, out var entry))
                {
                    _mentorLabel.Text = entry.Done
                        ? Strings.Tooltip_MentorDone
                        : string.Format(Strings.Tooltip_MentorProgress, entry.Current, entry.Max);
                    _mentorLabel.Location = _mentorIcon.Visible
                        ? new(_mentorIcon.Right + 5, yOffset)
                        : new(xOffset + 5, yOffset); // +5 left padding to avoid clipping
                    _mentorLabel.Visible = true;
                }
                else
                {
                    _mentorLabel.Text = string.Format(Strings.Tooltip_MentorProgress, 0, "?");
                    _mentorLabel.Location = _mentorIcon.Visible
                        ? new(_mentorIcon.Right + 5, yOffset)
                        : new(xOffset + 5, yOffset);
                    _mentorLabel.Visible = true;
                }

                yOffset += 25;
            }
            else
            {
                _mentorIcon.Visible = false;
                _mentorLabel.Visible = false;
            }
        }
        else
        {
            _powerIcon.Visible = false;
            _powerLabel.Visible = false;
            _condiIcon.Visible = false;
            _condiLabel.Visible = false;
            _defianceIcon.Visible = false;
            _defianceLabel.Visible = false;
            _mentorIcon.Visible = false;
            _mentorLabel.Visible = false;
        }

        Invalidate();
    }

    public StrikeMission StrikeMission
    {
        get => _strikeMission; set => Common.SetProperty(ref _strikeMission, value, ApplyStrikeMission);
    }
    private void ApplyStrikeMission(object sender, Utils.Kenedia.ValueChangedEventArgs<StrikeMission> e)
    {
        if (e.NewValue == null)
        {
            return;
        }
        _title.Text = $"{e.NewValue.Name}";
        _id.Text = $"({Service.StrikeSettings.GetEncounterLabel(e.NewValue.Id)})";
        if (e.NewValue.AssetId > 0)
        {
            _icon.Texture = Service.Textures!.DatAsset(e.NewValue.AssetId);
        }
    }

    public override void Draw(SpriteBatch spriteBatch, Rectangle drawBounds, Rectangle scissor)
    {
        //if (Skill == null) return;
        base.Draw(spriteBatch, drawBounds, scissor);
    }

    public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
    {
        base.PaintBeforeChildren(spriteBatch, bounds);

        //_image.Draw(this, spriteBatch);
        
    }

    public override void RecalculateLayout()
    {
        int height = 58;
        int contentHeight = 48;

        // Damage type indicators and defiance break (stacked vertically)
        int indicatorCount = 0;
        if (_powerIcon != null && _powerIcon.Visible) indicatorCount++;
        if (_condiIcon != null && _condiIcon.Visible) indicatorCount++;
        if (_defianceIcon != null && _defianceIcon.Visible) indicatorCount++;

        if (indicatorCount > 0)
        {
            height += indicatorCount * 25;
            contentHeight += indicatorCount * 25;
        }

        // Mentor progress line (with blank line above if other callouts present)
        if (_mentorIcon != null && _mentorIcon.Visible)
        {
            if (indicatorCount > 0)
            {
                height += 12;
                contentHeight += 12;
            }
            height += 25;
            contentHeight += 25;
        }

        base.Size = new(230, height);
        base.ContentRegion = new Rectangle(5, 5, 220, contentHeight);
    }

    protected override void DisposeControl()
    {
        //Skill = null;
        //_image.Texture = null;

        base.DisposeControl();
    }
}
