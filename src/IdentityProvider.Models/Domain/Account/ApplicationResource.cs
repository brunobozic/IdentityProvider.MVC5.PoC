using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IdentityProvider.Infrastructure;
using IdentityProvider.Infrastructure.Domain;

namespace IdentityProvider.Models.Domain.Account
{
    [Table("Resources" , Schema = "Application")]
    public class ApplicationResource : DomainEntity<int>, IActive, IAuditTrail
    {
        public ApplicationResource()
        {
            Active = true;
            ActiveFrom = DateTime.UtcNow;
        }

        [Required]
        [DisplayName("Resource Name")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Resource Description")]
        public string Description { get; set; }

        #region IsActive

        [DisplayName("Record is active")]
        public bool Active { get; set; }
        [DisplayName("Record is active from (date)")]
        public DateTime? ActiveFrom { get; set; }
        [DisplayName("Record is active to (date)")]
        public DateTime? ActiveTo { get; set; }

        #endregion IsActive

        public virtual ICollection<Permission> ResourcePermissions { get; set; }
        public bool MakeActive { get; set; }
        public DateTime? ActiveUntil { get; set; }

        public override IEnumerable<ValidationResult> Validate( ValidationContext validationContext )
        {
            var name = new[] { "Name" };

            if (string.IsNullOrEmpty(Name) && name.Length > 0)
            {
                yield return new ValidationResult("Application Resource name is required." , name);
            }
        }
    }
}