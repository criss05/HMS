using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HMS.Shared.Services;
using System.Windows.Input;

namespace HMS.DesktopClient.ViewModels
{
    public class AppointmentsDayViewModel : INotifyPropertyChanged
    {
        private readonly AppointmentService _appointmentService;

        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                if (_selectedDate != value)
                {
                    _selectedDate = value;
                    OnPropertyChanged(nameof(SelectedDate));
                    LoadAppointmentsAsync();
                }
            }
        }

        public ObservableCollection<AppointmentDto> Appointments { get; } = new();

        public AppointmentsDayViewModel()
        {
            _appointmentService = new AppointmentService(new Shared.Proxies.Implementations.AppointmentProxy(App.CurrentUser.Token));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private async void LoadAppointmentsAsync()
        {
            Appointments.Clear();
            var doctorId = App.CurrentDoctor.Id;
            var appointments = await _appointmentService.GetAllAsync();
            if (doctorId > 0)
            {
                appointments = appointments.Where(a => a.DoctorId == doctorId && a.DateTime.Date == SelectedDate.Date).ToList();
            }
            else
            {
                appointments = appointments.Where(a => a.DateTime.Date == SelectedDate.Date).ToList();
            }
            if (appointments == null || !appointments.Any())
            {
                OnPropertyChanged(nameof(Appointments));
                return;
            }
            foreach (var appointment in appointments)
                Appointments.Add(appointment);
            OnPropertyChanged(nameof(Appointments));
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
