namespace Module.CrossCutting
{
    public interface ICachedUserAuthorizationGrantsProvider
    {
        int[] OrganizationalUnits { get; set; }
        int[] ExplicitlyAssignedToProjects { get; set; }
    }
}