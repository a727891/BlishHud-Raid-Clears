using System.Collections.Generic;
using System.Linq;

namespace GatheringTools.Raids.Model
{
    public class Wing
    {
        public string name;
        public int index;
        public string shortName;
        public Encounter[] encounters;

        public Wing(string name, int index, string shortName, Encounter[] encounters)
        {
            this.name = name;
            this.index = index;
            this.shortName = shortName;
            this.encounters = encounters;
        }
    }
}