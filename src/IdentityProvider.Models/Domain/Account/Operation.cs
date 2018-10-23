using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IdentityProvider.Infrastructure.Domain;

namespace IdentityProvider.Models.Domain.Account
{
    [Table("Operations" , Schema = "Resource")]
    public class Operation : DomainEntity<int>, IActive
    {
        public Operation()
        {
            ResourcePermissions = new HashSet<Permission>();
            Active = true;
            ActiveFrom = DateTime.UtcNow;
        }

        [Required]
        [DisplayName("Name")]
        public string Name { get; set; }
        [DisplayName("Description")]
        public string Description { get; set; }
        [DisplayName("Record is active")]
        public bool Active { get; set; }
        [DisplayName("Record is active from (date)")]
        public DateTime? ActiveFrom { get; set; }
        [DisplayName("Record is active to (date)")]
        public DateTime? ActiveTo { get; set; }

        public virtual ICollection<Permission> ResourcePermissions { get; set; }

        public override IEnumerable<ValidationResult> Validate( ValidationContext validationContext )
        {
            var name = new[] { "Name" };

            if (string.IsNullOrEmpty(Name) && name.Length > 0)
            {
                yield return new ValidationResult("Operation name is required." , name);
            }
        }
    }
}