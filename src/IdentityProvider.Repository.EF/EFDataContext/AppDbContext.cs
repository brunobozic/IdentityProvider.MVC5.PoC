using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IdentityProvider.Infrastructure;
using IdentityProvider.Infrastructure.DatabaseAudit;
using IdentityProvider.Infrastructure.DatabaseLog.Model;
using IdentityProvider.Infrastructure.Domain;
using IdentityProvider.Models.Domain.Account;
using Microsoft.AspNet.Identity.EntityFramework;
using StructureMap;
using Database = System.Data.Entity.Database;
using ModelValidationException = IdentityProvider.Infrastructure.ModelValidationException;

namespace IdentityProvider.Repository.EF.EFDataContext
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>, IDisposable
    {
        private readonly List<DbAuditTrail> _auditList = new List<DbAuditTrail>();
        private readonly List<DbEntityEntry> _list = new List<DbEntityEntry>();
        private DbAuditTrailFactory _auditFactory;
        private bool _disposed;

        [DefaultConstructor] // Set Default Constructor for StructureMap
        public AppDbContext(string connectionStringName = "SimpleMembership")
            : base(connectionStringName)
        {
            InstanceId = Guid.NewGuid();
            // DbInterception.Add(new DatabaseInterceptor());
            Configuration.ProxyCreationEnabled = false;
        }

        public AppDbContext()
        {
            Database.SetInitializer<AppDbContext>(null);
            InstanceId = Guid.NewGuid();
        }

        public DbSet<ApplicationResource> ApplicationResource { get; set; }
        public DbSet<Operation> Operation { get; set; }
        public DbSet<RoleGroup> RoleGroup { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<OrgUnitContainsRoleLink> OrgUnitRoleLink { get; set; }
        public DbSet<Permission> Permission { get; set; }
        public DbSet<PermissionGroup> ResourcePermissionGroup { get; set; }
        public DbSet<PermissionGroupOwnsPermissionLink> PermissionGroupOwnsPermissionLink { get; set; }
        public DbSet<OrganizationalUnit> OrganisationalUnit { get; set; }
        public DbSet<RoleGroupContainsRoleLink> RoleGroupContainsRoles { get; set; }
        public DbSet<EmployeeBelongsToOrgUnitLink> EmployeesBelongToOgranizationalUnits { get; set; }
        public DbSet<OrgUnitContainsRoleGroupLink> OrganizationalUnitsHaveRoleGroups { get; set; }
        public DbSet<UserProfile> UserProfile { get; set; }
        public DbSet<DbLog> DatabaseLog { get; set; }
        // public DbSet<ResourcesHaveOperations> ResourcesHaveOperations { get; set; }
        public DbSet<DbAuditTrail> DbAuditTrail { get; set; }
        public Guid InstanceId { get; set; }
        public Database Database { get; set; }

        /// <inheritdoc />
        /// <summary>
        ///     Saves all changes made in this context to the underlying database.
        /// </summary>
        /// <exception cref="T:System.Data.Entity.Infrastructure.DbUpdateException">
        ///     An error occurred sending updates to the database.
        /// </exception>
        /// <exception cref="T:System.Data.Entity.Infrastructure.DbUpdateConcurrencyException">
        ///     A database command did not affect the expected number of rows. This usually
        ///     indicates an optimistic concurrency violation; that is, a row has been changed
        ///     in the database since it was queried.
        /// </exception>
        /// <exception cref="T:System.Data.Entity.Validation.DbEntityValidationException">
        ///     The save was aborted because validation of entity property values failed.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        ///     An attempt was made to use unsupported behavior such as executing multiple
        ///     asynchronous commands concurrently on the same context instance.
        /// </exception>
        /// <exception cref="T:System.ObjectDisposedException">
        ///     The context or connection have been disposed.
        /// </exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///     Some error occurred attempting to process entities in the context either
        ///     before or after sending commands to the database.
        /// </exception>
        /// <seealso cref="M:System.Data.Entity.DbContext.SaveChanges" />
        /// <returns>The number of objects written to the underlying database.</returns>
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
                        {
                            activeItem.Entity.ActiveFrom = DateTime.Now;
                        }
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
                        ChangeTracker.Entries<IAuditTrail>()
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
                            else
                            {
                                // ???
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
                        if (audit.Actions == AuditActions.I.ToString())
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
            catch (DbUpdateConcurrencyException ex
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

                throw new DbUpdateConcurrencyException(result.ToString(), ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Debug.WriteLine(ex);
            }

            return changes;
        }


        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <returns></returns>
        public System.Data.Entity.Database GetDatabase()
        {
            return Database;
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <returns></returns>
        public DbChangeTracker GetChangeTracker()
        {
            return ChangeTracker;
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <returns></returns>
        public DbContextConfiguration GetConfiguration()
        {
            return Configuration;
        }

        /// <inheritdoc />
        /// <summary>
        ///     Asynchronously saves all changes made in this context to the underlying database.
        /// </summary>
        /// <exception cref="T:System.Data.Entity.Infrastructure.DbUpdateException">
        ///     An error occurred sending updates to the database.
        /// </exception>
        /// <exception cref="T:System.Data.Entity.Infrastructure.DbUpdateConcurrencyException">
        ///     A database command did not affect the expected number of rows. This usually
        ///     indicates an optimistic concurrency violation; that is, a row has been changed
        ///     in the database since it was queried.
        /// </exception>
        /// <exception cref="T:System.Data.Entity.Validation.DbEntityValidationException">
        ///     The save was aborted because validation of entity property values failed.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        ///     An attempt was made to use unsupported behavior such as executing multiple
        ///     asynchronous commands concurrently on the same context instance.
        /// </exception>
        /// <exception cref="T:System.ObjectDisposedException">
        ///     The context or connection have been disposed.
        /// </exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///     Some error occurred attempting to process entities in the context either
        ///     before or after sending commands to the database.
        /// </exception>
        /// <seealso />
        /// <returns>
        ///     A task that represents the asynchronous save operation.  The
        ///     <see>Task.Result</see> contains the number of
        ///     objects written to the underlying database.
        /// </returns>
        public override async Task<int> SaveChangesAsync()
        {
            return await SaveChangesAsync(CancellationToken.None);
        }

        /// <inheritdoc />
        /// <summary>
        ///     Asynchronously saves all changes made in this context to the underlying database.
        /// </summary>
        /// <exception cref="T:System.Data.Entity.Infrastructure.DbUpdateException">
        ///     An error occurred sending updates to the database.
        /// </exception>
        /// <exception cref="T:System.Data.Entity.Infrastructure.DbUpdateConcurrencyException">
        ///     A database command did not affect the expected number of rows. This usually
        ///     indicates an optimistic concurrency violation; that is, a row has been changed
        ///     in the database since it was queried.
        /// </exception>
        /// <exception cref="T:System.Data.Entity.Validation.DbEntityValidationException">
        ///     The save was aborted because validation of entity property values failed.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        ///     An attempt was made to use unsupported behavior such as executing multiple
        ///     asynchronous commands concurrently on the same context instance.
        /// </exception>
        /// <exception cref="T:System.ObjectDisposedException">
        ///     The context or connection have been disposed.
        /// </exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///     Some error occurred attempting to process entities in the context either
        ///     before or after sending commands to the database.
        /// </exception>
        /// <seealso />
        /// <returns>
        ///     A task that represents the asynchronous save operation.  The
        ///     <see>Task.Result</see> contains the number of
        ///     objects written to the underlying database.
        /// </returns>
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
                        {
                            activeItem.Entity.ActiveFrom = DateTime.Now;
                        }
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
                        ChangeTracker.Entries<IAuditTrail>()
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
                            else
                            {
                                // ???
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
                        if (audit.Actions == AuditActions.I.ToString())
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
            catch (DbUpdateConcurrencyException ex) // This will fire only for entities that have the [RowVersion] property implemented...
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

                throw new DbUpdateConcurrencyException(result.ToString(), ex);
            }

            return changes;
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // free other managed objects that implement
                    // IDisposable only
                }

                // release any unmanaged objects
                // set object references to null

                _disposed = true;
            }

            base.Dispose(disposing);
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => !string.IsNullOrEmpty(type.Namespace))
                .Where(type => type.BaseType != null && type.BaseType.IsGenericType &&
                               type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));
            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }

            // Disable this default EF behaviour (we dont want to cascade delete many to many)
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            base.OnModelCreating((System.Data.Entity.DbModelBuilder)modelBuilder);
        }

        //private class DbInitializer<T> : DropCreateDatabaseAlways<DataContextAsync>
        //{
        //    protected override void Seed(DataContextAsync context)
        //    {
        //        base.Seed(context);
        //    }
        //}
        public void Dispose()
        {
        
        }
    }
}