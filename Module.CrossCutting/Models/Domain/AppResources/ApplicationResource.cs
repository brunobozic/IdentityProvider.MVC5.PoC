using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IdentityProvider.Infrastructure;
using IdentityProvider.Infrastructure.Domain;

namespace IdentityProvider.Models.Domain.Account
{
    [Table("Resources", Schema = "Application")]
    public class ApplicationResource : DomainEntity<int>, IActive, IAuditTrail
    {
        public ApplicationResource()
        {
            ResourcePermission = new HashSet<Permission>();
            Active = true;
            ActiveFrom = DateTime.UtcNow;
        }

        [Required]
        [DisplayName("Resource Name")]
        [MaxLength(50, ErrorMessage = "The name of the Operation must be between 2 and 50 characters")]
        [MinLength(2)]
        public string Name { get; set; }

        [DisplayName("Resource Description")]
        [MaxLength(260, ErrorMessage = "The description of the Operation must be between 2 and 260 characters")]
        [MinLength(2)]
        public string Description { get; set; }

        public virtual ICollection<Permission> ResourcePermission { get; set; }

        public bool MakeActive { get; set; }
        public DateTime? ActiveUntil { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var name = new[] {"Name"};

            if (string.IsNullOrEmpty(Name) && name.Length > 0)
                yield return new ValidationResult("Application Resource name is required.", name);
        }

        #region IsActive

        [DisplayName("Record is active")] public bool Active { get; set; }

        [DisplayName("Record is active from (date)")]
        public DateTime? ActiveFrom { get; set; }

        [DisplayName("Record is active to (date)")]
        public DateTime? ActiveTo { get; set; }

        #endregion IsActive
    }
}