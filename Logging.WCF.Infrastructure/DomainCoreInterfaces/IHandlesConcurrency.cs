namespace Logging.WCF.Infrastructure.DomainCoreInterfaces
{
    public interface IHandlesConcurrency
    {
        byte[] RowVersion { get; set; }
    }
}