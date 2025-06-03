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
                    { "Dermatology" },
                    { "Internal Medicine" },
                    { "Emergency Medicine" },
                    { "Radiology" },
                    { "Psychiatry" }
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
                    },
                    {
                        "system.admin@hms.com",
                        "admin123",
                        "System Administrator",
                        "1234567890124",
                        "0712345679",
                        DateTime.UtcNow,
                        (int)UserRole.Admin
                    }
                });

            // Then insert into Admins table with the same Id
            migrationBuilder.Sql(@"
                INSERT INTO Admins (Id)
                SELECT Id FROM Users WHERE Email LIKE '%@hms.com' AND Role = 0
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
                    },
                    {
                        "elena.dumitrescu@hms.com",
                        "doctor123",
                        "Dr. Elena Dumitrescu",
                        "2800890123456",
                        "0756789012",
                        DateTime.UtcNow,
                        (int)UserRole.Doctor
                    },
                    {
                        "mihai.stanescu@hms.com",
                        "doctor123",
                        "Dr. Mihai Stanescu",
                        "1771001234567",
                        "0767890123",
                        DateTime.UtcNow,
                        (int)UserRole.Doctor
                    },
                    {
                        "alexandra.georgescu@hms.com",
                        "doctor123",
                        "Dr. Alexandra Georgescu",
                        "2820112345678",
                        "0778901234",
                        DateTime.UtcNow,
                        (int)UserRole.Doctor
                    },
                    {
                        "stefan.munteanu@hms.com",
                        "doctor123",
                        "Dr. Stefan Munteanu",
                        "1760223456789",
                        "0789012345",
                        DateTime.UtcNow,
                        (int)UserRole.Doctor
                    },
                    {
                        "cristina.radulescu@hms.com",
                        "doctor123",
                        "Dr. Cristina Radulescu",
                        "2790334567890",
                        "0790123456",
                        DateTime.UtcNow,
                        (int)UserRole.Doctor
                    }
                });

            // Then insert into Doctors table with additional doctor-specific fields
            migrationBuilder.Sql(@"
                -- Ion Popescu -> Cardiology
                INSERT INTO Doctors (Id, DepartmentId, YearsOfExperience, LicenseNumber)
                SELECT u.Id, d.Id, 15, 'MD12345'
                FROM Users u
                CROSS JOIN Departments d
                WHERE u.Email = 'ion.popescu@hms.com'
                AND d.Name = 'Cardiology';

                -- Maria Ionescu -> Neurology
                INSERT INTO Doctors (Id, DepartmentId, YearsOfExperience, LicenseNumber)
                SELECT u.Id, d.Id, 12, 'MD12346'
                FROM Users u
                CROSS JOIN Departments d
                WHERE u.Email = 'maria.ionescu@hms.com'
                AND d.Name = 'Neurology';

                -- Andrei Popa -> Pediatrics
                INSERT INTO Doctors (Id, DepartmentId, YearsOfExperience, LicenseNumber)
                SELECT u.Id, d.Id, 8, 'MD12347'
                FROM Users u
                CROSS JOIN Departments d
                WHERE u.Email = 'andrei.popa@hms.com'
                AND d.Name = 'Pediatrics';

                -- Elena Dumitrescu -> General Surgery
                INSERT INTO Doctors (Id, DepartmentId, YearsOfExperience, LicenseNumber)
                SELECT u.Id, d.Id, 20, 'MD12348'
                FROM Users u
                CROSS JOIN Departments d
                WHERE u.Email = 'elena.dumitrescu@hms.com'
                AND d.Name = 'General Surgery';

                -- Mihai Stanescu -> Orthopedics
                INSERT INTO Doctors (Id, DepartmentId, YearsOfExperience, LicenseNumber)
                SELECT u.Id, d.Id, 10, 'MD12349'
                FROM Users u
                CROSS JOIN Departments d
                WHERE u.Email = 'mihai.stanescu@hms.com'
                AND d.Name = 'Orthopedics';

                -- Alexandra Georgescu -> Ophthalmology
                INSERT INTO Doctors (Id, DepartmentId, YearsOfExperience, LicenseNumber)
                SELECT u.Id, d.Id, 7, 'MD12350'
                FROM Users u
                CROSS JOIN Departments d
                WHERE u.Email = 'alexandra.georgescu@hms.com'
                AND d.Name = 'Ophthalmology';

                -- Stefan Munteanu -> ENT
                INSERT INTO Doctors (Id, DepartmentId, YearsOfExperience, LicenseNumber)
                SELECT u.Id, d.Id, 14, 'MD12351'
                FROM Users u
                CROSS JOIN Departments d
                WHERE u.Email = 'stefan.munteanu@hms.com'
                AND d.Name = 'ENT';

                -- Cristina Radulescu -> Dermatology
                INSERT INTO Doctors (Id, DepartmentId, YearsOfExperience, LicenseNumber)
                SELECT u.Id, d.Id, 9, 'MD12352'
                FROM Users u
                CROSS JOIN Departments d
                WHERE u.Email = 'cristina.radulescu@hms.com'
                AND d.Name = 'Dermatology';
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
                    },
                    {
                        "ana.popescu@email.com",
                        "patient123",
                        "Ana Popescu",
                        "2911178901234",
                        "0789012345",
                        DateTime.UtcNow,
                        (int)UserRole.Patient
                    },
                    {
                        "radu.ionescu@email.com",
                        "patient123",
                        "Radu Ionescu",
                        "1871289012345",
                        "0790123456",
                        DateTime.UtcNow,
                        (int)UserRole.Patient
                    },
                    {
                        "maria.popa@email.com",
                        "patient123",
                        "Maria Popa",
                        "2900190123456",
                        "0723123456",
                        DateTime.UtcNow,
                        (int)UserRole.Patient
                    },
                    {
                        "adrian.munteanu@email.com",
                        "patient123",
                        "Adrian Munteanu",
                        "1830201234567",
                        "0734234567",
                        DateTime.UtcNow,
                        (int)UserRole.Patient
                    },
                    {
                        "ioana.vasilescu@email.com",
                        "patient123",
                        "Ioana Vasilescu",
                        "2950312345678",
                        "0745345678",
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

                INSERT INTO Patients (Id, BloodType, EmergencyContact, Allergies, Weight, Height, BirthDate, Address)
                SELECT u.Id, 1, '0790123456', 'Nuts, Shellfish', 58.0, 162.0, '1991-11-17', 'Str. Unirii nr. 7, Bucuresti'
                FROM Users u
                WHERE u.Email = 'ana.popescu@email.com';

                INSERT INTO Patients (Id, BloodType, EmergencyContact, Allergies, Weight, Height, BirthDate, Address)
                SELECT u.Id, 2, '0723123456', 'Aspirin', 80.0, 175.0, '1987-12-08', 'Bulevardul Decebal nr. 12, Bucuresti'
                FROM Users u
                WHERE u.Email = 'radu.ionescu@email.com';

                INSERT INTO Patients (Id, BloodType, EmergencyContact, Allergies, Weight, Height, BirthDate, Address)
                SELECT u.Id, 5, '0734234567', 'Ibuprofen', 65.0, 170.0, '1990-01-01', 'Str. Dorobanti nr. 20, Bucuresti'
                FROM Users u
                WHERE u.Email = 'maria.popa@email.com';

                INSERT INTO Patients (Id, BloodType, EmergencyContact, Allergies, Weight, Height, BirthDate, Address)
                SELECT u.Id, 6, '0745345678', '', 90.0, 185.0, '1983-02-02', 'Calea Mosilor nr. 30, Bucuresti'
                FROM Users u
                WHERE u.Email = 'adrian.munteanu@email.com';

                INSERT INTO Patients (Id, BloodType, EmergencyContact, Allergies, Weight, Height, BirthDate, Address)
                SELECT u.Id, 7, '0756456789', 'Pollen', 55.0, 160.0, '1995-03-03', 'Str. Calarasi nr. 40, Bucuresti'
                FROM Users u
                WHERE u.Email = 'ioana.vasilescu@email.com';
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
                    { "Vital Signs Monitor", "Model: IntelliVue MX450, Manufacturer: Philips", "Monitoring", 8 },
                    { "MRI Scanner", "Model: MAGNETOM Vida, Manufacturer: Siemens", "Imaging", 1 },
                    { "CT Scanner", "Model: Revolution CT, Manufacturer: GE Healthcare", "Imaging", 2 },
                    { "Defibrillator", "Model: LIFEPAK 15, Manufacturer: Stryker", "Emergency", 6 },
                    { "Anesthesia Machine", "Model: Perseus A500, Manufacturer: Draeger", "Surgery", 4 },
                    { "Patient Monitor", "Model: B450, Manufacturer: GE Healthcare", "Monitoring", 15 },
                    { "Ventilator", "Model: Servo-u, Manufacturer: Getinge", "Critical Care", 7 }
                });

            // Rooms
            migrationBuilder.Sql(@"
                -- Cardiology rooms
                INSERT INTO Rooms (DepartmentId, Capacity)
                SELECT d.Id, 2
                FROM Departments d
                WHERE d.Name = 'Cardiology';

                INSERT INTO Rooms (DepartmentId, Capacity)
                SELECT d.Id, 3
                FROM Departments d
                WHERE d.Name = 'Cardiology';

                -- Neurology rooms
                INSERT INTO Rooms (DepartmentId, Capacity)
                SELECT d.Id, 2
                FROM Departments d
                WHERE d.Name = 'Neurology';

                INSERT INTO Rooms (DepartmentId, Capacity)
                SELECT d.Id, 4
                FROM Departments d
                WHERE d.Name = 'Neurology';

                -- Pediatrics rooms
                INSERT INTO Rooms (DepartmentId, Capacity)
                SELECT d.Id, 3
                FROM Departments d
                WHERE d.Name = 'Pediatrics';

                INSERT INTO Rooms (DepartmentId, Capacity)
                SELECT d.Id, 2
                FROM Departments d
                WHERE d.Name = 'Pediatrics';

                -- General Surgery rooms
                INSERT INTO Rooms (DepartmentId, Capacity)
                SELECT d.Id, 4
                FROM Departments d
                WHERE d.Name = 'General Surgery';

                INSERT INTO Rooms (DepartmentId, Capacity)
                SELECT d.Id, 5
                FROM Departments d
                WHERE d.Name = 'General Surgery';

                -- Orthopedics rooms
                INSERT INTO Rooms (DepartmentId, Capacity)
                SELECT d.Id, 2
                FROM Departments d
                WHERE d.Name = 'Orthopedics';

                INSERT INTO Rooms (DepartmentId, Capacity)
                SELECT d.Id, 3
                FROM Departments d
                WHERE d.Name = 'Orthopedics';

                -- Ophthalmology room
                INSERT INTO Rooms (DepartmentId, Capacity)
                SELECT d.Id, 2
                FROM Departments d
                WHERE d.Name = 'Ophthalmology';

                -- ENT room
                INSERT INTO Rooms (DepartmentId, Capacity)
                SELECT d.Id, 2
                FROM Departments d
                WHERE d.Name = 'ENT';

                -- Dermatology room
                INSERT INTO Rooms (DepartmentId, Capacity)
                SELECT d.Id, 2
                FROM Departments d
                WHERE d.Name = 'Dermatology';

                -- Internal Medicine room
                INSERT INTO Rooms (DepartmentId, Capacity)
                SELECT d.Id, 3
                FROM Departments d
                WHERE d.Name = 'Internal Medicine';

                -- Emergency Medicine room
                INSERT INTO Rooms (DepartmentId, Capacity)
                SELECT d.Id, 6
                FROM Departments d
                WHERE d.Name = 'Emergency Medicine';
            ");

            // Procedures
            migrationBuilder.Sql(@"
                -- Cardiology procedures
                INSERT INTO Procedures (DepartmentId, Name, Duration)
                SELECT d.Id, 'Electrocardiogram', '00:30:00'
                FROM Departments d
                WHERE d.Name = 'Cardiology';

                INSERT INTO Procedures (DepartmentId, Name, Duration)
                SELECT d.Id, 'Echocardiogram', '01:00:00'
                FROM Departments d
                WHERE d.Name = 'Cardiology';

                INSERT INTO Procedures (DepartmentId, Name, Duration)
                SELECT d.Id, 'Stress Test', '01:30:00'
                FROM Departments d
                WHERE d.Name = 'Cardiology';

                -- Neurology procedures
                INSERT INTO Procedures (DepartmentId, Name, Duration)
                SELECT d.Id, 'Neurological Examination', '01:00:00'
                FROM Departments d
                WHERE d.Name = 'Neurology';

                INSERT INTO Procedures (DepartmentId, Name, Duration)
                SELECT d.Id, 'EEG', '01:30:00'
                FROM Departments d
                WHERE d.Name = 'Neurology';

                -- Pediatrics procedures
                INSERT INTO Procedures (DepartmentId, Name, Duration)
                SELECT d.Id, 'Pediatric Consultation', '00:30:00'
                FROM Departments d
                WHERE d.Name = 'Pediatrics';

                INSERT INTO Procedures (DepartmentId, Name, Duration)
                SELECT d.Id, 'Vaccination', '00:15:00'
                FROM Departments d
                WHERE d.Name = 'Pediatrics';

                -- General Surgery procedures
                INSERT INTO Procedures (DepartmentId, Name, Duration)
                SELECT d.Id, 'Appendectomy', '02:00:00'
                FROM Departments d
                WHERE d.Name = 'General Surgery';

                INSERT INTO Procedures (DepartmentId, Name, Duration)
                SELECT d.Id, 'Hernia Repair', '01:30:00'
                FROM Departments d
                WHERE d.Name = 'General Surgery';

                -- Orthopedics procedures
                INSERT INTO Procedures (DepartmentId, Name, Duration)
                SELECT d.Id, 'Joint Replacement', '03:00:00'
                FROM Departments d
                WHERE d.Name = 'Orthopedics';

                INSERT INTO Procedures (DepartmentId, Name, Duration)
                SELECT d.Id, 'Fracture Treatment', '01:00:00'
                FROM Departments d
                WHERE d.Name = 'Orthopedics';

                -- Ophthalmology procedures
                INSERT INTO Procedures (DepartmentId, Name, Duration)
                SELECT d.Id, 'Eye Examination', '00:30:00'
                FROM Departments d
                WHERE d.Name = 'Ophthalmology';

                INSERT INTO Procedures (DepartmentId, Name, Duration)
                SELECT d.Id, 'Cataract Surgery', '01:00:00'
                FROM Departments d
                WHERE d.Name = 'Ophthalmology';

                -- ENT procedures
                INSERT INTO Procedures (DepartmentId, Name, Duration)
                SELECT d.Id, 'Hearing Test', '00:45:00'
                FROM Departments d
                WHERE d.Name = 'ENT';

                INSERT INTO Procedures (DepartmentId, Name, Duration)
                SELECT d.Id, 'Tonsillectomy', '01:30:00'
                FROM Departments d
                WHERE d.Name = 'ENT';

                -- Dermatology procedures
                INSERT INTO Procedures (DepartmentId, Name, Duration)
                SELECT d.Id, 'Skin Examination', '00:30:00'
                FROM Departments d
                WHERE d.Name = 'Dermatology';

                INSERT INTO Procedures (DepartmentId, Name, Duration)
                SELECT d.Id, 'Skin Biopsy', '00:45:00'
                FROM Departments d
                WHERE d.Name = 'Dermatology';
            ");

            // Shifts
            migrationBuilder.InsertData(
                table: "Shifts",
                columns: new[] { "Date", "StartTime", "EndTime" },
                values: new object[,]
                {
                    { new DateOnly(2024, 6, 10), new TimeOnly(8, 0), new TimeOnly(16, 0) },
                    { new DateOnly(2024, 6, 10), new TimeOnly(16, 0), new TimeOnly(0, 0) },
                    { new DateOnly(2024, 6, 10), new TimeOnly(0, 0), new TimeOnly(8, 0) },
                    { new DateOnly(2024, 6, 11), new TimeOnly(8, 0), new TimeOnly(16, 0) },
                    { new DateOnly(2024, 6, 11), new TimeOnly(16, 0), new TimeOnly(0, 0) },
                    { new DateOnly(2024, 6, 11), new TimeOnly(0, 0), new TimeOnly(8, 0) }
                });

            // Schedules (will be added after shifts are created)
            migrationBuilder.Sql(@"
                -- Get doctor IDs
                DECLARE @doctor1Id int = (SELECT Id FROM Users WHERE Email = 'ion.popescu@hms.com');
                DECLARE @doctor2Id int = (SELECT Id FROM Users WHERE Email = 'maria.ionescu@hms.com');
                DECLARE @doctor3Id int = (SELECT Id FROM Users WHERE Email = 'andrei.popa@hms.com');
                DECLARE @doctor4Id int = (SELECT Id FROM Users WHERE Email = 'elena.dumitrescu@hms.com');

                -- Get shift IDs
                DECLARE @shift1Id int = (SELECT Id FROM Shifts WHERE Date = '2024-06-10' AND StartTime = '08:00:00');
                DECLARE @shift2Id int = (SELECT Id FROM Shifts WHERE Date = '2024-06-10' AND StartTime = '16:00:00');
                DECLARE @shift3Id int = (SELECT Id FROM Shifts WHERE Date = '2024-06-10' AND StartTime = '00:00:00');
                DECLARE @shift4Id int = (SELECT Id FROM Shifts WHERE Date = '2024-06-11' AND StartTime = '08:00:00');
                DECLARE @shift5Id int = (SELECT Id FROM Shifts WHERE Date = '2024-06-11' AND StartTime = '16:00:00');
                DECLARE @shift6Id int = (SELECT Id FROM Shifts WHERE Date = '2024-06-11' AND StartTime = '00:00:00');

                -- Insert schedules
                INSERT INTO Schedules (DoctorId, ShiftId)
                VALUES
                    (@doctor1Id, @shift1Id),
                    (@doctor2Id, @shift2Id),
                    (@doctor3Id, @shift3Id),
                    (@doctor4Id, @shift4Id),
                    (@doctor1Id, @shift5Id),
                    (@doctor2Id, @shift6Id);
            ");

            // Appointments
            migrationBuilder.Sql(@"
                -- Get IDs for relationships
                DECLARE @patient1Id int = (SELECT Id FROM Users WHERE Email = 'george.dumitrescu@email.com');
                DECLARE @patient2Id int = (SELECT Id FROM Users WHERE Email = 'elena.stan@email.com');
                DECLARE @patient3Id int = (SELECT Id FROM Users WHERE Email = 'mihai.constantin@email.com');
                DECLARE @doctor1Id int = (SELECT Id FROM Users WHERE Email = 'ion.popescu@hms.com');
                DECLARE @doctor2Id int = (SELECT Id FROM Users WHERE Email = 'maria.ionescu@hms.com');
                DECLARE @procedure1Id int = (SELECT Id FROM Procedures WHERE Name = 'Electrocardiogram');
                DECLARE @procedure2Id int = (SELECT Id FROM Procedures WHERE Name = 'Neurological Examination');

                -- Get room IDs using department names
                DECLARE @room1Id int = (
                    SELECT TOP 1 r.Id 
                    FROM Rooms r
                    JOIN Departments d ON r.DepartmentId = d.Id
                    WHERE d.Name = 'Cardiology'
                );

                DECLARE @room2Id int = (
                    SELECT TOP 1 r.Id 
                    FROM Rooms r
                    JOIN Departments d ON r.DepartmentId = d.Id
                    WHERE d.Name = 'Neurology'
                );

                -- Verify that we have all required IDs
                IF @patient1Id IS NULL OR @patient2Id IS NULL OR @patient3Id IS NULL OR
                   @doctor1Id IS NULL OR @doctor2Id IS NULL OR
                   @procedure1Id IS NULL OR @procedure2Id IS NULL OR
                   @room1Id IS NULL OR @room2Id IS NULL
                BEGIN
                    THROW 51000, 'One or more required IDs are missing for appointments', 1;
                END

                -- Insert appointments
                INSERT INTO Appointments (PatientId, DoctorId, ProcedureId, RoomId, DateTime)
                VALUES
                    (@patient1Id, @doctor1Id, @procedure1Id, @room1Id, '2024-06-10 09:00:00'),
                    (@patient2Id, @doctor2Id, @procedure2Id, @room2Id, '2024-06-10 10:30:00'),
                    (@patient3Id, @doctor1Id, @procedure1Id, @room1Id, '2024-06-10 11:00:00');
            ");

            // Medical Records
            migrationBuilder.Sql(@"
                -- Get IDs for relationships
                DECLARE @patient1Id int = (SELECT Id FROM Users WHERE Email = 'george.dumitrescu@email.com');
                DECLARE @patient2Id int = (SELECT Id FROM Users WHERE Email = 'elena.stan@email.com');
                DECLARE @doctor1Id int = (SELECT Id FROM Users WHERE Email = 'ion.popescu@hms.com');
                DECLARE @doctor2Id int = (SELECT Id FROM Users WHERE Email = 'maria.ionescu@hms.com');
                DECLARE @procedure1Id int = (SELECT Id FROM Procedures WHERE Name = 'Electrocardiogram');
                DECLARE @procedure2Id int = (SELECT Id FROM Procedures WHERE Name = 'Neurological Examination');

                -- Insert medical records
                INSERT INTO MedicalRecords (PatientId, DoctorId, ProcedureId, Diagnosis, CreatedAt)
                VALUES
                    (@patient1Id, @doctor1Id, @procedure1Id, 'Mild arrhythmia detected. Recommended follow-up in 3 months.', '2024-06-10 09:30:00'),
                    (@patient2Id, @doctor2Id, @procedure2Id, 'No significant neurological findings. Prescribed rest and stress reduction.', '2024-06-10 11:00:00');
            ");

            // Reviews
            migrationBuilder.Sql(@"
                -- Get IDs for relationships
                DECLARE @patient1Id int = (SELECT Id FROM Users WHERE Email = 'george.dumitrescu@email.com');
                DECLARE @patient2Id int = (SELECT Id FROM Users WHERE Email = 'elena.stan@email.com');
                DECLARE @doctor1Id int = (SELECT Id FROM Users WHERE Email = 'ion.popescu@hms.com');
                DECLARE @doctor2Id int = (SELECT Id FROM Users WHERE Email = 'maria.ionescu@hms.com');

                -- Insert reviews
                INSERT INTO Reviews (PatientId, DoctorId, Value)
                VALUES
                    (@patient1Id, @doctor1Id, 5),
                    (@patient2Id, @doctor2Id, 4);
            ");

            // Notifications
            migrationBuilder.Sql(@"
                -- Get IDs for relationships
                DECLARE @patient1Id int = (SELECT Id FROM Users WHERE Email = 'george.dumitrescu@email.com');
                DECLARE @patient2Id int = (SELECT Id FROM Users WHERE Email = 'elena.stan@email.com');
                DECLARE @doctor1Id int = (SELECT Id FROM Users WHERE Email = 'ion.popescu@hms.com');

                -- Insert notifications
                INSERT INTO Notifications (UserId, Message, DeliveryDateTime)
                VALUES
                    (@patient1Id, 'Reminder: Your cardiology appointment is tomorrow at 9:00 AM', '2024-06-09 09:00:00'),
                    (@patient2Id, 'Reminder: Your neurology appointment is tomorrow at 10:30 AM', '2024-06-09 10:30:00'),
                    (@doctor1Id, 'New appointment scheduled for tomorrow at 9:00 AM', '2024-06-09 12:00:00');
            ");

            // Logs
            migrationBuilder.Sql(@"
                -- Get IDs for relationships
                DECLARE @patient1Id int = (SELECT Id FROM Users WHERE Email = 'george.dumitrescu@email.com');
                DECLARE @doctor1Id int = (SELECT Id FROM Users WHERE Email = 'ion.popescu@hms.com');
                DECLARE @adminId int = (SELECT Id FROM Users WHERE Email = 'admin@hms.com');

                -- Insert logs
                INSERT INTO Logs (UserId, Action, CreatedAt)
                VALUES
                    (@patient1Id, 'Appointment scheduled', '2024-06-09 08:30:00'),
                    (@doctor1Id, 'Medical record created', '2024-06-10 09:30:00'),
                    (@adminId, 'System configuration updated', '2024-06-09 10:00:00');
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Logs");
            migrationBuilder.Sql("DELETE FROM Notifications");
            migrationBuilder.Sql("DELETE FROM Reviews");
            migrationBuilder.Sql("DELETE FROM MedicalRecords");
            migrationBuilder.Sql("DELETE FROM Appointments");
            migrationBuilder.Sql("DELETE FROM Schedules");
            migrationBuilder.Sql("DELETE FROM Shifts");
            migrationBuilder.Sql("DELETE FROM Procedures");
            migrationBuilder.Sql("DELETE FROM Rooms");
            migrationBuilder.Sql("DELETE FROM Equipments");
            migrationBuilder.Sql("DELETE FROM Admins");
            migrationBuilder.Sql("DELETE FROM Doctors");
            migrationBuilder.Sql("DELETE FROM Patients");
            migrationBuilder.Sql("DELETE FROM Users");
            migrationBuilder.Sql("DELETE FROM Departments");
        }
    }
}
