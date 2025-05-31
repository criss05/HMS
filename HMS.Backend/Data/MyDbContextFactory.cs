using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace HMS.Backend.Data
{
    public class MyDbContextFactory : IDesignTimeDbContextFactory<MyDbContext>
    {
        public MyDbContext CreateDbContext(string[] args)
        {
            // Build configuration to get the connection string
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())  // Adjust if needed
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<MyDbContext>();

            // Use your provider and connection string here:
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            builder.UseSqlServer(connectionString);

            return new MyDbContext(builder.Options);
        }
    }
}
