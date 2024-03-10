﻿using IdentityProvider.Repository.EFCore.DatabaseAudit;
using IdentityProvider.Repository.EFCore.Domain;
using IdentityProvider.Repository.EFCore.Domain.Account;
using IdentityProvider.Repository.EFCore.Domain.Account.Employees;
using IdentityProvider.Repository.EFCore.Domain.OrganizationalUnits;
using IdentityProvider.Repository.EFCore.Domain.Permissions;
using IdentityProvider.Repository.EFCore.Domain.ResourceOperations;
using IdentityProvider.Repository.EFCore.Domain.Roles;
using IdentityProvider.Repository.EFCore.Mapping;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Module.CrossCutting;
using Module.CrossCutting.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityProvider.Repository.EFCore.EFDataContext
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, AppRole, Guid>, IAppDbContext
    {
        #region Private props

        private readonly List<DbAuditTrail> _auditList = new();
        private readonly List<EntityEntry> _list = new();
        private DbAuditTrailFactory _auditFactory;

        #endregion Private props

        #region Ctor
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            InstanceId = Guid.NewGuid();
            // DbInterception.Add(new DatabaseInterceptor());
        }
        #endregion Ctor

        public DbSet<Resource> ApplicationResource { get; set; }
        public DbSet<Operation> Operation { get; set; }
        public DbSet<RoleGroup> RoleGroup { get; set; }
        public DbSet<AppRole> Role { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<OrgUnitContainsRole> OrgUnitRoleLink { get; set; }
        public DbSet<Permission> Permission { get; set; }
        public DbSet<PermissionGroup> ResourcePermissionGroup { get; set; }
        public DbSet<PermissionGroupOwnsPermission> PermissionGroupOwnsPermissionLink { get; set; }
        public DbSet<OrganizationalUnit> OrganisationalUnit { get; set; }
        public DbSet<RoleGroupContainsRole> RoleGroupContainsRoles { get; set; }
        public DbSet<EmployeeBelongsToOrgUnit> EmployeesBelongToOgranizationalUnits { get; set; }
        public DbSet<EmployeeOwnsRoles> EmployeeOwnsRoles { get; set; }
        public DbSet<OrgUnitContainsRoleGroup> OrganizationalUnitsHaveRoleGroups { get; set; }
        public DbSet<UserProfile> UserProfile { get; set; }
        public DbSet<RoleContainsPermissionGroup> RoleContainsPermissionGroupLink { get; set; }
        public DbSet<RoleContainsPermissions> RoleContainsPermissions { get; set; }
        public DbSet<OutboxMessage> OutboxMessages { get; set; }
        public DbSet<InternalCommand> InternalCommands { get; set; }
        public DbSet<DbAuditTrail> DbAuditTrail { get; set; }
        public Guid InstanceId { get; set; }

        public override int SaveChanges()
        {
            _auditList.Clear();
            _list.Clear();
            _auditFactory = new DbAuditTrailFactory(this);
            var changes = 0;

            try
            {
                // Do the soft deletes
                foreach (var deletableEntity in ChangeTracker.Entries<ISoftDeletable>())
                {
                    if (deletableEntity.State != EntityState.Deleted) continue;

                    // We need to set this to unchanged here, because setting it to modified seems to set ALL of its fields to modified
                    deletableEntity.State = EntityState.Unchanged;

                    // This will set the entity's state to modified for the next time we query the ChangeTracker
                    deletableEntity.Entity.IsDeleted = true;

                    // Now, add soft deleted entity to full audit list...
                    var audit = _auditFactory.GetAudit(deletableEntity);
                    _auditList.Add(audit);
                    _list.Add(deletableEntity);
                }

                // Deal with the "made active" / "made inactive" / "will expire at datetime" entities here...
                foreach (var activeItem in ChangeTracker.Entries<IActive>())
                {
                    if (activeItem.State != EntityState.Deleted) continue;

                    if (activeItem.Entity.Active)
                    {
                        if (activeItem.Entity.ActiveFrom == null || activeItem.Entity.ActiveFrom == DateTime.MinValue)
                            activeItem.Entity.ActiveFrom = DateTime.Now;
                        if (activeItem.Entity.ActiveTo == null || activeItem.Entity.ActiveFrom == DateTime.MinValue)
                        {
                            // do not set in advance !!
                        }
                    }
                }

                // Do the audit trails
                var currentDateTime = DateTime.Now;

                var currentApplicationUserId = Guid.Empty;

                // TODO: fetch user Id from the facade
                currentApplicationUserId = Guid.Empty;

                #region Full audit trail

                try
                {
                    var entityList =
                        ChangeTracker.Entries<IFullAuditTrail>()
                            .Where(p => p.State == EntityState.Added
                                        || p.State == EntityState.Deleted
                                        || p.State == EntityState.Modified);

                    foreach (var entity in entityList)
                    {
                        var audit = _auditFactory.GetAudit(entity);
                        _auditList.Add(audit);
                        _list.Add(entity);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Debug.WriteLine(e);
                    // TODO: do logging here
                }

                #endregion Full audit trail

                foreach (var auditableEntity in ChangeTracker.Entries<IFullAudit>())
                {
                    if (auditableEntity.State != EntityState.Added &&
                        auditableEntity.State != EntityState.Modified) continue;

                    // Adding or modifying - update the edited audit trails
                    auditableEntity.Entity.ModifiedDate = currentDateTime;
                    auditableEntity.Entity.ModifiedById = currentApplicationUserId.ToString();

                    switch (auditableEntity.State)
                    {
                        case EntityState.Added:

                            // Adding - set the created audit trails
                            auditableEntity.Entity.CreatedDate = currentDateTime;
                            auditableEntity.Entity.CreatedById = currentApplicationUserId.ToString();

                            break;

                        case EntityState.Modified:

                            // Modified (or deleted from above) - ensure that the created fields are not being modified
                            var fullName = auditableEntity.Entity.GetType().Name;
                            if (fullName != null && !fullName.Equals("ApplicationUser"))
                            {
                                //if (auditableEntity.Property(p => p.CreatedDate).IsModified ||
                                //    auditableEntity.Property(p => p.CreatedById).IsModified)
                                //    throw new DbEntityValidationException(
                                //        $"Attempt to change created audit trails on a modified {auditableEntity.Entity.GetType().FullName}");
                            }

                            break;
                    }
                }

                foreach (var auditableEntity in ChangeTracker.Entries<IDeleteOnlyAudit>())
                {
                    if (auditableEntity.State == EntityState.Deleted)
                        auditableEntity.Entity.DeletedDate = currentDateTime;
                    auditableEntity.Entity.DeletedById = currentApplicationUserId;
                }

                foreach (var auditableEntity in ChangeTracker.Entries<IModifyOnlyAudit>())
                {
                    if (auditableEntity.State == EntityState.Modified)
                        auditableEntity.Entity.ModifiedDate = currentDateTime;
                    auditableEntity.Entity.ModifiedById = currentApplicationUserId;
                }

                foreach (var auditableEntity in ChangeTracker.Entries<ICreateOnlyAudit>())
                {
                    if (auditableEntity.State == EntityState.Added)
                        auditableEntity.Entity.CreatedDate = currentDateTime;
                    auditableEntity.Entity.CreatedById = currentApplicationUserId;
                }

                changes = base.SaveChanges();

                if (_auditList.Count > 0)
                {
                    var i = 0;
                    foreach (var audit in _auditList)
                    {
                        if (audit.Actions == AuditActions.Insert.ToString())
                            audit.TableIdValue = _auditFactory.GetKeyValue(_list[i]);
                        DbAuditTrail.Add(audit);
                        i++;
                    }

                    base.SaveChanges();
                }
            }
            catch (DbEntityValidationException entityException)
            {
                var errors = entityException.EntityValidationErrors;
                var result = new StringBuilder();
                var allErrors = new List<ValidationResult>();

                foreach (var error in errors)
                    foreach (var validationError in error.ValidationErrors)
                    {
                        result.AppendFormat(
                            "\r\n  Entity of type {0} has validation error \"{1}\" for property {2}.\r\n",
                            error.Entry.Entity.GetType(), validationError.ErrorMessage, validationError.PropertyName);
                        var domainEntity = error.Entry.Entity as DomainEntity<int>;
                        if (domainEntity != null)
                            result.Append(domainEntity.IsTransient()
                                ? "  This entity was added in this session.\r\n"
                                : $"  The Id of the entity is {domainEntity.Id}.\r\n");
                        allErrors.Add(new ValidationResult(validationError.ErrorMessage,
                            new[] { validationError.PropertyName }));
                    }

                throw new ModelValidationException(result.ToString(), entityException, allErrors);
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateConcurrencyException ex
            ) // This will fire only for entities that have the [RowVersion] property implemented...
            {
                var entry = ex.Entries.Single();
                var clientValues = entry.Entity;
                var databaseEntry = entry.GetDatabaseValues();

                if (databaseEntry == null)
                {
                    // The entity was deleted by another user...
                }
                else
                {
                    // Otherwise create the object based on what is now in the db...
                    var databaseValues = databaseEntry.ToObject();
                }

                //	if (databaseValues.Name != clientValues.Name)
                //		ModelState.AddModelError("Name", "Current value: "
                //			+ databaseValues.Name);
                //	if (databaseValues.Budget != clientValues.Budget)
                //		ModelState.AddModelError("Budget", "Current value: "
                //			+ String.Format("{0:c}", databaseValues.Budget));
                //	if (databaseValues.StartDate != clientValues.StartDate)
                //		ModelState.AddModelError("StartDate", "Current value: "
                //			+ String.Format("{0:d}", databaseValues.StartDate));
                //	if (databaseValues.InstructorID != clientValues.InstructorID)
                //		ModelState.AddModelError("InstructorID", "Current value: "
                //			+ db.Instructors.Find(databaseValues.InstructorID).FullName);
                //	ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                //		+ "was modified by another user after you got the original value. The "
                //		+ "edit operation was canceled and the current values in the database "
                //		+ "have been displayed. If you still want to edit this record, click "
                //		+ "the Save button again. Otherwise click the Back to List hyperlink.");
                //	departmentToUpdate.RowVersion = databaseValues.RowVersion;
                //}

                // Update the values of the entity that failed to save from the store
                ex.Entries.Single().Reload();

                var result = new StringBuilder();
                result.Append("The record you attempted to edit "
                              + "was modified by another user after you got the original value. The "
                              + "edit operation was canceled and the current values in the database "
                              + "have been displayed.");

                throw new System.Data.Entity.Infrastructure.DbUpdateConcurrencyException(result.ToString(), ex);
            }
            catch (Exception ex)
            {
                throw;
            }

            return changes;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            _auditList.Clear();
            _list.Clear();
            _auditFactory = new DbAuditTrailFactory(this);
            var changes = 0;

            try
            {
                // Do the soft deletes
                foreach (var deletableEntity in ChangeTracker.Entries<ISoftDeletable>())
                {
                    if (deletableEntity.State != EntityState.Deleted) continue;

                    // We need to set this to unchanged here, because setting it to modified seems to set ALL of its fields to modified
                    deletableEntity.State = EntityState.Unchanged;

                    // This will set the entity's state to modified for the next time we query the ChangeTracker
                    deletableEntity.Entity.IsDeleted = true;

                    // Now, add soft deleted entity to full audit list...
                    var audit = _auditFactory.GetAudit(deletableEntity);
                    _auditList.Add(audit);
                    _list.Add(deletableEntity);
                }

                // Deal with the "made active" / "made inactive" / "will expire at datetime" entities here...
                foreach (var activeItem in ChangeTracker.Entries<IActive>())
                {
                    if (activeItem.State != EntityState.Deleted) continue;

                    if (activeItem.Entity.Active)
                    {
                        if (activeItem.Entity.ActiveFrom == null || activeItem.Entity.ActiveFrom == DateTime.MinValue)
                            activeItem.Entity.ActiveFrom = DateTime.Now;
                        if (activeItem.Entity.ActiveTo == null || activeItem.Entity.ActiveFrom == DateTime.MinValue)
                        {
                            // do not set in advance !!
                        }
                    }
                }

                // Do the audit trails
                var currentDateTime = DateTime.Now;

                var currentApplicationUserId = Guid.Empty;

                // TODO: fetch user Id from the facade
                currentApplicationUserId = Guid.Empty;

                #region Full audit trail

                try
                {
                    var entityList =
                        ChangeTracker.Entries<IFullAuditTrail>()
                            .Where(p => p.State == EntityState.Added
                                        || p.State == EntityState.Deleted
                                        || p.State == EntityState.Modified);

                    foreach (var entity in entityList)
                    {
                        var audit = _auditFactory.GetAudit(entity);
                        _auditList.Add(audit);
                        _list.Add(entity);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Debug.WriteLine(e);
                    // TODO: do logging here
                }

                #endregion Full audit trail

                foreach (var auditableEntity in ChangeTracker.Entries<IFullAudit>())
                {
                    if (auditableEntity.State != EntityState.Added &&
                        auditableEntity.State != EntityState.Modified) continue;

                    // Adding or modifying - update the edited audit trails
                    auditableEntity.Entity.ModifiedDate = currentDateTime;
                    auditableEntity.Entity.ModifiedById = currentApplicationUserId.ToString();

                    switch (auditableEntity.State)
                    {
                        case EntityState.Added:

                            // Adding - set the created audit trails
                            auditableEntity.Entity.CreatedDate = currentDateTime;
                            auditableEntity.Entity.CreatedById = currentApplicationUserId.ToString();

                            break;

                        case EntityState.Modified:

                            // Modified (or deleted from above) - ensure that the created fields are not being modified
                            var fullName = auditableEntity.Entity.GetType().Name;
                            if (fullName != null && !fullName.Equals("ApplicationUser"))
                            {
                                //if (auditableEntity.Property(p => p.CreatedDate).IsModified ||
                                //    auditableEntity.Property(p => p.CreatedById).IsModified)
                                //    throw new DbEntityValidationException(
                                //        $"Attempt to change created audit trails on a modified {auditableEntity.Entity.GetType().FullName}");
                            }

                            break;
                    }
                }

                foreach (var auditableEntity in ChangeTracker.Entries<IDeleteOnlyAudit>())
                {
                    if (auditableEntity.State == EntityState.Deleted)
                        auditableEntity.Entity.DeletedDate = currentDateTime;
                    auditableEntity.Entity.DeletedById = currentApplicationUserId;
                }

                foreach (var auditableEntity in ChangeTracker.Entries<IModifyOnlyAudit>())
                {
                    if (auditableEntity.State == EntityState.Modified)
                        auditableEntity.Entity.ModifiedDate = currentDateTime;
                    auditableEntity.Entity.ModifiedById = currentApplicationUserId;
                }

                foreach (var auditableEntity in ChangeTracker.Entries<ICreateOnlyAudit>())
                {
                    if (auditableEntity.State == EntityState.Added)
                        auditableEntity.Entity.CreatedDate = currentDateTime;
                    auditableEntity.Entity.CreatedById = currentApplicationUserId;
                }

                changes = await base.SaveChangesAsync(cancellationToken);

                if (_auditList.Count > 0)
                {
                    var i = 0;
                    foreach (var audit in _auditList)
                    {
                        if (audit.Actions == AuditActions.Insert.ToString())
                            audit.TableIdValue = _auditFactory.GetKeyValue(_list[i]);
                        DbAuditTrail.Add(audit);
                        i++;
                    }

                    await base.SaveChangesAsync(cancellationToken);
                }
            }
            catch (DbEntityValidationException entityException)
            {
                var errors = entityException.EntityValidationErrors;
                var result = new StringBuilder();
                var allErrors = new List<ValidationResult>();

                foreach (var error in errors)
                    foreach (var validationError in error.ValidationErrors)
                    {
                        result.AppendFormat(
                            "\r\n  Entity of type {0} has validation error \"{1}\" for property {2}.\r\n",
                            error.Entry.Entity.GetType(), validationError.ErrorMessage, validationError.PropertyName);
                        var domainEntity = error.Entry.Entity as DomainEntity<int>;
                        if (domainEntity != null)
                            result.Append(domainEntity.IsTransient()
                                ? "  This entity was added in this session.\r\n"
                                : $"  The Id of the entity is {domainEntity.Id}.\r\n");
                        allErrors.Add(new ValidationResult(validationError.ErrorMessage,
                            new[] { validationError.PropertyName }));
                    }

                throw new ModelValidationException(result.ToString(), entityException, allErrors);
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateConcurrencyException ex
            ) // This will fire only for entities that have the [RowVersion] property implemented...
            {
                var entry = ex.Entries.Single();
                var clientValues = entry.Entity;
                var databaseEntry = entry.GetDatabaseValues();

                if (databaseEntry == null)
                {
                    // The entity was deleted by another user...
                }
                else
                {
                    // Otherwise create the object based on what is now in the db...
                    var databaseValues = databaseEntry.ToObject();
                }

                //	if (databaseValues.Name != clientValues.Name)
                //		ModelState.AddModelError("Name", "Current value: "
                //			+ databaseValues.Name);
                //	if (databaseValues.Budget != clientValues.Budget)
                //		ModelState.AddModelError("Budget", "Current value: "
                //			+ String.Format("{0:c}", databaseValues.Budget));
                //	if (databaseValues.StartDate != clientValues.StartDate)
                //		ModelState.AddModelError("StartDate", "Current value: "
                //			+ String.Format("{0:d}", databaseValues.StartDate));
                //	if (databaseValues.InstructorID != clientValues.InstructorID)
                //		ModelState.AddModelError("InstructorID", "Current value: "
                //			+ db.Instructors.Find(databaseValues.InstructorID).FullName);
                //	ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                //		+ "was modified by another user after you got the original value. The "
                //		+ "edit operation was canceled and the current values in the database "
                //		+ "have been displayed. If you still want to edit this record, click "
                //		+ "the Save button again. Otherwise click the Back to List hyperlink.");
                //	departmentToUpdate.RowVersion = databaseValues.RowVersion;
                //}

                // Update the values of the entity that failed to save from the store
                ex.Entries.Single().Reload();

                var result = new StringBuilder();
                result.Append("The record you attempted to edit "
                              + "was modified by another user after you got the original value. The "
                              + "edit operation was canceled and the current values in the database "
                              + "have been displayed.");

                throw new System.Data.Entity.Infrastructure.DbUpdateConcurrencyException(result.ToString(), ex);
            }

            return changes;
        }

        public async Task<Task<string>> SaveChangesAsync()
        {
            _auditList.Clear();
            _list.Clear();
            _auditFactory = new DbAuditTrailFactory(this);
            var changes = 0;

            try
            {
                // Do the soft deletes
                foreach (var deletableEntity in ChangeTracker.Entries<ISoftDeletable>())
                {
                    if (deletableEntity.State != EntityState.Deleted) continue;

                    // We need to set this to unchanged here, because setting it to modified seems to set ALL of its fields to modified
                    deletableEntity.State = EntityState.Unchanged;

                    // This will set the entity's state to modified for the next time we query the ChangeTracker
                    deletableEntity.Entity.IsDeleted = true;

                    // Now, add soft deleted entity to full audit list...
                    var audit = _auditFactory.GetAudit(deletableEntity);
                    _auditList.Add(audit);
                    _list.Add(deletableEntity);
                }

                // Deal with the "made active" / "made inactive" / "will expire at datetime" entities here...
                foreach (var activeItem in ChangeTracker.Entries<IActive>())
                {
                    if (activeItem.State != EntityState.Deleted) continue;

                    if (activeItem.Entity.Active)
                    {
                        if (activeItem.Entity.ActiveFrom == null || activeItem.Entity.ActiveFrom == DateTime.MinValue)
                            activeItem.Entity.ActiveFrom = DateTime.Now;
                        if (activeItem.Entity.ActiveTo == null || activeItem.Entity.ActiveFrom == DateTime.MinValue)
                        {
                            // do not set in advance !!
                        }
                    }
                }

                // Do the audit trails
                var currentDateTime = DateTime.Now;

                var currentApplicationUserId = Guid.Empty;

                // TODO: fetch user Id from the facade
                currentApplicationUserId = Guid.Empty;

                #region Full audit trail

                try
                {
                    var entityList =
                        ChangeTracker.Entries<IFullAuditTrail>()
                            .Where(p => p.State == EntityState.Added
                                        || p.State == EntityState.Deleted
                                        || p.State == EntityState.Modified);

                    foreach (var entity in entityList)
                    {
                        var audit = _auditFactory.GetAudit(entity);
                        _auditList.Add(audit);
                        _list.Add(entity);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Debug.WriteLine(e);
                    // TODO: do logging here
                }

                #endregion Full audit trail

                foreach (var auditableEntity in ChangeTracker.Entries<IFullAudit>())
                {
                    if (auditableEntity.State != EntityState.Added &&
                        auditableEntity.State != EntityState.Modified) continue;

                    // Adding or modifying - update the edited audit trails
                    auditableEntity.Entity.ModifiedDate = currentDateTime;
                    auditableEntity.Entity.ModifiedById = currentApplicationUserId.ToString();

                    switch (auditableEntity.State)
                    {
                        case EntityState.Added:

                            // Adding - set the created audit trails
                            auditableEntity.Entity.CreatedDate = currentDateTime;
                            auditableEntity.Entity.CreatedById = currentApplicationUserId.ToString();

                            break;

                        case EntityState.Modified:

                            // Modified (or deleted from above) - ensure that the created fields are not being modified
                            var fullName = auditableEntity.Entity.GetType().Name;
                            if (fullName != null && !fullName.Equals("ApplicationUser"))
                            {
                                //if (auditableEntity.Property(p => p.CreatedDate).IsModified ||
                                //    auditableEntity.Property(p => p.CreatedById).IsModified)
                                //    throw new DbEntityValidationException(
                                //        $"Attempt to change created audit trails on a modified {auditableEntity.Entity.GetType().FullName}");
                            }

                            break;
                    }
                }

                foreach (var auditableEntity in ChangeTracker.Entries<IDeleteOnlyAudit>())
                {
                    if (auditableEntity.State == EntityState.Deleted)
                        auditableEntity.Entity.DeletedDate = currentDateTime;
                    auditableEntity.Entity.DeletedById = currentApplicationUserId;
                }

                foreach (var auditableEntity in ChangeTracker.Entries<IModifyOnlyAudit>())
                {
                    if (auditableEntity.State == EntityState.Modified)
                        auditableEntity.Entity.ModifiedDate = currentDateTime;
                    auditableEntity.Entity.ModifiedById = currentApplicationUserId;
                }

                foreach (var auditableEntity in ChangeTracker.Entries<ICreateOnlyAudit>())
                {
                    if (auditableEntity.State == EntityState.Added)
                        auditableEntity.Entity.CreatedDate = currentDateTime;
                    auditableEntity.Entity.CreatedById = currentApplicationUserId;
                }

                changes = await base.SaveChangesAsync();

                if (_auditList.Count > 0)
                {
                    var i = 0;
                    foreach (var audit in _auditList)
                    {
                        if (audit.Actions == AuditActions.Insert.ToString())
                            audit.TableIdValue = _auditFactory.GetKeyValue(_list[i]);
                        DbAuditTrail.Add(audit);
                        i++;
                    }

                    await base.SaveChangesAsync();
                }
            }
            catch (DbEntityValidationException entityException)
            {
                var errors = entityException.EntityValidationErrors;
                var result = new StringBuilder();
                var allErrors = new List<ValidationResult>();

                foreach (var error in errors)
                    foreach (var validationError in error.ValidationErrors)
                    {
                        result.AppendFormat(
                            "\r\n  Entity of type {0} has validation error \"{1}\" for property {2}.\r\n",
                            error.Entry.Entity.GetType(), validationError.ErrorMessage, validationError.PropertyName);
                        var domainEntity = error.Entry.Entity as DomainEntity<int>;
                        if (domainEntity != null)
                            result.Append(domainEntity.IsTransient()
                                ? "  This entity was added in this session.\r\n"
                                : $"  The Id of the entity is {domainEntity.Id}.\r\n");
                        allErrors.Add(new ValidationResult(validationError.ErrorMessage,
                            new[] { validationError.PropertyName }));
                    }

                throw new ModelValidationException(result.ToString(), entityException, allErrors);
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateConcurrencyException ex
            ) // This will fire only for entities that have the [RowVersion] property implemented...
            {
                var entry = ex.Entries.Single();
                var clientValues = entry.Entity;
                var databaseEntry = entry.GetDatabaseValues();

                if (databaseEntry == null)
                {
                    // The entity was deleted by another user...
                }
                else
                {
                    // Otherwise create the object based on what is now in the db...
                    var databaseValues = databaseEntry.ToObject();
                }

                //	if (databaseValues.Name != clientValues.Name)
                //		ModelState.AddModelError("Name", "Current value: "
                //			+ databaseValues.Name);
                //	if (databaseValues.Budget != clientValues.Budget)
                //		ModelState.AddModelError("Budget", "Current value: "
                //			+ String.Format("{0:c}", databaseValues.Budget));
                //	if (databaseValues.StartDate != clientValues.StartDate)
                //		ModelState.AddModelError("StartDate", "Current value: "
                //			+ String.Format("{0:d}", databaseValues.StartDate));
                //	if (databaseValues.InstructorID != clientValues.InstructorID)
                //		ModelState.AddModelError("InstructorID", "Current value: "
                //			+ db.Instructors.Find(databaseValues.InstructorID).FullName);
                //	ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                //		+ "was modified by another user after you got the original value. The "
                //		+ "edit operation was canceled and the current values in the database "
                //		+ "have been displayed. If you still want to edit this record, click "
                //		+ "the Save button again. Otherwise click the Back to List hyperlink.");
                //	departmentToUpdate.RowVersion = databaseValues.RowVersion;
                //}

                // Update the values of the entity that failed to save from the store
                ex.Entries.Single().Reload();

                var result = new StringBuilder();
                result.Append("The record you attempted to edit "
                              + "was modified by another user after you got the original value. The "
                              + "edit operation was canceled and the current values in the database "
                              + "have been displayed.");

                throw new System.Data.Entity.Infrastructure.DbUpdateConcurrencyException(result.ToString(), ex);
            }

            return Task.FromResult("OK");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new AuditTrailConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeBelongsToOrgUnitConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeOwnsPermissionGroupConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeOwnsRoleGroupsConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeOwnsRolesConfiguration());
            modelBuilder.ApplyConfiguration(new OperationConfiguration());
            modelBuilder.ApplyConfiguration(new OrganisationalUnitConfiguration());
            modelBuilder.ApplyConfiguration(new PermissionConfiguration());
            modelBuilder.ApplyConfiguration(new PermissionGroupOwnsPermissionConfiguration());
            modelBuilder.ApplyConfiguration(new ResourceConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new RoleContainsPermissionsConfiguration());
            modelBuilder.ApplyConfiguration(new RoleGroupConfiguration());
            modelBuilder.ApplyConfiguration(new RoleGroupContainsRoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserProfileConfiguration());

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}