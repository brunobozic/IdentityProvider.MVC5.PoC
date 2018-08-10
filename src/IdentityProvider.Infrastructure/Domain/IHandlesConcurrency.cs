namespace IdentityProvider.Infrastructure.Domain
{
    public interface IHandlesConcurrency
    {
        byte[] RowVersion { get; set; }
    }
}