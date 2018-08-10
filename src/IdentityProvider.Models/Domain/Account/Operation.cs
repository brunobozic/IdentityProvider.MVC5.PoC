using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IdentityProvider.Infrastructure.Domain;
using TrackableEntities;

namespace IdentityProvider.Models.Domain.Account
{
    [Table("Operations", Schema = "Account")]
    public class Operation : DomainEntity<int>, IActive, ITrackable
    {
        public Operation()
        {
            Resources= new HashSet<Resource>();
        }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public DateTime? ActiveFrom { get; set; }
        public DateTime? ActiveTo { get; set; }
        public ICollection<Resource> Resources { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }

        [NotMapped]
        public new TrackingState TrackingState { get; set; }
    }
}