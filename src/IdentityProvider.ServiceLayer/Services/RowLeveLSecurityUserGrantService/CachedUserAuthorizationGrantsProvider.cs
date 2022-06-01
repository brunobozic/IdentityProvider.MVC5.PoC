using Module.CrossCutting;
using Module.CrossCutting.Caching;

namespace IdentityProvider.ServiceLayer.Services.RowLeveLSecurityUserGrantService
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

        public int[] OrganizationalUnits { get; set; }
        public int[] ExplicitlyAssignedToProjects { get; set; }

        public void FetchUsersGrantsByUser(string userId)
        {
            var response = _userGrantService.OrgUnitGrantedPriviligesByUser(userId);
        }

        public void FetchUsersGrantsByEmployee(int employeeId)
        {
            var response = _userGrantService.OrgUnitGrantedPriviligesByEmployee(employeeId);
        }
    }
}