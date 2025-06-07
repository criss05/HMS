using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using HMS.Shared.Services;
using System.Windows.Input;
using System.Collections.ObjectModel;
using HMS.Shared.Enums;
using System.Globalization;
using HMS.Shared.DTOs.Patient;
using HMS.Shared.DTOs;
using HMS.Shared.Proxies.Implementations;

namespace HMS.DesktopClient.ViewModels
{
    public class AppointmentsCalendarViewModel : INotifyPropertyChanged
    {
        private readonly AppointmentService _appointmentService;
        private readonly PatientProxy patientProxy = new PatientProxy(App.CurrentUser.Token);
        private readonly ProcedureProxy procedureProxy = new ProcedureProxy(App.CurrentUser.Token);
        private readonly RoomProxy roomProxy = new RoomProxy(App.CurrentUser.Token);

        public Dictionary<DateTime, int> AppointmentCounts { get; } = new();
        public ObservableCollection<AppointmentDto> Appointments { get; } = new ObservableCollection<AppointmentDto>();

        private ObservableCollection<PatientDto> _selectedPatients = new ObservableCollection<PatientDto>();

        private ObservableCollection<ProcedureDto> _selectedProcs = new ObservableCollection<ProcedureDto>();

        private ObservableCollection<RoomDto> _selectedRooms = new ObservableCollection<RoomDto>();

        public ObservableCollection<PatientDto> AvailablePatients { get; set; } = new ObservableCollection<PatientDto>();
        public ObservableCollection<ProcedureDto> AvailableProcedures { get; set; } = new ObservableCollection<ProcedureDto>();
        public ObservableCollection<RoomDto> AvailableRooms { get; set; } = new ObservableCollection<RoomDto>();


        public RelayCommand AddAppointmentCommand { get; }
        public RelayCommand UpdateAppointentCommand { get; }
        public RelayCommand DeleteAppointmentCommand { get; }

        private DateTimeOffset? _selectedDate;
        public DateTimeOffset? SelectedDate
        {
            get => _selectedDate;
            set
            {
                if (_selectedDate != value)
                {
                    _selectedDate = value;
                    OnPropertyChanged(nameof(SelectedDate));
                    AddAppointmentCommand.RaiseCanExecuteChanged();
                    UpdateAppointentCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private TimeSpan? _inputAppointmentTime;
        public TimeSpan? InputAppointmentTime
        {
            get => _inputAppointmentTime;
            set
            {
                if (_inputAppointmentTime != value)
                {
                    _inputAppointmentTime = value;
                    OnPropertyChanged(nameof(InputAppointmentTime));
                    AddAppointmentCommand.RaiseCanExecuteChanged();
                    UpdateAppointentCommand.RaiseCanExecuteChanged();
                }
            }
        }


        public ObservableCollection<PatientDto> SelectedPatients
        {
            get => _selectedPatients;
            set
            {
                _selectedPatients = value;
                OnPropertyChanged(nameof(SelectedPatients));
                AddAppointmentCommand.RaiseCanExecuteChanged();
                UpdateAppointentCommand.RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<ProcedureDto> SelectedProcs
        {
            get => _selectedProcs;
            set
            {
                _selectedProcs = value;
                OnPropertyChanged(nameof(SelectedProcs));
                AddAppointmentCommand.RaiseCanExecuteChanged();
                UpdateAppointentCommand.RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<RoomDto> SelectedRooms
        {
            get => _selectedRooms;
            set
            {
                _selectedRooms = value;
                OnPropertyChanged(nameof(SelectedRooms));
                AddAppointmentCommand.RaiseCanExecuteChanged();
                UpdateAppointentCommand.RaiseCanExecuteChanged();
            }
        }

        public AppointmentsCalendarViewModel()
        {
            _appointmentService = new AppointmentService(new Shared.Proxies.Implementations.AppointmentProxy(App.CurrentUser.Token));

            AddAppointmentCommand = new RelayCommand(async _ => await AddAppointmentAsync(), _ => CanAddAppontment());
            UpdateAppointentCommand = new RelayCommand(async _ => await UpdateAppointmentAsync(), _ => CanUpdateDeleteAppointment());
            DeleteAppointmentCommand = new RelayCommand(async _ => await DeleteAppointmentAsync(), _ => CanUpdateDeleteAppointment());

            LoadAppointmentsAsync();
            LoadAvailablePatientsAsync();
            LoadAvailableProceduresAsync();
            LoadAvailableRoomsAsync();

        }

        private AppointmentDto? _selectedAppointment;
        public AppointmentDto? SelectedAppointment
        {
            get => _selectedAppointment;
            set
            {
                if (_selectedAppointment != value)
                {
                    _selectedAppointment = value;
                    OnPropertyChanged(nameof(SelectedAppointment));
                    if (_selectedAppointment != null)
                    {
                        SelectedDate = _selectedAppointment.DateTime;
                        InputAppointmentTime = _selectedAppointment.DateTime.TimeOfDay;
                        SelectedPatients = new ObservableCollection<PatientDto>(
                            AvailablePatients.Where(p => p.Id == _selectedAppointment.PatientId)
                        );
                        SelectedProcs = new ObservableCollection<ProcedureDto>(
                            AvailableProcedures.Where(p => p.Id == _selectedAppointment.ProcedureId)
                        );
                        SelectedRooms = new ObservableCollection<RoomDto>(
                            AvailableRooms.Where(r => r.Id == _selectedAppointment.RoomId)
                        );
                    }
                    else
                    {
                        ClearInputFields();
                    }
                    UpdateAppointentCommand.RaiseCanExecuteChanged();
                    DeleteAppointmentCommand.RaiseCanExecuteChanged();
                }
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        private async void LoadAppointmentsAsync()
        {
            if (App.CurrentDoctor == null && App.CurrentAdmin != null)
            {
                var _appointments = await _appointmentService.GetAllAsync();
                AppointmentCounts.Clear();
                foreach (var group in _appointments.GroupBy(a => a.DateTime.Date))
                    AppointmentCounts[group.Key] = group.Count();
                OnPropertyChanged(nameof(AppointmentCounts));
                return;
            }
            var doctorId = App.CurrentDoctor.Id;
            var appointments = await _appointmentService.GetAllAsync();
            if (doctorId > 0)
            {
                appointments = appointments.Where(a => a.DoctorId == doctorId).ToList();
            }
            if (appointments == null || !appointments.Any())
            {
                AppointmentCounts.Clear();
                OnPropertyChanged(nameof(AppointmentCounts));
                return;
            }
            AppointmentCounts.Clear();
            foreach (var group in appointments.GroupBy(a => a.DateTime.Date))
                AppointmentCounts[group.Key] = group.Count();

            OnPropertyChanged(nameof(AppointmentCounts));
        }

        private async void LoadAvailablePatientsAsync()
        {
            var patients = await patientProxy.GetAllAsync();
            AvailablePatients.Clear();
            foreach (var patient in patients)
            {
                AvailablePatients.Add(patient);
            }
        }

        private async void LoadAvailableProceduresAsync()
        {
            var procedures = await procedureProxy.GetAllAsync();
            AvailableProcedures.Clear();
            foreach (var procedure in procedures)
            {
                AvailableProcedures.Add(procedure);
            }
        }

        private async void LoadAvailableRoomsAsync()
        {
            var rooms = await roomProxy.GetAllAsync();
            AvailableRooms.Clear();
            foreach (var room in rooms)
            {
                AvailableRooms.Add(room);
            }
        }

        public async 
        Task
AddAppointmentAsync()
        {
            if (!CanAddAppontment()) return;

            var fullDateTime = SelectedDate!.Value.Date + InputAppointmentTime!.Value;
            var appointment = new AppointmentDto
            {
                DateTime = fullDateTime,
                PatientId = SelectedPatients.FirstOrDefault()?.Id ?? 0,
                ProcedureId = SelectedProcs.FirstOrDefault()?.Id ?? 0,
                RoomId = SelectedRooms.FirstOrDefault()?.Id ?? 0,
                DoctorId = App.CurrentDoctor.Id
            };
            
            await _appointmentService.AddAsync(appointment);
            ClearInputFields();
            LoadAppointmentsAsync();
        }

        public async 
        Task
UpdateAppointmentAsync()
        {
            if (SelectedAppointment == null || !CanUpdateDeleteAppointment()) return;
            var fullDateTime = SelectedDate!.Value.Date + InputAppointmentTime!.Value;
            var appointment = new AppointmentDto
            {
                Id = SelectedAppointment.Id,
                DateTime = fullDateTime,
                PatientId = SelectedPatients.FirstOrDefault()?.Id ?? 0,
                ProcedureId = SelectedProcs.FirstOrDefault()?.Id ?? 0,
                RoomId = SelectedRooms.FirstOrDefault()?.Id ?? 0,
                DoctorId = App.CurrentDoctor.Id
            };
            await _appointmentService.UpdateAsync(appointment);
            ClearInputFields();
            LoadAppointmentsAsync();
        }

        public async 
        Task
DeleteAppointmentAsync()
        {
            if(SelectedAppointment == null || !CanUpdateDeleteAppointment()) return;
            await _appointmentService.DeleteAsync(SelectedAppointment.Id ?? -1);
            ClearInputFields();
            LoadAppointmentsAsync();
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public async Task<List<AppointmentDto>> GetAppointmentsForDoctorAsync(int doctorId)
        {
            var all = await _appointmentService.GetAllAsync();
            return all.Where(a => a.DoctorId == doctorId).ToList();
        }

        public async Task<List<AppointmentDto>> GetAppointmentsForPatientAsync(int patientId)
        {
            var all = await _appointmentService.GetAllAsync();
            return all.Where(a => a.PatientId == patientId).ToList();
        }

        public async Task<List<AppointmentDto>> GetAllAppointmentsAsync()
        {
            return await _appointmentService.GetAllAsync();
        }


        private bool CanAddAppontment()
        {
            return SelectedDate.HasValue &&
                   InputAppointmentTime.HasValue &&
                   SelectedPatients.Count > 0 &&
                   SelectedProcs.Count > 0 &&
                   SelectedRooms.Count > 0;
        }

        private bool CanUpdateDeleteAppointment()
        {
            return SelectedAppointment != null;
        }

        private void ClearInputFields()
        {
            SelectedDate = null;
            InputAppointmentTime = null;
            SelectedPatients.Clear();
            SelectedProcs.Clear();
            SelectedRooms.Clear();
        }
    }

}
