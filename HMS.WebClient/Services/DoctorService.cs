using HMS.Shared.DTOs;
using HMS.Shared.Repositories.Interfaces;
using HMS.WebClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.WebClient.Services
{
    public class DoctorService
    {
        private readonly IDoctorRepository _doctorRepository;

        public DoctorService(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public async Task<DoctorViewModel?> GetDoctorByIdAsync(int id)
        {
            var doctorDto = await _doctorRepository.GetByIdAsync(id);
            if (doctorDto == null) return null;
            
            return MapToDoctorViewModel(doctorDto);
        }

        public async Task<IEnumerable<DoctorViewModel>> GetAllDoctorsAsync()
        {
            var doctorDtos = await _doctorRepository.GetAllAsync();
            var viewModels = new List<DoctorViewModel>();
            
            foreach (var dto in doctorDtos)
            {
                viewModels.Add(MapToDoctorViewModel(dto));
            }
            
            return viewModels;
        }

        public async Task<DoctorViewModel> CreateDoctorAsync(DoctorViewModel viewModel)
        {
            var dto = MapToDoctorDto(viewModel);
            var createdDto = await _doctorRepository.AddAsync(dto);
            return MapToDoctorViewModel(createdDto);
        }

        public async Task<bool> UpdateDoctorAsync(DoctorViewModel viewModel)
        {
            var dto = MapToDoctorDto(viewModel);
            return await _doctorRepository.UpdateAsync(dto);
        }

        public async Task<bool> DeleteDoctorAsync(int id)
        {
            return await _doctorRepository.DeleteAsync(id);
        }

        private static DoctorViewModel MapToDoctorViewModel(DoctorDto dto)
        {
            return new DoctorViewModel
            {
                Id = dto.Id,
                Email = dto.Email,
                Role = dto.Role,
                Name = dto.Name,
                CNP = dto.CNP,
                PhoneNumber = dto.PhoneNumber,
                CreatedAt = dto.CreatedAt,
                DepartmentId = dto.DepartmentId,
                DepartmentName = dto.DepartmentName,
                YearsOfExperience = dto.YearsOfExperience,
                LicenseNumber = dto.LicenseNumber,
                ScheduleIds = dto.ScheduleIds,
                ReviewIds = dto.ReviewIds,
                AppointmentIds = dto.AppointmentIds
            };
        }

        private static DoctorDto MapToDoctorDto(DoctorViewModel viewModel)
        {
            return new DoctorDto
            {
                Id = viewModel.Id,
                Email = viewModel.Email,
                Password = viewModel.Password ?? string.Empty,
                Role = viewModel.Role,
                Name = viewModel.Name,
                CNP = viewModel.CNP,
                PhoneNumber = viewModel.PhoneNumber,
                CreatedAt = viewModel.CreatedAt,
                DepartmentId = viewModel.DepartmentId,
                DepartmentName = viewModel.DepartmentName,
                YearsOfExperience = viewModel.YearsOfExperience,
                LicenseNumber = viewModel.LicenseNumber,
                ScheduleIds = viewModel.ScheduleIds,
                ReviewIds = viewModel.ReviewIds,
                AppointmentIds = viewModel.AppointmentIds
            };
        }
    }
} 