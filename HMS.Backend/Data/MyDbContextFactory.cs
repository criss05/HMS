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
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var rawConnectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
            var dbHost = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost\\SQLEXPRESS";
            var connectionString = rawConnectionString.Replace("{DB_HOST}", dbHost);

            var builder = new DbContextOptionsBuilder<MyDbContext>();
            builder.UseSqlServer(connectionString);

            return new MyDbContext(builder.Options);
        }
    }
}
