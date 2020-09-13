namespace Logging.WCF.Repository.EF.DomainCoreInterfaces
{
    public interface IHandlesConcurrency
    {
        byte[] RowVersion { get; set; }
    }
}