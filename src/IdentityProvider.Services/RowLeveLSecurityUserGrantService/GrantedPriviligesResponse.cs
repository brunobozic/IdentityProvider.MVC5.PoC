using IdentityProvider.Repository.EF.Queries.UserGrants;
using System.Collections.Generic;

namespace IdentityProvider.Services.RowLeveLSecurityUserGrantService
{
    public class GrantedPriviligesResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<GrantedPriviligesDto> GrantedPriviliges { get; set; }

        public GrantedPriviligesResponse()
        {
            Success = false;
            Message = string.Empty;
        }
    }
}