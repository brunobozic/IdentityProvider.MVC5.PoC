using IdentityProvider.Repository.EFCore.Domain;
using IdentityProvider.Repository.EFCore.EFDataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;

namespace IdentityProvider.Repository.EFCore.DatabaseAudit
{
    public class DbAuditTrailFactory
    {
        private readonly DbContext _context;
        private AppDbContext appDbContext;

        public DbAuditTrailFactory(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public DbAuditTrail GetAudit(EntityEntry entry)
        {
            var audit = new DbAuditTrail
            {
                UserId =
                    1,
                // System.Web.HttpContext.Current.User.Identity.Name;
                // TODO: Change this line according to your needs
                TableName = GetTableName(entry),
                UpdatedAt = DateTime.Now,
                TableIdValue = GetKeyValue(entry)
            };

            switch (entry.State)
            {
                case EntityState.Added:
                    {
                        var newValues = new StringBuilder();
                        SetAddedProperties(entry, newValues);
                        audit.NewData = newValues.ToString();
                        audit.Actions = AuditActions.Insert.ToString();
                        break;
                    }
                case EntityState.Deleted:
                    {
                        var oldValues = new StringBuilder();
                        SetDeletedProperties(entry, oldValues);
                        audit.OldData = oldValues.ToString();
                        audit.Actions = AuditActions.Delete.ToString();
                        break;
                    }
                case EntityState.Modified:
                    {
                        var oldValues = new StringBuilder();
                        var newValues = new StringBuilder();
                        SetModifiedProperties(entry, oldValues, newValues);
                        audit.OldData = oldValues.ToString();
                        audit.NewData = newValues.ToString();
                        audit.Actions = AuditActions.Update.ToString();
                        break;
                    }

                case EntityState.Detached:
                    break;

                case EntityState.Unchanged:
                    break;
            }

            return audit;
        }

        private void SetAddedProperties(EntityEntry entry, StringBuilder newData)
        {
            foreach (var propertyName in entry.CurrentValues.Properties)
            {
                var newVal = entry.CurrentValues[propertyName];
                if (newVal != null)
                    newData.AppendFormat("{0}={1} || ", propertyName, newVal);
            }

            if (newData.Length > 0)
                newData = newData.Remove(newData.Length - 3, 3);
        }

        private void SetDeletedProperties(EntityEntry entry, StringBuilder oldData)
        {
            var dbValues = entry.GetDatabaseValues();
            foreach (var propertyName in dbValues.Properties)
            {
                var oldVal = dbValues[propertyName];
                if (oldVal != null)
                    oldData.AppendFormat("{0}={1} || ", propertyName, oldVal);
            }

            if (oldData.Length > 0)
                oldData = oldData.Remove(oldData.Length - 3, 3);
        }

        private void SetModifiedProperties(EntityEntry entry, StringBuilder oldData, StringBuilder newData)
        {
            var dbValues = entry.GetDatabaseValues();
            foreach (var propertyName in entry.OriginalValues.Properties)
            {
                var oldVal = dbValues[propertyName];
                var newVal = entry.CurrentValues[propertyName];
                if (oldVal != null && newVal != null && !Equals(oldVal, newVal))
                {
                    newData.AppendFormat("{0}={1} || ", propertyName, newVal);
                    oldData.AppendFormat("{0}={1} || ", propertyName, oldVal);
                }
            }

            if (oldData.Length > 0)
                oldData = oldData.Remove(oldData.Length - 3, 3);
            if (newData.Length > 0)
                newData = newData.Remove(newData.Length - 3, 3);
        }

        public long? GetKeyValue(EntityEntry entry)
        {
            var objectStateEntry =
                ((IObjectContextAdapter)_context).ObjectContext.ObjectStateManager.GetObjectStateEntry(entry.Entity);
            long id = 0;
            if (objectStateEntry.EntityKey.EntityKeyValues != null)
                id = Convert.ToInt64(objectStateEntry.EntityKey.EntityKeyValues[0].Value);

            return id;
        }

        private string GetTableName(EntityEntry dbEntry)
        {
            var tableAttr =
                dbEntry.Entity.GetType().GetCustomAttributes(typeof(TableAttribute), false)
                    .SingleOrDefault() as TableAttribute;
            var tableName = tableAttr != null ? tableAttr.Name : dbEntry.Entity.GetType().Name;
            return tableName;
        }
    }

    public enum AuditActions
    {
        Insert,
        Update,
        Delete
    }
}