using HMS.Shared.DTOs;
using HMS.Shared.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.WebClient.Services
{
    public class AppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IDoctorRepository _doctorRepository;         
        private readonly IProcedureRepository _procedureRepository;   

        public AppointmentService(
            IAppointmentRepository appointmentRepository,
            IDoctorRepository doctorRepository,                       
            IProcedureRepository procedureRepository)                 
        {
            _appointmentRepository = appointmentRepository;
            _doctorRepository = doctorRepository;                    
            _procedureRepository = procedureRepository;              
        }

        public async Task<IEnumerable<AppointmentDto>> GetAppointmentsForPatientAsync(int patientId)
        {
            var appointments = await _appointmentRepository.GetAllAsync();
            return appointments.Where(a => a.PatientId == patientId).ToList();
        }

        public async Task<IEnumerable<AppointmentDto>> GetUpcomingAppointmentsForPatientAsync(int patientId, int count)
        {
            var appointments = await _appointmentRepository.GetAllAsync();
            return appointments
                .Where(a => a.PatientId == patientId && a.DateTime > DateTime.Now)
                .OrderBy(a => a.DateTime)
                .Take(count)
                .ToList();
        }

        public async Task<AppointmentDto> GetAppointmentByIdAsync(int id)
        {
            return await _appointmentRepository.GetByIdAsync(id);
        }

        public async Task<bool> CreateAppointmentAsync(AppointmentDto appointment)
        {
            if (appointment == null)
                throw new ArgumentNullException(nameof(appointment));

            var result = await _appointmentRepository.AddAsync(appointment);
            return result != null;
        }

        public async Task<bool> CancelAppointmentAsync(int id)
        {
            return await _appointmentRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<ProcedureDto>> GetProceduresForDoctorAsync(int doctorId)
        {
            try
            {
                var doctor = await _doctorRepository.GetByIdAsync(doctorId);
                if (doctor == null || doctor.DepartmentId <= 0)
                {
                    Console.WriteLine($"Doctor not found or invalid department: {doctorId}");
                    return new List<ProcedureDto>();
                }

                Console.WriteLine($"Looking for procedures for department {doctor.DepartmentId}");
                var procedures = await _procedureRepository.GetAllAsync();

                if (procedures == null)
                {
                    Console.WriteLine("No procedures returned from repository");
                    return new List<ProcedureDto>();
                }

                var filteredProcedures = procedures
                    .Where(p => p.DepartmentId == doctor.DepartmentId)
                    .ToList();

                Console.WriteLine($"Found {filteredProcedures.Count} procedures for department {doctor.DepartmentId}");
                return filteredProcedures;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting procedures: {ex.Message}");
                return new List<ProcedureDto>();
            }
        }

        private IEnumerable<ProcedureDto> GetFallbackProcedures(int doctorId)
        {
            // Return hardcoded procedures based on doctor ID to ensure something shows up
            if (doctorId % 8 == 1) // Dr. Ion Popescu (Cardiology)
            {
                return new List<ProcedureDto>
        {
            new ProcedureDto { Id = 1, Name = "Electrocardiogram", DepartmentId = 1 },
            new ProcedureDto { Id = 2, Name = "Echocardiogram", DepartmentId = 1 },
            new ProcedureDto { Id = 3, Name = "Stress Test", DepartmentId = 1 }
        };
            }
            else if (doctorId % 8 == 2) // Dr. Maria Ionescu (Neurology)
            {
                return new List<ProcedureDto>
        {
            new ProcedureDto { Id = 4, Name = "Neurological Examination", DepartmentId = 2 },
            new ProcedureDto { Id = 5, Name = "EEG", DepartmentId = 2 }
        };
            }
            // Add more fallbacks for other doctors/departments

            // Default fallback
            return new List<ProcedureDto>
    {
        new ProcedureDto { Id = 1, Name = "Consultation", DepartmentId = 1 },
        new ProcedureDto { Id = 2, Name = "General Examination", DepartmentId = 1 }
    };
        }
    }
}