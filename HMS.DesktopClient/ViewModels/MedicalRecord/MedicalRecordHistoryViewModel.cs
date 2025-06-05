using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using HMS.Shared.DTOs;
using HMS.Shared.Proxies.Implementations;
using System.Linq;

namespace HMS.DesktopClient.ViewModels
{
    public class MedicalRecordHistoryViewModel
    {
        private readonly MedicalRecordProxy _medicalRecordProxy;

        public ObservableCollection<MedicalRecordDto> MedicalRecords { get; } = new();

        public MedicalRecordHistoryViewModel(int doctorId, MedicalRecordProxy medicalRecordProxy)
        {
            _medicalRecordProxy = medicalRecordProxy;
            InitializeAsync(doctorId);
        }

        private async Task InitializeAsync(int doctorId)
        {
            try
            {
                var records = await _medicalRecordProxy.GetAllAsync();
                foreach (var record in records.Where(r => r.DoctorId == doctorId))
                {
                    MedicalRecords.Add(record);
                }
            }
            catch (Exception ex)
            {
                // Handle error appropriately (e.g., logging, UI notification)
                Console.WriteLine($"Error loading medical records: {ex.Message}");
            }
        }
    }
}