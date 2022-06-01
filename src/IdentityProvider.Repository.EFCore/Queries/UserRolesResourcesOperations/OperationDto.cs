namespace IdentityProvider.Repository.EFCore.Queries.UserRolesResourcesOperations
{
    public class OperationDto
    {
        public string Name { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }
        public string Description { get; set; }
    }
}