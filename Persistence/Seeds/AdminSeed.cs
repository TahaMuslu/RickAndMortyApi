using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

namespace Persistence.Seeds
{
    public class AdminSeed
    {
        public static async Task SeedAdmin(IServiceProvider _serviceProvider)
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<RickAndMortyDbContext>();
            var users = await dbContext.Users.ToListAsync();
            if (users.Count == 0)
            {
                User user = new User() { Name = "Admin", Email = "admin@admin.com", Password = CryptoService.HashPassword("P@ssw0rd") };
                await dbContext.Users.AddAsync(user);
                await dbContext.SaveChangesAsync();
            }
        }

    }
}
