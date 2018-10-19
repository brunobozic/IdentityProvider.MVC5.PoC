using System;
using System.Collections.Generic;
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
           
            
            base.Id = Guid.NewGuid().ToString();
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
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public virtual ApplicationUser UserProfile { get; set; }
        public virtual ICollection<RoleGroupRoleJoin> RoleGroups { get; set; }
        public virtual ICollection<RoleResourceJoin> Resources { get; set; }
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