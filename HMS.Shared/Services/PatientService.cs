using HMS.Shared.DTOs.Patient;
using HMS.Shared.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Shared.Services
{
    public class PatientService
    {
        private readonly IPatientRepository _patientRepository;

        public PatientService(IPatientRepository patientRepository)
        {
            this._patientRepository = patientRepository;
        }

        public async Task<bool> UpdatePatientAsync(PatientDto patient)
        {
            var updateDto = MapToUpdateDto(patient);

            return await this._patientRepository.UpdateAsync(updateDto, patient.Id);
        }

        public async Task<PatientDto> GetPatientByIdAsync(int id)
        {
            return await this._patientRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<PatientDto>> GetAllPatientsAsync()
        {
            return await this._patientRepository.GetAllAsync();
        }

        public async Task<bool> DeletePatientAsync(int id)
        {
            return await this._patientRepository.DeleteAsync(id);
        }

        public async Task<PatientDto> AddPatientAsync(PatientDto patient)
        {
            return await this._patientRepository.AddAsync(patient);
        }

        private PatientUpdateDto MapToUpdateDto(PatientDto patient)
        {
            return new PatientUpdateDto
            {
                Id = patient.Id,
                Email = patient.Email,
                Password = patient.Password,
                Name = patient.Name,
                CNP = patient.CNP,
                PhoneNumber = patient.PhoneNumber,
                Role = patient.Role.ToString(),
                BloodType = patient.BloodType ?? "",
                EmergencyContact = patient.EmergencyContact ?? "",
                Allergies = patient.Allergies ?? "",
                Weight = patient.Weight,
                Height = patient.Height,
                BirthDate = patient.BirthDate,
                Address = patient.Address ?? ""
            };
        }
    }
}
