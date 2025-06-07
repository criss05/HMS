using HMS.Shared.DTOs.Doctor;
using HMS.Shared.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HMS.DesktopClient.ViewModels.Doctor
{
    public class DoctorSummaryViewModel : INotifyPropertyChanged
    {
        private DoctorService _doctorService;
        public ObservableCollection<DoctorListItemDto> Doctors { get; } = new();


        public DoctorSummaryViewModel(DoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        public async Task LoadDoctorsSummary()
        {
            try
            {
                Doctors.Clear();
                var doctors = await _doctorService.GetDoctorsSummaryAsync();
                foreach (var doctor in doctors)
                {
                    Doctors.Add(doctor);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to load doctors summary: " + ex.Message);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
