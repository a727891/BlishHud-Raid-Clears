using System.Collections.Generic;
using Blish_HUD.Controls;
using System.Linq;

namespace RaidClears.Raids.Model
{
    public class Encounter
    {
        public string id;
        public string name;
        public string short_name;
        public bool is_cleared = false;

        private Label _label;

        public Encounter(string id, string name, string short_name)
        {
            this.id = id;
            this.name = name;
            this.short_name = short_name;
        }

        public void SetLabelReference(Label label)
        {
            _label = label;
        }

        public Label GetLabelReference()
        {
            return _label;
        }
    }
}