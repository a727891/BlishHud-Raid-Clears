﻿using Blish_HUD;
using Blish_HUD.Controls;
using RaidClears.Raids.Model;
using Microsoft.Xna.Framework;
using Settings.Enums;

namespace RaidClears.Raids.Controls
{
    public class WingPanel : FlowPanel
    {
        private Wing _wing;

        private Orientation _orientation;
        private WingLabel _labelDisplay;


        private Label _wingLabelObj;

        public WingPanel(Container parent, Wing wing, Orientation orientation, WingLabel label, ContentService.FontSize fontSize)
        {
            _wing = wing;

            ControlPadding = new Vector2(2, 2);
            HeightSizingMode = SizingMode.AutoSize;
            Parent = parent;
            WidthSizingMode = SizingMode.AutoSize;

            _wingLabelObj = new Label()
            {
                AutoSizeHeight = true,
                BasicTooltipText = GetWingTooltip(),
                HorizontalAlignment = WingLabelAlignment(),
                //Opacity = (float)1f,
                Parent = this,
                Text = GetWingLabelText()
            };
            foreach(var encounter in _wing.encounters)
            {
                var encounterLabel = new Label()
                {
                    AutoSizeHeight = true,
                    //BackgroundColor = GetClearedState(encounter.id),
                    BasicTooltipText = encounter.name,
                    /*Font = GameService
                        .Content
                        .GetFont(ContentService.FontFace.Menomonia,
                        _settingFontSize.Value,
                        ContentService.FontStyle.Regular),*/
                    HorizontalAlignment = HorizontalAlignment.Center,
                    //Opacity = (float)1f,// _settingEncounterLabelOpacity.Value / MAX_SLIDER_INT,
                    Parent = this,
                    Text = encounter.short_name
                };
                encounter.SetLabelReference(encounterLabel);
            }

            SetOrientation(orientation);
            SetLabelDisplay(label);
            SetFontSize(fontSize);
        }

        public string GetWingTooltip()
        {
            if (_wing.isCallOfTheMist)
            {
                return "(Call of the Mists) " + _wing.name;
            }
            if (_wing.isEmboldened)
            {
                return "(Emboldened) " + _wing.name;
            }

            return _wing.name;
        }
        public string GetWingLabelText()
        {
            switch (_labelDisplay)
            {
                case WingLabel.NoLabel: return "";
                case WingLabel.WingNumber: return _wing.index.ToString();
                case WingLabel.Abbreviation: return _wing.shortName;
                default: return "-";
            }
            
        }

        private HorizontalAlignment WingLabelAlignment()
        {
            if (_orientation == Orientation.Vertical)
            {
                if (_labelDisplay == WingLabel.WingNumber)
                {
                    return HorizontalAlignment.Center;
                }
                else
                {
                    return HorizontalAlignment.Right;
                }
            }

            return HorizontalAlignment.Center;

        }

        public void SetOrientation(Orientation orientation)
        {
           /**  FlowDirection based on WingOrientation 
            * V L>R          H   1  2  3
            * 1 B1 B2 B3     T   B1 B1 B1
            * 2 B1 B2 B3     v   B2 B2 B2
            * 3 B1 B2 B3     B   B3 B3 B3
            */
            _orientation = orientation;
            FlowDirection = GetFlowDirection(orientation);
            _wingLabelObj.HorizontalAlignment = WingLabelAlignment();
        }

        private ControlFlowDirection GetFlowDirection(Orientation orientation)
        {
            switch (orientation)
            {
                case Orientation.Horizontal: return ControlFlowDirection.SingleTopToBottom;
                case Orientation.Vertical: return ControlFlowDirection.SingleLeftToRight;
                case Orientation.SingleRow: return ControlFlowDirection.SingleLeftToRight;
                default: return ControlFlowDirection.SingleLeftToRight;
            }
        }

        public void SetLabelDisplay(WingLabel label)
        {
            _labelDisplay = label;
            _wingLabelObj.Text = GetWingLabelText();
            _wingLabelObj.HorizontalAlignment = WingLabelAlignment();

            if(label == WingLabel.NoLabel)
            {
                _wingLabelObj.Hide();
            }
            else
            {
                _wingLabelObj.Show();
            }
            Invalidate();
        }

        public void SetHighlightColor(Color color)
        {
            this.Children.ToList().ForEach(elem =>
            {
                if (elem is Label label)
                {
                    label.TextColor = color;
                }
            });
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

            _wingLabelObj.Font = font;
            _wingLabelObj.Width = width;
            foreach(var encounter in _wing.encounters)
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
            _wingLabelObj.Opacity = opacity;
        }

        public void SetEncounterOpacity(float opacity)
        {
            foreach(var encounter in _wing.encounters)
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
