﻿using System.Collections.Generic;
using Gw2Sharp.WebApi.V2.Models;
using System.Linq;

namespace RaidClears.Raids.Model
{
    public class ApiRaids
    {
        public ApiRaids()
        {

        }
        public ApiRaids(List<string> clears)
        {
            Clears = clears;
        }

        public List<string> Clears { get; } = new List<string>();
    }
}