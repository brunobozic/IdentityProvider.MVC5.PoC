using IdentityProvider.Repository.EFCore.Domain.Account.Employees;
using IdentityProvider.Repository.EFCore.Domain.Roles;
using Microsoft.AspNetCore.Identity;
using Module.CrossCutting.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using TrackableEntities.Common.Core;

namespace IdentityProvider.Repository.EFCore.Domain.Account
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser, IFullAudit, ITrackable
    {
        public ApplicationUser()
        {
            Id = Guid.NewGuid().ToString();
            Active = true;
            ActiveFrom = DateTime.UtcNow;
        }

        public ApplicationUser(string userName) : this()
        {
            UserName = userName;
            Active = true;
            ActiveFrom = DateTime.UtcNow;
        }

        public bool IsConfirmed { get; set; }
        public string ConfirmationToken { get; set; }

        [PersonalData]
        public string LastName { get; set; }

        [PersonalData]
        public string FirstName { get; set; }

        public byte[] UserImage { get; set; }

        [PersonalData]
        public string MobilePhone { get; set; }

        [PersonalData]
        public string HomePhone { get; set; }

        public DateTime? LastLoginDate { get; set; }
        public string TwoFactorSecret { get; set; }
        public Guid UserUid { get; set; }
        public string PasswordResetToken { get; set; }

        public bool IsDeleted { get; set; }

        public virtual UserProfile UserProfile { get; set; }
        public virtual int? UserProfileId { get; set; }

        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }

        public virtual int? EmployeeId { get; set; }

        public string ModifiedById { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string DeletedById { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string CreatedById { get; set; }
        public DateTime? CreatedDate { get; set; }

        public byte[] RowVersion { get; set; }

        [NotMapped]
        public TrackingState TrackingState { get; set; }

        #region IsActive

        public bool Active { get; set; }

        [DisplayName("Record is active from (date)")]
        public DateTime? ActiveFrom { get; set; }

        [DisplayName("Record is active to (date)")]
        public DateTime? ActiveTo { get; set; }

        [NotMapped]
        public ICollection<string> ModifiedProperties { get; set; }

        #endregion IsActive


        public virtual ICollection<AppUserAppRole> UserRoles { get; set; }
        public virtual ICollection<AppUserClaim> Claims { get; set; }
        public virtual ICollection<AppUserLogin> Logins { get; set; }
        public virtual ICollection<AppUserToken> Tokens { get; set; }
    }
}