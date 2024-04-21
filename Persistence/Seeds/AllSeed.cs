using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Persistence.Seeds
{
    public class AllSeed
    {
        public static async Task SeedAll(IServiceProvider _serviceProvider)
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<RickAndMortyDbContext>();
            var characters = await dbContext.Characters.ToListAsync();
            var episodes = await dbContext.Episodes.ToListAsync();
            var locations = await dbContext.Locations.ToListAsync();

            if (characters.Count == 0 && episodes.Count == 0 && locations.Count == 0)
            {
                //read from sql file
                var sql = "";
                try
                {
                    sql = File.ReadAllText("addAllDataOneTime.sql");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                if (!string.IsNullOrEmpty(sql))
                {
                    dbContext.Database.ExecuteSqlRaw(sql);
                    dbContext.SaveChanges();
                }
                //await dbContext.Users.AddAsync(user);
                //await dbContext.SaveChangesAsync();
            }
        }

    }
}
