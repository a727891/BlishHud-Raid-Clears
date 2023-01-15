using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using RaidClears.Features.Shared.Controls;

namespace RaidClears.Features.Raids.Models
{
    public class Encounter : BoxModel
    {
        public Encounter(string id, string name, string short_name) : base(id, name, short_name)
        {
        }
    }
}
