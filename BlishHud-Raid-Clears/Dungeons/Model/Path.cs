﻿using System.Collections.Generic;
using Blish_HUD.Controls;
using System.Linq;

namespace RaidClears.Dungeons.Model
{
    public class Path
    {
        public string id;
        public string name;
        public string short_name;
        public bool is_cleared = false;

        private Label _label;

        private Microsoft.Xna.Framework.Color ColorUnknown = new Microsoft.Xna.Framework.Color(64, 64, 64, 196);
        private Microsoft.Xna.Framework.Color ColorNotCleared = new Microsoft.Xna.Framework.Color(120, 20, 20, 196);
        private Microsoft.Xna.Framework.Color ColorCleared = new Microsoft.Xna.Framework.Color(20, 120, 20, 196);


        public Path(string id, string name, string short_name)
        {
            this.id = id;
            this.name = name;
            this.short_name = short_name;
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
        }

        public void SetFrequenter(bool done)
        {
            _label.TextColor = done ? Microsoft.Xna.Framework.Color.Yellow : Microsoft.Xna.Framework.Color.White;
            //_label.TextColor = Microsoft.Xna.Framework.Color.White;
            //_label.TextColor = Microsoft.Xna.Framework.Color.Yellow;
        }

    }
}