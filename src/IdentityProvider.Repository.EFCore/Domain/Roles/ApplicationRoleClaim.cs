using IdentityProvider.Repository.EFCore.Domain.Account;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityProvider.Repository.EFCore.Domain.Roles
{
    public class AppUserRole : IdentityUserRole<string>
    {
        public virtual ApplicationUser User { get; set; }
        public virtual AppRole Role { get; set; }
    }

    public class AppUserClaim : IdentityUserClaim<string>
    {
        public virtual ApplicationUser User { get; set; }
    }

    public class AppUserLogin : IdentityUserLogin<string>
    {
        public virtual ApplicationUser User { get; set; }
    }

    public class AppRoleClaim : IdentityRoleClaim<string>
    {
        public virtual AppRole Role { get; set; }
    }

    public class AppUserToken : IdentityUserToken<string>
    {
        public virtual ApplicationUser User { get; set; }
    }
}
