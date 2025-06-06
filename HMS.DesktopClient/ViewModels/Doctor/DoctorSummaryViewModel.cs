using HMS.Shared.DTOs.Doctor;
using HMS.Shared.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.DesktopClient.ViewModels.Doctor
{
    public class DoctorSummaryViewModel : INotifyPropertyChanged
    {
        private DoctorService _doctorService;

        public DoctorSummaryViewModel(DoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        public async Task<IEnumerable<DoctorListItemDto>> LoadDoctorsSummary()
        {
            try
            {
                return await _doctorService.GetDoctorsSummaryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to load doctors summary: " + ex.Message);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
