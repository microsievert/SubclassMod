using SubclassMod.Enums;

namespace SubclassMod.Patterns
{
    internal interface INamedRole
    {
      string NamePrefix { get; set; }  
      string NamePostfix { get; set; }
      
      NamingMethod NamingMethod { get; set; }
    }
}