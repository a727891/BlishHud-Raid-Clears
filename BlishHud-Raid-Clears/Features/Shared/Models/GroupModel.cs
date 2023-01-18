
using RaidClears.Features.Shared.Controls;

namespace RaidClears.Features.Shared.Models;

public class GroupModel
{
    public string name;
    public int index;
    public string shortName;
    public BoxModel[] boxes;
    public bool highlightColor;

    public GridGroup GridGroup { get; private set; }
    public GridBox GroupLabel { get; private set; }

    public GroupModel(string name, int index, string shortName, BoxModel[] boxes)
    {
        this.name = name;
        this.index = index;
        this.shortName = shortName;
        this.boxes = boxes;

        highlightColor = false;
    }


    public void SetGridGroupReference(GridGroup group)
    {
       GridGroup = group;

    }
    public void SetGroupLabelReference(GridBox box)
    {
        GroupLabel = box;
    }

}
