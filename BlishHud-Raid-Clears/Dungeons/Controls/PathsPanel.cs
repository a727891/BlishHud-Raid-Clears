using Blish_HUD;
using Blish_HUD.Controls;
using RaidClears.Dungeons.Model;
using Microsoft.Xna.Framework;
using Settings.Enums;

namespace RaidClears.Dungeons.Controls
{
    public class PathsPanel : FlowPanel
    {
        private Model.Dungeon _dungeon;

        private DungeonOrientation _orientation;
        private DungeonLabel _labelDisplay;


        private Label _dungeonLabel;




        public PathsPanel(Container parent, Model.Dungeon dungeon, DungeonOrientation orientation, DungeonLabel label, ContentService.FontSize fontSize)
        {
            _dungeon = dungeon;

            ControlPadding = new Vector2(2, 2);
            HeightSizingMode = SizingMode.AutoSize;
            Parent = parent;
            WidthSizingMode = SizingMode.AutoSize;

            _dungeonLabel = new Label()
            {
                AutoSizeHeight = true,
                BasicTooltipText = dungeon.GetTooltip(),
                HorizontalAlignment = DungeonLabelAlignment(),
                Parent = this,
                Text = GetWingLabelText()
            };
            foreach(var path in _dungeon.paths)
            {
                var pathLabel = new Label()
                {
                    AutoSizeHeight = true,
                    BasicTooltipText = path.name,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Parent = this,
                    Text = path.short_name,
                };
                path.SetLabelReference(pathLabel);
            }

            SetOrientation(orientation);
            SetLabelDisplay(label);
            SetFontSize(fontSize);
        }

        public string GetWingLabelText()
        {
            switch (_labelDisplay)
            {
                case DungeonLabel.NoLabel: return "";
                case DungeonLabel.Abbreviation: return _dungeon.shortName;
                default: return "-";
            }
            
        }

        private HorizontalAlignment DungeonLabelAlignment()
        {
            if (_orientation == DungeonOrientation.Vertical)
            {
                return HorizontalAlignment.Right;
            }

            return HorizontalAlignment.Center;

        }

        public void SetOrientation(DungeonOrientation orientation)
        {
           /**  FlowDirection based on WingOrientation 
            * V L>R          H   1  2  3
            * 1 B1 B2 B3     T   B1 B1 B1
            * 2 B1 B2 B3     v   B2 B2 B2
            * 3 B1 B2 B3     B   B3 B3 B3
            */
            _orientation = orientation;
            FlowDirection = GetFlowDirection(orientation);
            _dungeonLabel.HorizontalAlignment = DungeonLabelAlignment();
        }

        private ControlFlowDirection GetFlowDirection(DungeonOrientation orientation)
        {
            switch (orientation)
            {
                case DungeonOrientation.Horizontal: return ControlFlowDirection.SingleTopToBottom;
                case DungeonOrientation.Vertical: return ControlFlowDirection.SingleLeftToRight;
                case DungeonOrientation.SingleRow: return ControlFlowDirection.SingleLeftToRight;
                default: return ControlFlowDirection.SingleLeftToRight;
            }
        }

        public void SetLabelDisplay(DungeonLabel label)
        {
            _labelDisplay = label;
            _dungeonLabel.Text = GetWingLabelText();
            _dungeonLabel.HorizontalAlignment = DungeonLabelAlignment();

            if(label == DungeonLabel.NoLabel)
            {
                _dungeonLabel.Hide();
            }
            else
            {
                _dungeonLabel.Show();
            }
            Invalidate();
        }

        public void SetFontSize(ContentService.FontSize fontSize)
        {
            var font = GameService
                .Content
                .GetFont(
                ContentService.FontFace.Menomonia,
                fontSize,
                ContentService.FontStyle.Regular
               );
            var width = GetLabelWidthForFontSize(fontSize);

            _dungeonLabel.Font = font;
            _dungeonLabel.Width = width;
            foreach(var encounter in _dungeon.paths)
            {
                encounter.GetLabelReference().Font = font;
                encounter.GetLabelReference().Width = width;
            }
        }

        public int GetLabelWidthForFontSize(ContentService.FontSize size)
        {
            switch (size)
            {
                //case ContentService.FontSize.Size36:
                case ContentService.FontSize.Size34:
                case ContentService.FontSize.Size32:
                    return  80;
                case ContentService.FontSize.Size24:
                //case ContentService.FontSize.Size22:
                case ContentService.FontSize.Size20:
                    return  50;
                //case ContentService.FontSize.Size18:
                case ContentService.FontSize.Size16:
                case ContentService.FontSize.Size14:
                    return  40;
                //case ContentService.FontSize.Size12:
                case ContentService.FontSize.Size11:
                    return 35;
                case ContentService.FontSize.Size8:
                    return 39;
                default: return 40;
            }
        }

        public void SetWingLabelOpacity(float opacity)
        {
            _dungeonLabel.Opacity = opacity;
        }

        public void SetEncounterOpacity(float opacity)
        {
            foreach(var encounter in _dungeon.paths)
            {
                encounter.GetLabelReference().Opacity = opacity;
            }
        }
        public void ShowHide(bool shouldShow)
        {
            if(Visible==true && !shouldShow)
            {
                Hide();
            }
            if(Visible == false && shouldShow)
            {
                Show();
            }
        }

    }
}
