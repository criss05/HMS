using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Hospital.Managers;
using Hospital.Models;
using Hospital.Views;

namespace Hospital.ViewModels
{
    public class MedicalRecordHistoryViewModel
    {
        private readonly IMedicalRecordManager _medicalRecordManager;
        private readonly IDocumentManager _documentManager;

        public ObservableCollection<MedicalRecordJointModel> MedicalRecords { get; private set; }

        public MedicalRecordHistoryViewModel(int patientId, IMedicalRecordManager medicalRecordManager, IDocumentManager documentManager)
        {
            _medicalRecordManager = medicalRecordManager;
            _documentManager = documentManager;
            MedicalRecords = new ObservableCollection<MedicalRecordJointModel>();
            InitializeAsync(patientId);
        }

        private async void InitializeAsync(int patientId)
        {
            try
            {
                await _medicalRecordManager.LoadMedicalRecordsForPatient(patientId);
                var records = await _medicalRecordManager.GetMedicalRecords();
                foreach (var record in records)
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

        public void ShowMedicalRecordDetails(MedicalRecordJointModel medicalRecord)
        {
            if (medicalRecord == null)
                throw new ArgumentNullException(nameof(medicalRecord));

            var detailsView = new MedicalRecordDetailsView(medicalRecord, _documentManager);
            detailsView.Activate();
        }
    }
}
