using IdentityProvider.Repository.EFCore.Queries.UserGrants;
using System.Collections.Generic;

namespace IdentityProvider.ServiceLayer.Services.RowLeveLSecurityUserGrantService
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