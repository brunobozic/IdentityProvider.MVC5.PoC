using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityProvider.Infrastructure.Domain;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TrackableEntities;

namespace IdentityProvider.Models.Domain.Account
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

        public ApplicationUser( string userName ) : this()
        {
            UserName = userName;
            Active = true;
            ActiveFrom = DateTime.UtcNow;
        }

        public virtual ICollection<ApplicationRole> MyRoles { get; set; }
        public bool IsConfirmed { get; set; }
        public string ConfirmationToken { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public byte[] UserImage { get; set; }
        public string MobilePhone { get; set; }
        public string HomePhone { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public string TwoFactorSecret { get; set; }
        public Guid UserUid { get; set; }
        public string PasswordResetToken { get; set; }

        public bool IsDeleted { get; set; }

        public bool Active { get; set; }
        public DateTime? ActiveFrom { get; set; }
        public DateTime? ActiveTo { get; set; }

        public string ModifiedById { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string DeletedById { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string CreatedById { get; set; }
        public DateTime? CreatedDate { get; set; }

   
        public ICollection<string> ModifiedProperties { get; set; }

        public virtual UserProfile UserProfile { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync( UserManager<ApplicationUser> manager )
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this , DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public TrackingState TrackingState { get; set; }
        public Employee Employee1 { get; set; }
    }
}