using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using RaidClears.Features.Shared.Controls;

namespace RaidClears.Features.Raids.Models
{
    public class BoxModel
    {
        public string id;
        public string name;
        public string short_name;
        public bool is_cleared = false;

        public GridBox Box { get; private set; }

        private Color ColorUnknown = new Color(64, 64, 64);
        private Color ColorNotCleared = new Color(120, 20, 20);
        private Color ColorCleared = new Color(20, 120, 20);

        public BoxModel(string id, string name, string short_name)
        {
            this.id = id;
            this.name = name;
            this.short_name = short_name;
        }

        public void SetClearColors(Color cleared, Color notCleared)
        {
            ColorCleared = cleared;
            ColorNotCleared = notCleared;

            Box.BackgroundColor = is_cleared ? ColorCleared : ColorNotCleared;
            Box.Invalidate();
        }

        public void SetGridBoxReference(GridBox box)
        {
            Box = box;
            Box.BackgroundColor = ColorUnknown;
        }


        public void SetCleared(bool cleared)
        {
            Box.BackgroundColor = cleared ? ColorCleared : ColorNotCleared;
            is_cleared = cleared;
        }

    }
}
