using HMS.DesktopClient.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using HMS.Shared.DTOs;
using System.Threading.Tasks;
using HMS.Shared.Enums;

namespace HMS.DesktopClient.Views
{
    public sealed partial class UsersDisplayPage : Page
    {
        public UserManagementViewModel ViewModel { get; private set; }

        public UsersDisplayPage()
        {
            this.InitializeComponent();
            ViewModel = new UserManagementViewModel(App.CurrentUser);
            this.DataContext = ViewModel;

            // Set initial role filter to "All"
            RoleFilter.SelectedIndex = 0;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await LoadUsersAsync();
        }

        private async Task LoadUsersAsync()
        {
            try
            {
                await ViewModel.LoadUsersAsync();
            }
            catch (Exception ex)
            {
                await ShowErrorDialogAsync("Failed to load users: " + ex.Message);
            }
        }

        private void HighlightSelectedUser(UserDto selectedUser)
        {
            foreach (var item in UsersListView.Items)
            {
                if (item is UserDto user && user.Id == selectedUser.Id)
                {
                    UsersListView.SelectedItem = item;
                    UsersListView.ScrollIntoView(item);
                    break;
                }
            }
        }

        private async void EditUser_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int userId)
            {
                var user = ViewModel.GetUserById(userId);
                if (user != null)
                {
                    HighlightSelectedUser(user);
                    await ShowEditUserDialog(user);
                }
            }
        }

        private async Task ShowEditUserDialog(UserDto user)
        {
            var dialog = new ContentDialog
            {
                Title = $"Edit User - {user.Name}",
                XamlRoot = this.XamlRoot
            };

            var panel = new StackPanel { Spacing = 10 };

            // Name field
            panel.Children.Add(new TextBlock { Text = "Name:" });
            var nameTextBox = new TextBox { Text = user.Name };
            panel.Children.Add(nameTextBox);

            // Email field
            panel.Children.Add(new TextBlock { Text = "Email:" });
            var emailTextBox = new TextBox { Text = user.Email };
            panel.Children.Add(emailTextBox);

            // Phone number field
            panel.Children.Add(new TextBlock { Text = "Phone Number:" });
            var phoneTextBox = new TextBox { Text = user.PhoneNumber };
            panel.Children.Add(phoneTextBox);

            // Role field (display only, not editable)
            panel.Children.Add(new TextBlock { Text = "Role:" });
            var roleTextBlock = new TextBlock { Text = user.Role.ToString() };
            panel.Children.Add(roleTextBlock);

            // CNP Field
            panel.Children.Add(new TextBlock { Text = "CNP:" });
            var cnpTextBox = new TextBox { Text = user.CNP };
            panel.Children.Add(cnpTextBox);

            dialog.Content = panel;
            dialog.PrimaryButtonText = "Save";
            dialog.SecondaryButtonText = "Cancel";

            dialog.PrimaryButtonClick += async (s, args) =>
            {
                user.Name = nameTextBox.Text;
                user.Email = emailTextBox.Text;
                user.PhoneNumber = phoneTextBox.Text;
                user.CNP = cnpTextBox.Text;

                var result = await ViewModel.UpdateUserAsync(user);
                if (!result)
                {
                    args.Cancel = true;
                    await ShowErrorDialogAsync("Failed to update user information.");
                }
            };

            await dialog.ShowAsync();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel.SearchTerm = SearchBox.Text;
            ViewModel.FilterUsers();
        }

        private void RoleFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RoleFilter.SelectedItem is ComboBoxItem selectedItem)
            {
                string role = selectedItem.Content.ToString();
                if (role == "All")
                    ViewModel.SelectedRole = null;
                else
                    ViewModel.SelectedRole = Enum.Parse<UserRole>(role);

                ViewModel.FilterUsers();
            }
        }

        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            await LoadUsersAsync();
        }

        private void UsersListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // You can implement additional functionality when a user is selected
        }

        private async Task ShowErrorDialogAsync(string message)
        {
            var dialog = new ContentDialog
            {
                Title = "Error",
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };

            await dialog.ShowAsync();
        }
    }
}
