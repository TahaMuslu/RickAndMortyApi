using Core.Entity;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Persistence
{
    public class RickAndMortyDbContext : DbContext
    {
        // Migration commands
        // cd .\Persistence\
        // dotnet ef --startup-project ..\Api\ migrations add <migration-name>
        // dotnet ef database update
        public RickAndMortyDbContext(DbContextOptions<RickAndMortyDbContext> options) : base(options)
        {
            this.Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>()
                .ToTable("Users")
                .Property(u => u.Id)
            .ValueGeneratedOnAdd();

            builder.Entity<Character>(entity =>
            {
                entity.ToTable("Characters");

                entity.Property(c => c.Id).ValueGeneratedOnAdd();

                entity.HasOne(d => d.Origin)
                      .WithMany()
                      .HasForeignKey("OriginId")
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Location)
                      .WithMany(l => l.Characters)
                      .HasForeignKey("LocationId")
                      .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Location>()
                .ToTable("Locations")
                .Property(l => l.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<Episode>()
                .ToTable("Episodes")
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Episode> Episodes { get; set; }

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

