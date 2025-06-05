using HMS.Shared.DTOs;
using System.ComponentModel;

namespace HMS.DesktopClient.ViewModels
{
    public class PatientProfileViewModel : INotifyPropertyChanged
    {
        private UserWithTokenDto _user;

        public event PropertyChangedEventHandler? PropertyChanged;

        public PatientProfileViewModel(UserWithTokenDto user)
        {
            _user = user;
        }

        // Editable fields
        public string Name
        {
            get => _user.Name;
            set
            {
                if (_user.Name != value)
                {
                    _user.Name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public string PhoneNumber
        {
            get => _user.PhoneNumber;
            set
            {
                if (_user.PhoneNumber != value)
                {
                    _user.PhoneNumber = value;
                    OnPropertyChanged(nameof(PhoneNumber));
                }
            }
        }

        public string CNP
        {
            get => _user.CNP;
            set
            {
                if (_user.CNP != value)
                {
                    _user.CNP = value;
                    OnPropertyChanged(nameof(CNP));
                }
            }
        }

        // Read-only properties
        public string Email => _user.Email;
        public string Role => _user.Role.ToString();
        public string CreatedAt => _user.CreatedAt.ToString("yyyy-MM-dd HH:mm");
        public string Token => _user.Token;

        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
