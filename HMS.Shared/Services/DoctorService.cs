using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HMS.Shared.DTOs;
using HMS.Shared.Repositories.Interfaces;

namespace HMS.Shared.Services
{
    public class DoctorService
    {
        private readonly IDoctorRepository _doctorRepository;

        public DoctorService(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public async Task<DoctorDto> AddDoctorAsync(DoctorDto doctor)
        {
            if (doctor == null)
                throw new ArgumentNullException(nameof(doctor), "Doctor cannot be null");
            return await _doctorRepository.AddAsync(doctor);
        }

        public async Task<DoctorDto> GetDoctorByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid doctor ID", nameof(id));
            return await _doctorRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<DoctorDto>> GetAllDoctorsAsync()
        {
            return await _doctorRepository.GetAllAsync();
        }

        public async Task<bool> UpdateDoctorAsync(DoctorDto doctor)
        {
            if (doctor == null)
                throw new ArgumentNullException(nameof(doctor), "Doctor cannot be null");
            return await _doctorRepository.UpdateAsync(doctor);
        }

        public async Task<bool> DeleteDoctorAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid doctor ID", nameof(id));
            return await _doctorRepository.DeleteAsync(id);
        }
     }
}
