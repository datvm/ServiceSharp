namespace ServiceSharp;


[AttributeUsage(
    AttributeTargets.Class |
    AttributeTargets.Interface |
    AttributeTargets.Struct)]
public class IgnoreAttribute : Attribute
{
}
