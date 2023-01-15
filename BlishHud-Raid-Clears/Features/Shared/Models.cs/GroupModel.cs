
using RaidClears.Features.Shared.Controls;
using System.Net.NetworkInformation;

namespace RaidClears.Features.Raids.Models
{
    internal class GroupModel
    {
        public string name;
        public int index;
        public string shortName;
        public BoxModel[] boxes;
        public bool highlightColor;

        public GroupModel(string name, int index, string shortName, GridBox label,  BoxModel[] boxes)
        {
            this.name = name;
            this.index = index;
            this.shortName = shortName;
            this.boxes = boxes;

            this.highlightColor = false;
        }


    }
}
