using System.Numerics;
using HMS.Shared.Entities;
using HMS.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace HMS.Backend.Data
{
    // It's funny cuz it looks like a beginner tutorial name, but it's actually a design choice.
    // Ah, you dont find it funny? Well, I do.
    // The voices are getting louder, I need to stop talking to myself in comments.
    public class MyDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Patient> Patients { get; set; }

        public MyDbContext(DbContextOptions<MyDbContext> options)
                : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Table-per-Type mapping
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Patient>().ToTable("Patients");
            modelBuilder.Entity<Admin>().ToTable("Admins");
            modelBuilder.Entity<Doctor>().ToTable("Doctors");

            modelBuilder.Entity<Department>().ToTable("Departments");

            base.OnModelCreating(modelBuilder);
        }
    }
}
