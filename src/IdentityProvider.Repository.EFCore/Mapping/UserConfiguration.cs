using IdentityProvider.Repository.EFCore.Domain.Account;
using IdentityProvider.Repository.EFCore.Domain.Account.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityProvider.Repository.EFCore.Mapping
{
    public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> modelBuilder)
        {
            // User Profile One-To-One Relationship
            modelBuilder.HasOne(e => e.UserProfile)
                .WithOne(a => a.User)
                .HasForeignKey<UserProfile>(a => a.UserId)
                .IsRequired(false);

            // Employee One-To-One Relationship (If applicable)
            modelBuilder.HasOne(e => e.Employee)
                .WithOne(a => a.ApplicationUser)
                .HasForeignKey<Employee>(a => a.ApplicationUserId)
                .IsRequired(false);


        }
    }

}