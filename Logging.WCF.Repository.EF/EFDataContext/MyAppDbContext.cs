
using Microsoft.EntityFrameworkCore;
using StructureMap;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Logging.WCF.Repository.EF.EFDataContext
{
    public class MyAppDbContext : DbContext
    {
        [DefaultConstructor]
        public MyAppDbContext(DbContextOptions<MyAppDbContext> options, string connectionString) : base(options)
        {
            // Database.Migrate();
            _connectionString = connectionString;
        }
        public MyAppDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        //public MyAppDbContext(string connectionString)
        //{
        //    var builder = new DbContextOptionsBuilder<MyAppDbContext>().UseSqlServer(connectionString);         
        //    var context = new MyAppDbContext(builder.Options);
        //}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<DatabaseLog>();
            // modelBuilder.SetUpSoftDeletableColumnDefaultValue();
            modelBuilder.DisableCascadeDelete();
            modelBuilder.LoadAllEntityConfigurations();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public override int SaveChanges()
        {
            throw new InvalidOperationException(
                "Use only the SaveChangesAsync() because synchronous saving is not supported!");
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var changes = 0;

            // Do the audit trails
            var currentDateTime = DateTime.Now;

            var currentApplicationUserId = 1;

            // TODO: fetch user Id from the facade
            currentApplicationUserId = 1;

            try
            {

                changes = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);


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


                // Update the values of the entity that failed to save from the store 
                ex.Entries.Single().Reload();

                var result = new StringBuilder();
                result.Append("The record you attempted to edit "
                              + "was modified by another user after you got the original value. The "
                              + "edit operation was canceled and the current values in the database "
                              + "have been displayed.");

                // throw new DbUpdateConcurrencyException(result.ToString(), ex.Data);
            }

            return changes;
        }

        public DbSet<DatabaseLog> DbLog { get; set; }
        public string _connectionString { get; }
    }
}