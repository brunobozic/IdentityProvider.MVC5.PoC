using System;

namespace IdentityProvider.Infrastructure.Domain
{
    public interface IFullAudit
    {
        Guid ModifiedById { get; set; }
        DateTime? ModifiedDate { get; set; }
        Guid DeletedById { get; set; }
        DateTime? DeletedDate { get; set; }
        Guid CreatedById { get; set; }
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