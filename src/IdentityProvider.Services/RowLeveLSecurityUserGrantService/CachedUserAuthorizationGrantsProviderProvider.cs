using IdentityProvider.Infrastructure;
using IdentityProvider.Infrastructure.Caching;

namespace IdentityProvider.Services.RowLeveLSecurityUserGrantService
{
    public class CachedUserAuthorizationGrantsProvider : ICachedUserAuthorizationGrantsProvider
    {
        private readonly IMemoryCacheProvider _memoryCacheAdapter;
        private readonly IUserGrantService _userGrantService;

        public CachedUserAuthorizationGrantsProvider(
            IMemoryCacheProvider memoryCacheAdapter
            , IUserGrantService userGrantService
        )
        {
            _memoryCacheAdapter = memoryCacheAdapter;
            _userGrantService = userGrantService;
        }

        public void FetchUsersGrantsByUser(string userId)
        {
            var response = _userGrantService.OrgUnitGrantedPriviligesByUser(userId);
        }

        public void FetchUsersGrantsByEmployee( int employeeId )
        {
            var response = _userGrantService.OrgUnitGrantedPriviligesByEmployee(employeeId);
        }



        public int[] OrganizationalUnits { get; set; }
        public int[] ExplicitlyAssignedToProjects { get; set; }
    }
}
