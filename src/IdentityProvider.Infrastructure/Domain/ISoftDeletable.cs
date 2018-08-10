namespace IdentityProvider.Infrastructure.Domain
{
    public interface ISoftDeletable
    {
        bool IsDeleted { get; set; }
    }
}