using System;

namespace IdentityProvider.Models.Domain.Account
{
    public interface IActive
    {
        bool Active { get; set; }
        DateTime? ActiveFrom { get; set; }
        DateTime? ActiveTo { get; set; }
    }
}