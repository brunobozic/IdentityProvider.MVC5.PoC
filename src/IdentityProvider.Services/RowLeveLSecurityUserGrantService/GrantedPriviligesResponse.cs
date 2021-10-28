using System.Collections.Generic;
using IdentityProvider.Repository.EF.Queries.UserGrants;

namespace IdentityProvider.Services.RowLeveLSecurityUserGrantService
{
    public class GrantedPriviligesResponse
    {
        public GrantedPriviligesResponse()
        {
            Success = false;
            Message = string.Empty;
        }

        public bool Success { get; set; }
        public string Message { get; set; }
        public List<GrantedPriviligesDto> GrantedPriviliges { get; set; }
    }
}