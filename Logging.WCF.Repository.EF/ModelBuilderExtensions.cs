using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;

namespace Logging.WCF.Repository.EF
{
    public static class ModelBuilderExtensions
    {

        //public static void SetUpSoftDeletableColumnDefaultValue(this ModelBuilder modelBuilder)
        //{
        //    var entityTypes = modelBuilder.Model.GetEntityTypes().Select(p => p.ClrType)
        //        .Where(c => typeof(ISoftDeletable).IsAssignableFrom(c));
        //    foreach (var entityType in entityTypes)
        //        modelBuilder.Entity(entityType).Property<bool>(nameof(ISoftDeletable.Deleted)).HasDefaultValue(false);
        //}

        public static void DisableCascadeDelete(this ModelBuilder modelBuilder)
        {
            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;
        }
        public static void LoadAllEntityConfigurations(this ModelBuilder builder)
        {
            var dalAssembly = Assembly.GetAssembly(typeof(ModelBuilderExtensions));
            var configurations = dalAssembly.GetTypes()
                .Where(type => type.GetInterfaces().Any(
                    i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)))
                .ToList();

            foreach (var configurationInstance in configurations.Select(configuration =>
                Activator.CreateInstance(configuration)))
                builder.ApplyConfiguration((dynamic)configurationInstance);
        }
    }
}
