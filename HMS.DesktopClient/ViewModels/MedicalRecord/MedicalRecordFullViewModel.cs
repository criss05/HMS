using HMS.Shared.DTOs;
using HMS.Shared.Repositories.Interfaces;
using HMS.Shared.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HMS.DesktopClient.ViewModels.MedicalRecord
{
    public class MedicalRecordFullViewModel : INotifyPropertyChanged
    {
        private readonly MedicalRecordService _medicalRecordService;

        public ObservableCollection<MedicalRecordSummaryDto> MedicalRecords { get; } = new();

        public MedicalRecordFullViewModel(MedicalRecordService medicalRecordService)
        {
            _medicalRecordService = medicalRecordService;
        }

        public async Task LoadDataAsync()
        {
            var records = await _medicalRecordService.GetMedicalRecordsWithDetailsAsync();
            MedicalRecords.Clear();
            foreach(var record in records)
            {
                MedicalRecords.Add(record);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
