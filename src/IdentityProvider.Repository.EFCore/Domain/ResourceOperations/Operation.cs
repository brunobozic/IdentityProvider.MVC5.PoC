﻿using IdentityProvider.Repository.EFCore.Domain.Permissions;
using Module.CrossCutting;
using Module.CrossCutting.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityProvider.Repository.EFCore.Domain.ResourceOperations
{
    [Table("Operations", Schema = "Resource")]
    public class Operation : DomainEntity<int>, IActive, IFullAuditTrail
    {
        public Operation()
        {
            Active = true;
            ActiveFrom = DateTime.UtcNow;
        }

        [Required]
        [DisplayName("Name")]
        [MaxLength(50, ErrorMessage = "The name of the Operation must be between 1 and 50 characters")]
        [MinLength(1)]
        public string Name { get; set; }

        [DisplayName("Description")]
        [MaxLength(260, ErrorMessage = "The description of the Operation must be between 2 and 260 characters")]
        [MinLength(2)]
        public string Description { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var name = new[] { "Name" };
            var description = new[] { "Description" };

            if (string.IsNullOrEmpty(Name) && name.Length > 0)
                yield return new ValidationResult("Operation name is required.", name);

            if (string.IsNullOrEmpty(Description) && description.Length > 0)
                yield return new ValidationResult("Description is required.", description);
        }

        [DisplayName("Record is active")]
        public bool Active { get; set; }

        [DisplayName("Record is active from (date)")]
        public DateTime? ActiveFrom { get; set; }

        [DisplayName("Record is active to (date)")]
        public DateTime? ActiveTo { get; set; }

        public ICollection<Permission> Permissions { get; set; }
    }
}