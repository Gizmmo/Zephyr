
namespace Zephyr.BuildOps.SceneCompiler
{
    // Multiuse attribute.
    [System.AttributeUsage(System.AttributeTargets.Class |
        System.AttributeTargets.Struct,
        AllowMultiple = true)  // Multiuse attribute.
]
    public class OnPreBuild : System.Attribute
    {
    
    }
}
