using Module.CrossCutting;
using Module.CrossCutting.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityProvider.Repository.EFCore.Domain.Roles
{
    [Table("RoleGroupContainsRole", Schema = "Organization")]
    public class RoleGroupContainsRole : DomainEntity<int>, IActive, IFullAuditTrail
    {
        public RoleGroupContainsRole()
        {
            Active = true;
            ActiveFrom = DateTime.UtcNow;
        }
        [ForeignKey("RoleId")]
        public AppRole Role { get; set; }
        [ForeignKey("RoleGroupId")]
        public RoleGroup RoleGroup { get; set; }

        public Guid RoleId { get; set; }
        public int? RoleGroupId { get; set; }

        #region IValidatable Entity contract implementation

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }

        #endregion IValidatable Entity contract implementation

        #region IsActive

        public bool Active { get; set; }
        public DateTime? ActiveFrom { get; set; }
        public DateTime? ActiveTo { get; set; }

        #endregion IsActive
    }
}