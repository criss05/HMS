using HMS.Shared.DTOs;
using HMS.Shared.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Shared.Services
{
    public class MedicalRecordService
    {
        private readonly IMedicalRecordRepository _medicalRecordRepository;

        public MedicalRecordService(IMedicalRecordRepository medicalRecordRepository)
        {
            _medicalRecordRepository = medicalRecordRepository;
        }

        public async Task<IEnumerable<MedicalRecordSummaryDto>> GetMedicalRecordsWithDetailsAsync()
        {
            try
            {
                return await _medicalRecordRepository.GetMedicalRecordsWithDetailsAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve medical records with details: " + ex.Message);
            }
        }
    }
}
