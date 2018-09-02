using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IdentityProvider.Infrastructure.Domain;

namespace IdentityProvider.Models.Domain.Account
{
    [Table("Resources" , Schema = "Account")]
    public class Resource : DomainEntity<int>, IActive
    {
        public Resource()
        {
            Roles = new HashSet<ApplicationRole>();
            Operations = new HashSet<Operation>();
            Active = true;
            ActiveFrom = DateTime.UtcNow;
        }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public DateTime? ActiveFrom { get; set; }
        public DateTime? ActiveTo { get; set; }
        public virtual ICollection<Operation> Operations { get; set; }
        public virtual ICollection<ApplicationRole> Roles { get; set; }

        public void AssignOperationsToThisResource( List<Operation> operations )
        {
            foreach (var op in operations)
            {
                Operations.Add(op);
            }
        }

        public void AssignOperationToThisResource( Operation operation )
        {
            Operations.Add(operation);
        }

        public void AssignRolesToThisResource( List<ApplicationRole> roles )
        {
            foreach (var role in roles)
            {
                Roles.Add(role);
            }
        }

        public void AssignRoleToThisResource( ApplicationRole role )
        {
            Roles.Add(role);
        }

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