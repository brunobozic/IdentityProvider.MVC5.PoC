namespace IdentityProvider.Infrastructure
{
    public interface ICachedUserAuthorizationGrantsProvider
    {
       
        int[] OrganizationalUnits { get; set; }
        int[] ExplicitlyAssignedToProjects { get; set; }
    }
}