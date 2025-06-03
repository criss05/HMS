using Microsoft.EntityFrameworkCore.Migrations;
using System;
using HMS.Shared.Enums;

namespace HMS.Backend.Migrations
{
    public partial class Seeder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Departments
            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Name" },
                values: new object[,]
                {
                    { "Cardiology" },
                    { "Neurology" },
                    { "Pediatrics" },
                    { "General Surgery" },
                    { "Orthopedics" },
                    { "Ophthalmology" },
                    { "ENT" },
                    { "Dermatology" }
                });

            // Admin - First insert into Users table
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Email", "Password", "Name", "CNP", "PhoneNumber", "CreatedAt", "Role" },
                values: new object[,]
                {
                    {
                        "admin@hms.com",
                        "admin123",
                        "Administrator",
                        "1234567890123",
                        "0712345678",
                        DateTime.UtcNow,
                        (int)UserRole.Admin
                    }
                });

            // Then insert into Admins table with the same Id
            migrationBuilder.Sql(@"
                INSERT INTO Admins (Id)
                SELECT Id FROM Users WHERE Email = 'admin@hms.com'
            ");

            // Doctors - First insert into Users table
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Email", "Password", "Name", "CNP", "PhoneNumber", "CreatedAt", "Role" },
                values: new object[,]
                {
                    {
                        "ion.popescu@hms.com",
                        "doctor123",
                        "Dr. Ion Popescu",
                        "1780512345678",
                        "0723456789",
                        DateTime.UtcNow,
                        (int)UserRole.Doctor
                    },
                    {
                        "maria.ionescu@hms.com",
                        "doctor123",
                        "Dr. Maria Ionescu",
                        "2810623456789",
                        "0734567890",
                        DateTime.UtcNow,
                        (int)UserRole.Doctor
                    },
                    {
                        "andrei.popa@hms.com",
                        "doctor123",
                        "Dr. Andrei Popa",
                        "1750734567890",
                        "0745678901",
                        DateTime.UtcNow,
                        (int)UserRole.Doctor
                    }
                });

            // Then insert into Doctors table with additional doctor-specific fields
            migrationBuilder.Sql(@"
                INSERT INTO Doctors (Id, DepartmentId, YearsOfExperience, LicenseNumber)
                SELECT u.Id, 1, 15, 'MD12345'
                FROM Users u
                WHERE u.Email = 'ion.popescu@hms.com';

                INSERT INTO Doctors (Id, DepartmentId, YearsOfExperience, LicenseNumber)
                SELECT u.Id, 2, 12, 'MD12346'
                FROM Users u
                WHERE u.Email = 'maria.ionescu@hms.com';

                INSERT INTO Doctors (Id, DepartmentId, YearsOfExperience, LicenseNumber)
                SELECT u.Id, 3, 8, 'MD12347'
                FROM Users u
                WHERE u.Email = 'andrei.popa@hms.com';
            ");

            // Patients - First insert into Users table
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Email", "Password", "Name", "CNP", "PhoneNumber", "CreatedAt", "Role" },
                values: new object[,]
                {
                    {
                        "george.dumitrescu@email.com",
                        "patient123",
                        "George Dumitrescu",
                        "1890845678901",
                        "0756789012",
                        DateTime.UtcNow,
                        (int)UserRole.Patient
                    },
                    {
                        "elena.stan@email.com",
                        "patient123",
                        "Elena Stan",
                        "2920956789012",
                        "0767890123",
                        DateTime.UtcNow,
                        (int)UserRole.Patient
                    },
                    {
                        "mihai.constantin@email.com",
                        "patient123",
                        "Mihai Constantin",
                        "1850667890123",
                        "0778901234",
                        DateTime.UtcNow,
                        (int)UserRole.Patient
                    }
                });

            // Then insert into Patients table with additional patient-specific fields
            migrationBuilder.Sql(@"
                INSERT INTO Patients (Id, BloodType, EmergencyContact, Allergies, Weight, Height, BirthDate, Address)
                SELECT u.Id, 0, '0767890123', 'Polen, Penicilina', 75.5, 178.0, '1989-08-04', 'Str. Primaverii nr. 10, Bucuresti'
                FROM Users u
                WHERE u.Email = 'george.dumitrescu@email.com';

                INSERT INTO Patients (Id, BloodType, EmergencyContact, Allergies, Weight, Height, BirthDate, Address)
                SELECT u.Id, 3, '0778901234', 'Lactate', 62.0, 165.0, '1992-09-05', 'Str. Victoriei nr. 25, Bucuresti'
                FROM Users u
                WHERE u.Email = 'elena.stan@email.com';

                INSERT INTO Patients (Id, BloodType, EmergencyContact, Allergies, Weight, Height, BirthDate, Address)
                SELECT u.Id, 4, '0789012345', '', 85.0, 182.0, '1985-06-06', 'Str. Libertatii nr. 15, Bucuresti'
                FROM Users u
                WHERE u.Email = 'mihai.constantin@email.com';
            ");

            // Equipment
            migrationBuilder.InsertData(
                table: "Equipments",
                columns: new[] { "Name", "Specification", "Type", "Stock" },
                values: new object[,]
                {
                    { "Electrocardiograph", "Model: ECG-2000, Manufacturer: Philips", "Diagnostic", 5 },
                    { "Ultrasound Machine", "Model: Voluson E10, Manufacturer: GE Healthcare", "Imaging", 3 },
                    { "Surgical Kit", "Complete set of sterile surgical instruments", "Surgery", 10 },
                    { "Vital Signs Monitor", "Model: IntelliVue MX450, Manufacturer: Philips", "Monitoring", 8 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Admins");
            migrationBuilder.Sql("DELETE FROM Doctors");
            migrationBuilder.Sql("DELETE FROM Patients");
            migrationBuilder.Sql("DELETE FROM Users");
            migrationBuilder.Sql("DELETE FROM Departments");
            migrationBuilder.Sql("DELETE FROM Equipments");
        }
    }
}
