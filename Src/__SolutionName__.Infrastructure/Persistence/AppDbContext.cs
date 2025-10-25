using __SolutionName__.Domain.Entities;
using __SolutionName__.Domain.Entities.Base;
using __SolutionName__.Infrastructure.Configurations;
using __SolutionName__.Infrastructure.Persistence.DataSeed;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace __SolutionName__.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        private readonly string _currentUser;

        // Optionally inject a user context service
        // public AppDbContext(DbContextOptions<AppDbContext> options, IUserContextService userContextService)
        //     : base(options)
        // {
        //     _currentUser = userContextService.GetCurrentUsername() ?? "system";
        // }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            _currentUser = "system"; // Default if not using IUserContextService
        }

        public DbSet<Flight> Flights { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply entity-specific config
            modelBuilder.ApplyConfiguration(new FlightConfiguration());

            // Seed data
            DataSeeder.Seed(modelBuilder);

            // Apply global query filter for soft delete
            ApplySoftDeleteFilter(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyAuditInfo();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ApplyAuditInfo()
        {
            var currentTime = DateTime.UtcNow;
            foreach (EntityEntry<BaseEntity> entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedOn = currentTime;
                        entry.Entity.CreatedBy = _currentUser;
                        entry.Entity.Status = true;
                        break;

                    case EntityState.Modified:
                        entry.Entity.UpdatedOn = currentTime;
                        entry.Entity.UpdatedBy = _currentUser;
                        break;

                    case EntityState.Deleted:
                        // Soft delete instead of physical delete
                        entry.State = EntityState.Modified;
                        entry.Entity.Status = false;
                        entry.Entity.UpdatedOn = currentTime;
                        entry.Entity.UpdatedBy = _currentUser;
                        break;
                }
            }
        }

        private static void ApplySoftDeleteFilter(ModelBuilder modelBuilder)
        {
            // Apply global query filter for every entity that inherits BaseEntity
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var method = typeof(AppDbContext)
                        .GetMethod(nameof(FilterStatusTrue), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!
                        .MakeGenericMethod(entityType.ClrType);

                    method.Invoke(null, new object[] { modelBuilder });
                }
            }
        }

        private static void FilterStatusTrue<TEntity>(ModelBuilder builder) where TEntity : BaseEntity
        {
            builder.Entity<TEntity>().HasQueryFilter(e => e.Status);
        }
    }
}