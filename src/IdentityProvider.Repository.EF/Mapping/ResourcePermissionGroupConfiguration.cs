﻿using IdentityProvider.Models.Domain.Account;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace IdentityProvider.Repository.EF.Mapping
{
    public class ResourcePermissionGroupConfiguration : EntityTypeConfiguration<PermissionGroup>
    {
        public ResourcePermissionGroupConfiguration()
        {
            // Primary Key
            HasKey(e => e.Id);

            // Properties
            Property(e => e.Id)
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.Name)
                .IsVariableLength()
                // .HasMaxLength(100)
                .IsRequired();

            // Table & Column Mappings
            Property(t => t.RowVersion)
                .IsRowVersion()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

        }
    }
}