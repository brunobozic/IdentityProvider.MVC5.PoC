using System.Collections.Generic;

namespace IdentityProvider.Repository.EFCore.Queries.UserGrants
{
    public class UserGrantQueryResponse
    {
        public bool Succes { get; set; }
        public string Message { get; set; }
        public List<GrantedPriviligesDto> GrantedPriviliges { get; set; }
    }
}