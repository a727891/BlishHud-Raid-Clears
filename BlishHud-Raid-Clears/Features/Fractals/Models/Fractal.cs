using System.Collections.Generic;
using RaidClears.Features.Raids.Models;
using RaidClears.Features.Shared.Models;

namespace RaidClears.Features.Fractals.Models;

public class Fractal : GroupModel
{
    public Fractal(string name, int index, string shortName, IEnumerable<BoxModel> boxes) : base(name, shortName, index, shortName, boxes)
    {

    }
    public virtual void Dispose()
    {

    }
}
