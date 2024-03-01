using IdentityProvider.Repository.EFCore.Domain.Account.Employees;
using IdentityProvider.Repository.EFCore.Domain.OrganizationalUnits;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TrackableEntities.Common.Core;

namespace IdentityProvider.Repository.EFCore.Domain.Roles
{
    public class AppRole : IdentityRole, ITrackable
    {
        public AppRole()
        {
            Active = true;
            ActiveFrom = DateTime.UtcNow;
        }

        public AppRole(string roleName) : base(roleName)
        {
            Name = roleName;
            Active = true;
            ActiveFrom = DateTime.UtcNow;
        }

        [Required]
        [DisplayName("Name")]
        [MaxLength(50, ErrorMessage = "The name of the AppRole must be between 2 and 50 characters"), MinLength(2)]
        public string Name { get; set; }

        [DisplayName("Description")]
        [MaxLength(260, ErrorMessage = "The description of the AppRole must be between 2 and 260 characters")]
        [MinLength(2)]
        public string Description { get; set; }

        #region Navigation props

        public ICollection<RoleGroupContainsRole> RoleGroups { get; set; } // an app role may belong to multiple role groups
        public ICollection<EmployeeOwnsRoles> Employees { get; set; } // many employees may be assigned this app role
        public ICollection<RoleContainsPermissions> Permissions { get; set; } // this app role contains many permissions
        public ICollection<OrgUnitContainsRole> OrganizationUnits { get; set; } // this app role may be assigned to many org units
        public virtual ICollection<AppUserAppRole> UserRoles { get; set; }

        public virtual ICollection<AppRoleClaim> RoleClaims { get; set; }
        #endregion Navigation props

        #region IsActive

        public bool Active { get; set; }

        [DisplayName("Record is active from (date)")]
        public DateTime? ActiveFrom { get; set; }

        [DisplayName("Record is active to (date)")]
        public DateTime? ActiveTo { get; set; }
        [NotMapped]
        public TrackingState TrackingState { get; set; }

        [NotMapped]
        public ICollection<string> ModifiedProperties { get; set; }

        #endregion IsActive


    }
}