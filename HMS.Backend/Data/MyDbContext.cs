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
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Patient> Patients { get; set; } = null!;
        public DbSet<Admin> Admins { get; set; } = null!;
        public DbSet<Doctor> Doctors { get; set; } = null!;

        public DbSet<Department> Departments { get; set; } = null!;
        public DbSet<Log> Logs { get; set; } = null!;
        public DbSet<Notification> Notifications { get; set; } = null!;
        public DbSet<Procedure> Procedures { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<Room> Rooms { get; set; }

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
            modelBuilder.Entity<Log>().ToTable("Logs");
            modelBuilder.Entity<Notification>().ToTable("Notifications");
            modelBuilder.Entity<Procedure>().ToTable("Procedures");
            modelBuilder.Entity<MedicalRecord>().ToTable("MedicalRecords");
            modelBuilder.Entity<Room>().ToTable("Rooms");

            // Configure discriminator for TPT inheritance - stores user roles in Users table
            modelBuilder.Entity<User>()
                .HasDiscriminator<UserRole>("Role")
                .HasValue<User>(UserRole.Admin)    // base User can be Admin or generic user
                .HasValue<Patient>(UserRole.Patient)
                .HasValue<Doctor>(UserRole.Doctor);
            // I'm not sure about this ^^^ TODO: FIX THIS (look into it)

            // Configure relationships if needed explicitly (optional if conventions suffice)
            modelBuilder.Entity<Doctor>()
                .HasOne(d => d.Department)
                .WithMany()  // Or .WithMany(dep => dep.Doctors) if you add collection to Department
                .HasForeignKey(d => d.DepartmentId)
                .IsRequired();

            modelBuilder.Entity<Log>()
                .HasOne(l => l.User)
                .WithMany()  // Or .WithMany(u => u.Logs) if you add Logs collection to User
                .HasForeignKey(l => l.UserId)
                .IsRequired();

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany() // optionally .WithMany(u => u.Notifications) if you add navigation collection
                .HasForeignKey(n => n.UserId)
                .IsRequired();

            modelBuilder.Entity<Procedure>()
                .HasOne(p => p.Department)
                .WithMany() // or .WithMany(d => d.Procedures) if you add navigation collection on Department
                .HasForeignKey(p => p.DepartmentId)
                .IsRequired();

            modelBuilder.Entity<MedicalRecord>()
                .HasOne(m => m.Patient)
                .WithMany() // or .WithMany(p => p.MedicalRecords) if you add navigation property
                .HasForeignKey(m => m.PatientId)
                .IsRequired();

            modelBuilder.Entity<MedicalRecord>()
                .HasOne(m => m.Doctor)
                .WithMany() // or .WithMany(d => d.MedicalRecords)
                .HasForeignKey(m => m.DoctorId)
                .IsRequired();

            modelBuilder.Entity<MedicalRecord>()
                .HasOne(m => m.Procedure)
                .WithMany() // or .WithMany(pr => pr.MedicalRecords)
                .HasForeignKey(m => m.ProcedureId)
                .IsRequired();

            modelBuilder.Entity<Room>()
                .HasOne(r => r.Department)
                .WithMany() // Or .WithMany(d => d.Rooms) if you want to add navigation collection
                .HasForeignKey(r => r.DepartmentId)
                .IsRequired();

            // voodoo ^^^

            base.OnModelCreating(modelBuilder);
        }
    }
}
