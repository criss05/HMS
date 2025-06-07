using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.ComponentModel;
using HMS.Shared.DTOs;
using HMS.Shared.Proxies.Implementations;
using System.Windows.Input;

namespace HMS.DesktopClient.ViewModels
{
    public class ShiftViewModel : INotifyPropertyChanged
    {
        private readonly ShiftProxy _shiftProxy;
        private readonly DoctorProxy _doctorProxy;

        public int UserRole { get; set; } // 0: Admin, 1: Doctor, 2: Patient

        public ObservableCollection<ShiftDto> Shifts { get; set; } = new ObservableCollection<ShiftDto>();

        public ObservableCollection<DoctorDto> AvailableDoctors { get; set; } = new ObservableCollection<DoctorDto>();

        private ObservableCollection<DoctorDto> _selectedDoctors = new ObservableCollection<DoctorDto>();
        public ObservableCollection<DoctorDto> SelectedDoctors
        {
            get => _selectedDoctors;
            set
            {
                _selectedDoctors = value;
                OnPropertyChanged(nameof(SelectedDoctors));
                AddShiftCommand.RaiseCanExecuteChanged();
                UpdateShiftCommand.RaiseCanExecuteChanged();
            }
        }

        private DateTimeOffset? _inputShiftDate;
        public DateTimeOffset? InputShiftDate
        {
            get => _inputShiftDate;
            set
            {
                if (_inputShiftDate != value)
                {
                    _inputShiftDate = value;
                    OnPropertyChanged(nameof(InputShiftDate));
                    AddShiftCommand.RaiseCanExecuteChanged();
                    UpdateShiftCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private string _inputShiftStartTimeString = string.Empty;
        public string InputShiftStartTimeString
        {
            get => _inputShiftStartTimeString;
            set
            {
                if (_inputShiftStartTimeString != value)
                {
                    _inputShiftStartTimeString = value;
                    OnPropertyChanged(nameof(InputShiftStartTimeString));
                    AddShiftCommand.RaiseCanExecuteChanged();
                    UpdateShiftCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private string _inputShiftEndTimeString = string.Empty;
        public string InputShiftEndTimeString
        {
            get => _inputShiftEndTimeString;
            set
            {
                if (_inputShiftEndTimeString != value)
                {
                    _inputShiftEndTimeString = value;
                    OnPropertyChanged(nameof(InputShiftEndTimeString));
                    AddShiftCommand.RaiseCanExecuteChanged();
                    UpdateShiftCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private ShiftDto? _selectedShift;
        public ShiftDto? SelectedShift
        {
            get => _selectedShift;
            set
            {
                if (_selectedShift != value)
                {
                    _selectedShift = value;
                    OnPropertyChanged(nameof(SelectedShift));
                    if (_selectedShift != null)
                    {
                        InputShiftDate = new DateTimeOffset(_selectedShift.Date.ToDateTime(TimeOnly.MinValue));
                        InputShiftStartTimeString = _selectedShift.StartTime.ToString("HH:mm");
                        InputShiftEndTimeString = _selectedShift.EndTime.ToString("HH:mm");
                        SelectedDoctors = new ObservableCollection<DoctorDto>(
                            AvailableDoctors.Where(d => _selectedShift.DoctorIds.Contains(d.Id))
                        );
                    }
                    else
                    {
                        ClearInputFields();
                    }
                    UpdateShiftCommand.RaiseCanExecuteChanged();
                    DeleteShiftCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string FormattedDoctorIds => SelectedShift?.FormattedDoctorIds ?? "None";

        public RelayCommand AddShiftCommand { get; }
        public RelayCommand UpdateShiftCommand { get; }
        public RelayCommand DeleteShiftCommand { get; }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ShiftViewModel(HttpClient httpClient, string token)
        {
            _shiftProxy = new ShiftProxy(httpClient, token);
            _doctorProxy = new DoctorProxy(httpClient, token);

            AddShiftCommand = new RelayCommand(async _ => await AddShiftAsync(), _ => CanAddShift());
            UpdateShiftCommand = new RelayCommand(async _ => await UpdateShiftAsync(), _ => CanUpdateDeleteShift());
            DeleteShiftCommand = new RelayCommand(async _ => await DeleteShiftAsync(), _ => CanUpdateDeleteShift());

            _ = LoadDoctorsAsync();
        }

        private bool CanAddShift()
        {
            return UserRole == 0 &&
                   InputShiftDate.HasValue &&
                   TimeOnly.TryParseExact(InputShiftStartTimeString, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out _) &&
                   TimeOnly.TryParseExact(InputShiftEndTimeString, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        }

        private bool CanUpdateDeleteShift()
        {
            return UserRole == 0 && SelectedShift != null;
        }

        private void ClearInputFields()
        {
            InputShiftDate = null;
            InputShiftStartTimeString = string.Empty;
            InputShiftEndTimeString = string.Empty;
            SelectedDoctors.Clear();
        }

        public async Task LoadShiftsAsync()
        {
            if (UserRole == 0)
            {
                try
                {
                    var allShifts = await _shiftProxy.GetAllAsync();
                    Shifts.Clear();
                    foreach (var shift in allShifts)
                    {
                        Shifts.Add(shift);
                    }
                }
                catch (HttpRequestException httpEx)
                {
                    Console.WriteLine($"HTTP request error loading shifts: {httpEx.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred loading shifts: {ex.Message}");
                }
            }
        }

        public async Task LoadDoctorsAsync()
        {
            try
            {
                var doctors = await _doctorProxy.GetAllAsync();
                AvailableDoctors.Clear();
                foreach (var doc in doctors)
                {
                    AvailableDoctors.Add(doc);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading doctors: {ex.Message}");
            }
        }

        public async Task AddShiftAsync()
        {
            if (!CanAddShift()) return;

            DateOnly date = DateOnly.FromDateTime(InputShiftDate!.Value.Date);
            TimeOnly startTime = TimeOnly.ParseExact(InputShiftStartTimeString, "HH:mm", CultureInfo.InvariantCulture);
            TimeOnly endTime = TimeOnly.ParseExact(InputShiftEndTimeString, "HH:mm", CultureInfo.InvariantCulture);

            var doctorIds = SelectedDoctors.Select(d => d.Id).ToList();

            var newShift = new ShiftDto
            {
                Date = date,
                StartTime = startTime,
                EndTime = endTime,
                DoctorIds = doctorIds
            };

            var addedShift = await _shiftProxy.AddAsync(newShift);
            Shifts.Add(addedShift);
            ClearInputFields();
        }

        public async Task UpdateShiftAsync()
        {
            if (!CanUpdateDeleteShift() || SelectedShift == null) return;

            SelectedShift.Date = DateOnly.FromDateTime(InputShiftDate!.Value.Date);
            SelectedShift.StartTime = TimeOnly.ParseExact(InputShiftStartTimeString, "HH:mm", CultureInfo.InvariantCulture);
            SelectedShift.EndTime = TimeOnly.ParseExact(InputShiftEndTimeString, "HH:mm", CultureInfo.InvariantCulture);
            SelectedShift.DoctorIds = SelectedDoctors.Select(d => d.Id).ToList();

            bool success = await _shiftProxy.UpdateAsync(SelectedShift);

            if (success)
            {
                ClearInputFields();
            }
            else
            {
                Console.WriteLine($"Update failed for shift with ID {SelectedShift.Id} on backend.");
            }
        }

        public async Task DeleteShiftAsync()
        {
            if (!CanUpdateDeleteShift() || SelectedShift == null) return;

            int shiftIdToDelete = SelectedShift.Id;
            bool success = await _shiftProxy.DeleteAsync(shiftIdToDelete);

            if (success)
            {
                Shifts.Remove(SelectedShift);
                ClearInputFields();
            }
            else
            {
                Console.WriteLine($"Deletion failed for shift {shiftIdToDelete} on backend.");
            }
        }
    }
}
