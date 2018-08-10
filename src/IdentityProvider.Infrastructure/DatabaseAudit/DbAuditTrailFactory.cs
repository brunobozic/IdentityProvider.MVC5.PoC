using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;

namespace IdentityProvider.Infrastructure.DatabaseAudit
{
    public class DbAuditTrailFactory
    {
        private readonly DbContext _context;

        public DbAuditTrailFactory(DbContext context)
        {
            _context = context;
        }

        public DbAuditTrail GetAudit(DbEntityEntry entry)
        {
            var audit = new DbAuditTrail
            {
                UserId =
                    1, // System.Web.HttpContext.Current.User.Identity.Name; //Change this line according to your needs
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
                    audit.Actions = AuditActions.I.ToString();
                    break;
                }
                case EntityState.Deleted:
                {
                    var oldValues = new StringBuilder();
                    SetDeletedProperties(entry, oldValues);
                    audit.OldData = oldValues.ToString();
                    audit.Actions = AuditActions.D.ToString();
                    break;
                }
                case EntityState.Modified:
                {
                    var oldValues = new StringBuilder();
                    var newValues = new StringBuilder();
                    SetModifiedProperties(entry, oldValues, newValues);
                    audit.OldData = oldValues.ToString();
                    audit.NewData = newValues.ToString();
                    audit.Actions = AuditActions.U.ToString();
                    break;
                }

                case EntityState.Detached:
                    break;
                case EntityState.Unchanged:
                    break;
                default:
                    break;
            }

            return audit;
        }

        private void SetAddedProperties(DbEntityEntry entry, StringBuilder newData)
        {
            foreach (var propertyName in entry.CurrentValues.PropertyNames)
            {
                var newVal = entry.CurrentValues[propertyName];
                if (newVal != null)
                    newData.AppendFormat("{0}={1} || ", propertyName, newVal);
            }
            if (newData.Length > 0)
                newData = newData.Remove(newData.Length - 3, 3);
        }

        private void SetDeletedProperties(DbEntityEntry entry, StringBuilder oldData)
        {
            var dbValues = entry.GetDatabaseValues();
            foreach (var propertyName in dbValues.PropertyNames)
            {
                var oldVal = dbValues[propertyName];
                if (oldVal != null)
                    oldData.AppendFormat("{0}={1} || ", propertyName, oldVal);
            }
            if (oldData.Length > 0)
                oldData = oldData.Remove(oldData.Length - 3, 3);
        }

        private void SetModifiedProperties(DbEntityEntry entry, StringBuilder oldData, StringBuilder newData)
        {
            var dbValues = entry.GetDatabaseValues();
            foreach (var propertyName in entry.OriginalValues.PropertyNames)
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

        public long? GetKeyValue(DbEntityEntry entry)
        {
            var objectStateEntry =
                ((IObjectContextAdapter) _context).ObjectContext.ObjectStateManager.GetObjectStateEntry(entry.Entity);
            long id = 0;
            if (objectStateEntry.EntityKey.EntityKeyValues != null)
                id = Convert.ToInt64(objectStateEntry.EntityKey.EntityKeyValues[0].Value);

            return id;
        }

        private string GetTableName(DbEntityEntry dbEntry)
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
        I,
        U,
        D
    }
}