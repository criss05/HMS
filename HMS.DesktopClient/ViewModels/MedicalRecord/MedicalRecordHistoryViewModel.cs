using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using HMS.Shared.DTOs;
using HMS.Shared.Proxies.Implementations;
using System.Linq;
using HMS.Shared.Entities;

namespace HMS.DesktopClient.ViewModels
{
    /// <summary>
    /// View model that provides a collection of medical records for a specific patient.
    /// </summary>
    /// <remarks>
    /// This view model is responsible for loading and maintaining a patient's medical history,
    /// facilitating the display of their complete medical record timeline.
    /// </remarks>
    public class MedicalRecordHistoryViewModel
    {
        /// <summary>
        /// The proxy used to communicate with the medical record service.
        /// </summary>
        /// <remarks>
        /// Handles all API calls related to retrieving medical record data.
        /// </remarks>
        private readonly MedicalRecordProxy _medicalRecordProxy;
        private readonly DoctorProxy _doctorProxy;
        private readonly PatientProxy _patientProxy;
        private readonly ProcedureProxy _procedureProxy;

        /// <summary>
        /// Gets the collection of medical records for the specified patient.
        /// </summary>
        /// <remarks>
        /// This observable collection is bound to UI elements to display the patient's medical history.
        /// It automatically notifies the UI when records are added, removed, or modified.
        /// </remarks>
        public ObservableCollection<EnrichedMedicalRecordDto> MedicalRecords { get; } = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="MedicalRecordHistoryViewModel"/> class.
        /// </summary>
        /// <param name="patientId">The unique identifier of the patient whose records to retrieve.</param>
        /// <param name="medicalRecordProxy">The proxy used to fetch medical record data.</param>
        /// <remarks>
        /// Upon initialization, this constructor triggers the asynchronous loading of medical records
        /// for the specified patient.
        /// </remarks>
        public MedicalRecordHistoryViewModel(MedicalRecordProxy medicalRecordProxy, DoctorProxy doctorProxy, PatientProxy patientProxy, ProcedureProxy procedureProxy)
        {
            _medicalRecordProxy = medicalRecordProxy;
            _doctorProxy = doctorProxy;
            _patientProxy = patientProxy;
            _procedureProxy = procedureProxy;
        }


        /// <summary>
        /// Loads records for a specific patient.
        /// </summary>
        public async Task LoadRecordsForPatientAsync(int patientId)
        {
            try
            {
                MedicalRecords.Clear();
                var records = await _medicalRecordProxy.GetAllAsync();
                var filteredRecords = records.Where(r => r.PatientId == patientId);
                foreach (var record in filteredRecords)
                {
                    var doctor = await _doctorProxy.GetByIdAsync(record.DoctorId);
                    var procedure = await _procedureProxy.GetByIdAsync(record.ProcedureId);
                    MedicalRecords.Add(new EnrichedMedicalRecordDto
                    {
                        Id = record.Id,
                        Name = doctor?.Name ?? "Unknown Doctor",
                        ProcedureName = procedure?.Name ?? "Unknown Procedure",
                        Diagnosis = record.Diagnosis,
                        CreatedAt = record.CreatedAt ?? DateTime.Now
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading medical records: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads records for a specific doctor (i.e., all records where they treated patients).
        /// </summary>
        public async Task LoadRecordsForDoctorAsync(int doctorId)
        {
            try
            {
                MedicalRecords.Clear();
                var records = await _medicalRecordProxy.GetAllAsync();
                var filteredRecords = records.Where(r => r.DoctorId == doctorId);
                foreach (var record in filteredRecords)
                {
                    var patient = await _patientProxy.GetByIdAsync(record.PatientId);
                    var procedure = await _procedureProxy.GetByIdAsync(record.ProcedureId);

                    MedicalRecords.Add(new EnrichedMedicalRecordDto
                    {
                        Id = record.Id,
                        Name = patient?.Name ?? "Unknown Patient",
                        ProcedureName = procedure?.Name ?? "Unknown Procedure",
                        Diagnosis = record.Diagnosis,
                        CreatedAt = record.CreatedAt ?? DateTime.Now
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading medical records: {ex.Message}");
            }
        }
    }
}