using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IdentityProvider.Infrastructure.Domain;
using TrackableEntities;

namespace IdentityProvider.Models.Domain.Account
{
    [Table("Resources", Schema = "Account")]
    public class Resource : DomainEntity<int>, IActive, ITrackable
    {
        public Resource()
        {
            Roles = new HashSet<ApplicationRole>();
            Operations= new HashSet<Operation>();
        }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public DateTime? ActiveFrom { get; set; }
        public DateTime? ActiveTo { get; set; }
        public virtual ICollection<Operation> Operations { get; set; }
        public virtual ICollection<ApplicationRole> Roles { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return null;
        }


        public TrackingState TrackingState { get; set; }
    }
}