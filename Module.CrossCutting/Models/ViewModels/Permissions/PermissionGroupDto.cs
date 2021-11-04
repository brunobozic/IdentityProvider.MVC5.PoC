using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityProvider.Models.ViewModels.Permissions
{
    public class PermissionGroupDto
    {
        public string Name { get; set; }
        public List<PermissionDto> Permissions { get; set; }
    }
}
