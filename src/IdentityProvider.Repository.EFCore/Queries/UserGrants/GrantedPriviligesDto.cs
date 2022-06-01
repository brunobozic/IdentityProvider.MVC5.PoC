using System.Collections.Generic;

namespace IdentityProvider.Repository.EFCore.Queries.UserGrants
{
    public class GrantedPriviligesDto
    {
        public List<OrganizationalUnitsDto> OrganizationalUnits { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string UserId { get; set; }
        public string EmployeeTitle { get; set; }
        public string EmployeeId { get; set; }
    }
}