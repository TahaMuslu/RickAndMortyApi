using Apposite.Core.Entity;
using Apposite.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Apposite.Persistence
{
    public class RickAndMortyDbContext : DbContext
    {
        // Migration commands
        // cd .\Apposite.Persistence\
        // dotnet ef --startup-project ..\Apposite.Api\ migrations add <migration-name>
        // dotnet ef database update
        public RickAndMortyDbContext(DbContextOptions<RickAndMortyDbContext> options) : base(options)
        {
            this.Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>().ToTable("User");
        }

        public DbSet<User> Users { get; set; }
        

        public override int SaveChanges()
        {
            AddTimestamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AddTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }
        private void AddTimestamps()
        {
            var entities = ChangeTracker.Entries()
                .Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entity in entities)
            {
                var now = DateTime.UtcNow;

                if (entity.State == EntityState.Added)
                {
                    ((BaseEntity)entity.Entity).CreatedAt = now;
                }
                if (entity.State == EntityState.Modified)
                    ((BaseEntity)entity.Entity).UpdatedAt = now;
            }
        }
    }
}

