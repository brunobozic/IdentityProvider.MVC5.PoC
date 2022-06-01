using IdentityProvider.Repository.EFCore.Domain.Account;
using IdentityProvider.Repository.EFCore.Domain.Account.Employees;
using IdentityProvider.Repository.EFCore.Domain.OrganizationalUnits;
using Microsoft.AspNetCore.Identity;
using Module.CrossCutting.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TrackableEntities.Common.Core;

namespace IdentityProvider.Repository.EFCore.Domain.Roles
{
    public class IdentityFrameworkRole : IdentityRole, IFullAudit, ISoftDeletable, IHandlesConcurrency, ITrackable
    {
        public IdentityFrameworkRole()
        {
            Id = Guid.NewGuid().ToString();
            Active = true;
            ActiveFrom = DateTime.UtcNow;
        }

        public IdentityFrameworkRole(string roleName) : this()
        {
            base.Name = roleName;
            Active = true;
            ActiveFrom = DateTime.UtcNow;
        }

        [Required]
        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Description")]
        [MaxLength(260, ErrorMessage = "The description of the IdentityFrameworkRole must be between 2 and 260 characters")]
        [MinLength(2)]
        public string Description { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<RoleGroupContainsRole> RoleGroups { get; set; }

        public string ModifiedById { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string DeletedById { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string CreatedById { get; set; }
        public DateTime? CreatedDate { get; set; }
        public byte[] RowVersion { get; set; }
        public bool IsDeleted { get; set; }

        [NotMapped]
        public TrackingState TrackingState { get; set; }

        [NotMapped]
        public ICollection<string> ModifiedProperties { get; set; }

        #region IsActive

        public bool Active { get; set; }

        [DisplayName("Record is active from (date)")]
        public DateTime? ActiveFrom { get; set; }

        [DisplayName("Record is active to (date)")]
        public DateTime? ActiveTo { get; set; }

        public ICollection<EmployeeOwnsRoles> Employees { get; set; }
        public ICollection<RoleContainsPermissions> Permissions { get; set; }
        public OrganizationalUnit OrganizationUnit { get; set; }

        #endregion IsActive
    }
}