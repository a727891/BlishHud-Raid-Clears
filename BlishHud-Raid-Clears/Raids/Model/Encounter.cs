using System.Collections.Generic;
using Blish_HUD.Controls;
using System.Linq;
using Microsoft.Xna.Framework;

namespace RaidClears.Raids.Model
{
    public class Encounter
    {
        public string id;
        public string name;
        public string short_name;
        public bool is_cleared = false;

        private Label _label;

        private Color ColorUnknown = new Color(64, 64, 64);
        private Color ColorNotCleared = new Color(120, 20, 20);
        private Color ColorCleared = new Color(20, 120, 20);


        public Encounter(string id, string name, string short_name)
        {
            this.id = id;
            this.name = name;
            this.short_name = short_name;
        }

        public void SetClearColors(Color cleared, Color notCleared)
        {
            ColorCleared = cleared;
            ColorNotCleared = notCleared;

        }

        public void UpdateColors(Color cleared, Color notCleared)
        {
            ColorNotCleared = notCleared;
            ColorCleared = cleared;

            _label.BackgroundColor = is_cleared ? ColorCleared : ColorNotCleared;

        }

        public void SetLabelReference(Label label)
        {
            _label = label;
            _label.BackgroundColor = ColorUnknown;
        }

        public Label GetLabelReference()
        {
            return _label;
        }



        public void SetCleared(bool cleared)
        {
            _label.BackgroundColor = cleared ? ColorCleared : ColorNotCleared;
            is_cleared = cleared;
        }

    }
}