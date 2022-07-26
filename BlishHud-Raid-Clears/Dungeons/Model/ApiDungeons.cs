using System.Collections.Generic;
using Gw2Sharp.WebApi.V2.Models;
using System.Linq;

namespace RaidClears.Dungeons.Model
{
    public class ApiDungeons
    {
        public ApiDungeons()
        {

        }
        public ApiDungeons(List<string> clears)
        {
            Clears = clears;
        }

        public List<string> Clears { get; } = new List<string>();
    }
}