using Blish_HUD.Controls;

namespace RaidClears.Utils;

public static class ContainerExtensions
{
    public static Control AddControl(this Container container, Control control, out Control generatedControl)
    {
        control.Parent = container;
        
        container.AddChild(control);
        generatedControl = control;
        return control;
    }
    
    public static Control AddControl(this Container container, Control control)
    {
        control.Parent = container;
        
        container.AddChild(control);
        return control;
    }
}