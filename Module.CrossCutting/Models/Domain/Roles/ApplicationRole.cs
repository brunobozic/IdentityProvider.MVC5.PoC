using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using IdentityProvider.Infrastructure.Domain;
using Microsoft.AspNet.Identity.EntityFramework;
using TrackableEntities;

namespace IdentityProvider.Models.Domain.Account
{
    public class ApplicationRole : IdentityRole, IFullAudit, ISoftDeletable, IHandlesConcurrency, ITrackable
    {
        public ApplicationRole()
        {
            Id = Guid.NewGuid().ToString();
            Active = true;
            ActiveFrom = DateTime.UtcNow;
        }

        public ApplicationRole(string roleName) : this()
        {
            base.Name = roleName;
            Active = true;
            ActiveFrom = DateTime.UtcNow;
        }

        [Required]
        [DisplayName("Name")]
        // [MaxLength(50, ErrorMessage = "The name of the ApplicationRole must be between 2 and 50 characters"), MinLength(2)]
        public string Name { get; set; }

        [DisplayName("Description")]
        [MaxLength(260, ErrorMessage = "The description of the ApplicationRole must be between 2 and 260 characters")]
        [MinLength(2)]
        public string Description { get; set; }

        public virtual ApplicationUser UserProfile { get; set; }
        public virtual ICollection<RoleGroupContainsRole> RoleGroups { get; set; }

        public string ModifiedById { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string DeletedById { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string CreatedById { get; set; }
        public DateTime? CreatedDate { get; set; }
        public byte[] RowVersion { get; set; }
        public bool IsDeleted { get; set; }
        public TrackingState TrackingState { get; set; }
        public ICollection<string> ModifiedProperties { get; set; }

        #region IsActive

        public bool Active { get; set; }

        [DisplayName("Record is active from (date)")]
        public DateTime? ActiveFrom { get; set; }

        [DisplayName("Record is active to (date)")]
        public DateTime? ActiveTo { get; set; }

        #endregion IsActive
    }
}