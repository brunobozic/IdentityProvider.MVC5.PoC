using System;

namespace Module.CrossCutting.Domain
{
    public interface IFullAudit
    {
        string ModifiedById { get; set; }
        DateTime? ModifiedDate { get; set; }
        string DeletedById { get; set; }
        DateTime? DeletedDate { get; set; }
        string CreatedById { get; set; }
        DateTime? CreatedDate { get; set; }
    }

    public interface IModifyOnlyAudit
    {
        Guid ModifiedById { get; set; }
        DateTime? ModifiedDate { get; set; }
    }

    public interface IDeleteOnlyAudit
    {
        Guid DeletedById { get; set; }
        DateTime? DeletedDate { get; set; }
    }

    public interface ICreateOnlyAudit
    {
        Guid CreatedById { get; set; }
        DateTime? CreatedDate { get; set; }
    }
}