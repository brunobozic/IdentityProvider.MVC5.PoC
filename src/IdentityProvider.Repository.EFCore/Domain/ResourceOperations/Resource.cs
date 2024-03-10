using IdentityProvider.Repository.EFCore.Domain.Permissions;
using Module.CrossCutting;
using Module.CrossCutting.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityProvider.Repository.EFCore.Domain.ResourceOperations
{
    [Table("Resources", Schema = "Account")]
    public class Resource : DomainEntity<int>, IActive, IFullAuditTrail
    {
        public Resource()
        {
            Active = true;
            ActiveFrom = DateTime.UtcNow;
        }

        [Required] public string Name { get; set; }

        public string Description { get; set; }
        public bool Active { get; set; }
        public DateTime? ActiveFrom { get; set; }
        public DateTime? ActiveTo { get; set; }
        public ICollection<Permission> Permissions { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var name = new[] { "Name" };

            if (string.IsNullOrEmpty(Name) && name.Length > 0)
                yield return new ValidationResult("Operation name is required.", name);
        }
    }
}