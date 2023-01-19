using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace RaidClears.Utils;

public static class ContainerExtensions
{
    public static Control AddControl(this Container container, Control control, out Control generatedControl)
    {
        control.Location += new Point(0, 25);
        
        container.AddChild(control);
        generatedControl = control;
        return control;
    }
    
    public static Control AddControl(this Container container, Control control)
    {        
        control.Location += new Point(0, 25);
        
        container.AddChild(control);
        return control;
    }
}