using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace HMS.Backend.Data
{
    public class MyDbContextFactory : IDesignTimeDbContextFactory<MyDbContext>
    {
        public MyDbContext CreateDbContext(string[] args)
        {
            DotNetEnv.Env.Load(); // this line took 30mins to find out it was an issue

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var rawConnectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
            var dbHost = Environment.GetEnvironmentVariable("DB_HOST") ?? "IF_YOU_DONT_HAVE_.ENV_SETUP_THIS_CRASHES";
            var connectionString = rawConnectionString.Replace("{DB_HOST}", dbHost);

            var builder = new DbContextOptionsBuilder<MyDbContext>();
            builder.UseSqlServer(connectionString);

            return new MyDbContext(builder.Options);
        }
    }
}
