using HMS.Shared.DTOs;
using HMS.Shared.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.WebClient.Services
{
    public class MedicalRecordService
    {
        private readonly IMedicalRecordRepository _medicalRecordRepository;

        public MedicalRecordService(IMedicalRecordRepository medicalRecordRepository)
        {
            _medicalRecordRepository = medicalRecordRepository;
        }

        public async Task<IEnumerable<MedicalRecordDto>> GetMedicalRecordsForPatientAsync(int patientId)
        {
            var records = await _medicalRecordRepository.GetAllAsync();
            return records.Where(r => r.PatientId == patientId).ToList();
        }

        public async Task<IEnumerable<MedicalRecordDto>> GetRecentMedicalRecordsForPatientAsync(int patientId, int count)
        {
            var records = await _medicalRecordRepository.GetAllAsync();
            return records
                .Where(r => r.PatientId == patientId)
                .OrderByDescending(r => r.CreatedAt)
                .Take(count)
                .ToList();
        }

        public async Task<MedicalRecordDto> GetMedicalRecordByIdAsync(int id)
        {
            return await _medicalRecordRepository.GetByIdAsync(id);
        }

        public async Task<byte[]> GenerateRecordPdfAsync(MedicalRecordDto record)
        {
            // This is a placeholder for PDF generation logic
            // In a real application, you would use a PDF generation library like iTextSharp or similar

            // For now, just return an empty byte array
            return new byte[0];
        }
    }
}