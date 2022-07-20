using System.Collections.Generic;
using System.Linq;

namespace GatheringTools.Raids.Model
{
    public class Encounter
    {
        public string id;
        public string name;
        public string short_name;
        public bool is_cleared = false;

        public Encounter(string id, string name, string short_name)
        {
            this.id = id;
            this.name = name;
            this.short_name = short_name;
        }
    }
}