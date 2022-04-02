using Itbeard.Data;
using Microsoft.EntityFrameworkCore;

namespace Itbeard.Shortener.Common
{
    public static class DatabaseContextServiceExtension
    {
        public static void AddCustomSqlContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(o => o.UseSqlServer(connectionString));
            services.BuildServiceProvider().GetService<ApplicationDbContext>().Database.Migrate(); // DB automigration on start enable
        }
    }
}