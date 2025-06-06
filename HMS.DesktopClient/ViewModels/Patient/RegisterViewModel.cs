using HMS.Shared.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.DesktopClient.ViewModels.Patient
{
    public class RegisterViewModel : INotifyPropertyChanged
    {
        private PatientService _patientService;

        public RegisterViewModel(PatientService patientService)
        {
            this._patientService = patientService;
        }

        public async Task<PatientCreateDto> RegisterPatientAsync(PatientCreateDto patientDto)
        {
            if (patientDto == null)
                throw new ArgumentNullException(nameof(patientDto));
            try
            {
                return await _patientService.AddPatientAsync(patientDto);
            }
            catch (Exception ex)
            {
                throw new Exception("Registration failed: " + ex.Message);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
