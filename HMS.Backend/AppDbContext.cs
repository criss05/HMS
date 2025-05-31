using System.Numerics;
using HMS.Shared.Entities;
using HMS.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace HMS.Backend
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Patient> Patients { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
                : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Table-per-Type mapping
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Patient>().ToTable("Patients");
            //modelBuilder.Entity<Admin>().ToTable("Admins");
            //modelBuilder.Entity<Doctor>().ToTable("Doctors");

            base.OnModelCreating(modelBuilder);
        }
    }
}
