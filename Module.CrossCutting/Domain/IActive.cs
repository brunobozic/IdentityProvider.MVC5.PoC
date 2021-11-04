using System;

namespace IdentityProvider.Infrastructure.Domain
{
    public interface IActive
    {
        bool Active { get; set; }
        DateTime? ActiveFrom { get; set; }
        DateTime? ActiveTo { get; set; }
    }
}