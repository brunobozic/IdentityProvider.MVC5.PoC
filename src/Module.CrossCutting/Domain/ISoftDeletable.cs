namespace Module.CrossCutting.Domain
{
    public interface ISoftDeletable
    {
        bool IsDeleted { get; set; }
    }
}