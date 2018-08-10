using System;

namespace IdentityProvider.Models
{
    [Flags]
    public enum Operations
    {
        None = 0,
        Create = 1,
        Read = 2,
        Update = 4,
        Delete = 8,
        Execute = 16,
        All = 31
    }
}