using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using IdentityProvider.Infrastructure.Domain;

namespace IdentityProvider.Models.Domain.Account
{
    [Table("Operations" , Schema = "Account")]
    public class Operation : DomainEntity<int>, IActive
    {
        public Operation()
        {
            Resources = new HashSet<Resource>();
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

        public ICollection<Resource> Resources { get; set; }

        public List<Resource> FetchAssociatedResources()
        {
            List<Resource> resources;

            try
            {
                resources = Resources.Where(i => i.Active && !i.IsDeleted && i.Id.Equals(Id)).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Debug.WriteLine(e);

                return null;
            }


            return resources;
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