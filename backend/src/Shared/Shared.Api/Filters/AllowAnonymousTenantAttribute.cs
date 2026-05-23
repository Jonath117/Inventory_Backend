namespace Backend.API.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AllowAnonymousTenantAttribute : Attribute
{
}