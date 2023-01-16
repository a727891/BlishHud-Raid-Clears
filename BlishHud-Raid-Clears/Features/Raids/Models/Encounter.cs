using RaidClears.Features.Shared.Models;

namespace RaidClears.Features.Raids.Models
{
    public class Encounter : BoxModel
    {
        public Encounter(string id, string name, string short_name) : base(id, name, short_name)
        {
        }
    }
}
