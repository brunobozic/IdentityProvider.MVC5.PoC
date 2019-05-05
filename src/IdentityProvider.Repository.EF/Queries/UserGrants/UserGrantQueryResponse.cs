using System.Collections.Generic;

namespace IdentityProvider.Repository.EF.Queries.UserGrants
{
    public class UserGrantQueryResponse
    {
        public bool Succes { get; set; }
        public string Message { get; set; }
        public List<GrantedPriviligesDto> GrantedPriviliges { get; set; }
    }
}