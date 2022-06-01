namespace Module.CrossCutting.Domain
{
    public interface IHandlesConcurrency
    {
        byte[] RowVersion { get; set; }
    }
}