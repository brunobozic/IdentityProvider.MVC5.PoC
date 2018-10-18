using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IdentityProvider.Infrastructure.Domain;

namespace IdentityProvider.Models.Domain.Account
{
    [Table("RoleGroupRoleJoin" , Schema = "Account")]
   public class RoleGroupRoleJoin : DomainEntity<int>, IActive
    {
        public RoleGroupRoleJoin()
        {
            Active = true;
            ActiveFrom = DateTime.UtcNow;
        }

        public virtual ApplicationRole Role { get; set; }
        public virtual RoleGroup RoleGroup { get; set; }

        public string RoleId { get; set; }
        public int RoleGroupId { get; set; }

        public override IEnumerable<ValidationResult> Validate( ValidationContext validationContext )
        {
            throw new NotImplementedException();
        }

        public bool Active { get; set; }
        public DateTime? ActiveFrom { get; set; }
        public DateTime? ActiveTo { get; set; }
    }
}
