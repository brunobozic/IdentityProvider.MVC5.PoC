using IdentityProvider.Repository.EFCore.Domain.Account;
using IdentityProvider.Repository.EFCore.Domain.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityProvider.Web.MVC6.Identity
{
    public class AdditionalUserClaimsPrincipalFactory
          : UserClaimsPrincipalFactory<ApplicationUser, IdentityFrameworkRole>
    {
        public AdditionalUserClaimsPrincipalFactory(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityFrameworkRole> roleManager,
            IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, roleManager, optionsAccessor)
        { }

        public override async Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
        {
            var principal = await base.CreateAsync(user);
            var identity = (ClaimsIdentity)principal.Identity;

            var claims = new List<Claim>();

            //if (user.Employee.Roles.Any(ou => ou.Name == "Developer"))
            //{
            //	claims.Add(new Claim(JwtClaimTypes.Role, "SuperUser"));
            //}

            if (user.Email == "bruno.bozic@gmail.com")
            {
                claims.Add(new Claim(JwtClaimTypes.Role, "SuperUser"));
            }

            //if (user.Employee.OrganizationalUnits.Any(ou=>ou.OrganizationalUnit.Name=="CEO Office"))
            //{
            //	claims.Add(new Claim(JwtClaimTypes.Role, "CEO"));
            //}
            //else if (user.Employee.OrganizationalUnits.Any(ou => ou.OrganizationalUnit.Name == "Developer"))
            //{
            //	claims.Add(new Claim(JwtClaimTypes.Role, "SuperUser"));
            //}

            identity.AddClaims(claims);
            return principal;
        }
    }
}