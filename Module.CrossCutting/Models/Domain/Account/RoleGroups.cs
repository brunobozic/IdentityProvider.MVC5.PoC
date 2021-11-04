using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IdentityProvider.Infrastructure.Domain;

namespace IdentityProvider.Models.Domain.Account
{
    [Table("RoleGroups", Schema = "Account")]
    public class RoleGroups : DomainEntity<int>, IActive
    {
        public RoleGroups()
        {
            Roles = new HashSet<ApplicationRole>();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string UserProfileId { get; set; }
        public virtual ApplicationUser UserProfile { get; set; }
        public bool Active { get; set; }
        public DateTime? ActiveFrom { get; set; }
        public DateTime? ActiveTo { get; set; }
        // Many to many with Roles (one RoleGroup will contain many Roles, one role can exist in many groups)
        public virtual ICollection<ApplicationRole> Roles { get; set; }   
        public int RoleId { get; set; }


        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return null;
        }
    }
}