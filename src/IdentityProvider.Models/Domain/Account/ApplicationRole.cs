using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IdentityProvider.Infrastructure.Domain;
using Microsoft.AspNet.Identity.EntityFramework;
using TrackableEntities;

namespace IdentityProvider.Models.Domain.Account
{
    
    public class ApplicationRole : IdentityRole, IFullAudit, ISoftDeletable, IHandlesConcurrency, ITrackable
    {

        public ApplicationRole()
        {
            RoleGroup = new HashSet<RoleGroups>();
            Resources = new HashSet<Resource>();
            base.Id = Guid.NewGuid().ToString();
        }

        public ApplicationRole(string roleName) : this()
        {
            RoleGroup = new HashSet<RoleGroups>();
            Resources = new HashSet<Resource>();
            base.Name = roleName;
        }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public virtual ApplicationUser UserProfile { get; set; }
        public virtual ICollection<RoleGroups> RoleGroup { get; set; }
        public virtual ICollection<Resource> Resources { get; set; }
        public TrackingState TrackingState { get; set; }
        public ICollection<string> ModifiedProperties { get; set; }
        public bool Active { get; set; }
        public DateTime? ActiveFrom { get; set; }
        public DateTime? ActiveTo { get; set; }
        public string ModifiedById { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string DeletedById { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string CreatedById { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public byte[] RowVersion { get; set; }
    }
}