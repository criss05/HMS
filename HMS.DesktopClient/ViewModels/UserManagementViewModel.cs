using HMS.Shared.DTOs;
using HMS.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HMS.DesktopClient.ViewModels
{
    public class UserManagementViewModel : INotifyPropertyChanged
    {
        private readonly UserWithTokenDto _currentUser;
        private readonly HttpClient _httpClient;
        private ObservableCollection<UserDto> _allUsers;
        private ObservableCollection<UserDto> _displayedUsers;
        private string _searchTerm;
        private UserRole? _selectedRole;
        private string _errorMessage;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<UserDto> AllUsers
        {
            get => _allUsers;
            set
            {
                _allUsers = value;
                OnPropertyChanged(nameof(AllUsers));
            }
        }

        public ObservableCollection<UserDto> DisplayedUsers
        {
            get => _displayedUsers;
            set
            {
                _displayedUsers = value;
                OnPropertyChanged(nameof(DisplayedUsers));
            }
        }

        public string SearchTerm
        {
            get => _searchTerm;
            set
            {
                _searchTerm = value;
                OnPropertyChanged(nameof(SearchTerm));
            }
        }

        public UserRole? SelectedRole
        {
            get => _selectedRole;
            set
            {
                _selectedRole = value;
                OnPropertyChanged(nameof(SelectedRole));
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public UserManagementViewModel(UserWithTokenDto currentUser)
        {
            _currentUser = currentUser;
            _httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5203/api/") };
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_currentUser.Token}");

            AllUsers = new ObservableCollection<UserDto>();
            DisplayedUsers = new ObservableCollection<UserDto>();
            SearchTerm = string.Empty;
        }

        public async Task LoadUsersAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("User");
                if (response.IsSuccessStatusCode)
                {
                    string jsonContent = await response.Content.ReadAsStringAsync();

                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        ReferenceHandler = ReferenceHandler.Preserve,
                        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
                    };

                    var users = JsonSerializer.Deserialize<List<UserDto>>(jsonContent, options);
                    if (users != null)
                    {
                        AllUsers = new ObservableCollection<UserDto>(users);
                        FilterUsers();
                    }
                    else
                    {
                        ErrorMessage = "No users returned from API";
                    }
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    ErrorMessage = $"Error loading users. Status: {response.StatusCode}, Message: {errorContent}";
                    throw new Exception(ErrorMessage);
                }
            }
            catch (JsonException jex)
            {
                ErrorMessage = $"Failed to parse user data: {jex.Message}";
                throw;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Failed to load users: {ex.Message}";
                throw;
            }
        }

        public async Task<bool> UpdateUserAsync(UserDto user)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"User/{user.Id}", user);
                if (response.IsSuccessStatusCode)
                {
                    // Update the user in our collections
                    var existingUserIndex = AllUsers.ToList().FindIndex(u => u.Id == user.Id);
                    if (existingUserIndex >= 0)
                    {
                        AllUsers[existingUserIndex] = user;
                    }

                    FilterUsers(); // Refresh the displayed users
                    return true;
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    ErrorMessage = $"Error updating user. Status: {response.StatusCode}, Message: {errorContent}";
                    return false;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Failed to update user: {ex.Message}";
                return false;
            }
        }

        public UserDto GetUserById(int userId)
        {
            return AllUsers.FirstOrDefault(u => u.Id == userId);
        }

        public void FilterUsers()
        {
            var filteredUsers = AllUsers.AsEnumerable();

            // Apply search term filter
            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                filteredUsers = filteredUsers.Where(u =>
                    u.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    u.Email.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    u.Id.ToString().Contains(SearchTerm));
            }

            // Apply role filter
            if (SelectedRole.HasValue)
            {
                filteredUsers = filteredUsers.Where(u => u.Role == SelectedRole.Value);
            }

            DisplayedUsers = new ObservableCollection<UserDto>(filteredUsers);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}