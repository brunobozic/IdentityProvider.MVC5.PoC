﻿using IdentityProvider.Models.Domain.Account;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace IdentityProvider.Repository.EF.Mapping
{
    public class RolePermissionGroupLinkConfiguration : EntityTypeConfiguration<RoleContainsPermissionGroupLink>
    {
        public RolePermissionGroupLinkConfiguration()
        {
            // Primary Key
            HasKey(e => e.Id);

            // Properties
            Property(e => e.Id)
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            // Table & Column Mappings
            Property(t => t.RowVersion)
                .IsRowVersion()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
        }
    }
}
