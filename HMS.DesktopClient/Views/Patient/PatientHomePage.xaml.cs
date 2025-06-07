using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using HMS.Shared.DTOs;
using HMS.Shared.Services;
using HMS.DesktopClient.ViewModels.Notification;
using HMS.Shared.Repositories.Interfaces;
using HMS.Shared.Proxies.Implementations;
using System.Threading.Tasks;
using HMS.DesktopClient.Utils;

namespace HMS.DesktopClient.Views.Patient
{
    public sealed partial class PatientHomePage : Window
    {
        private readonly NotificationViewModel _notificationViewModel;
        private int _currentUserId = App.CurrentUser.Id;
        private string _token = App.CurrentUser.Token;

        public PatientHomePage()
        {
            this.InitializeComponent();

            _notificationViewModel = new NotificationViewModel(new NotificationService(new NotificationProxy(_token)));
            LoadNotificationsAsync();
        }

        private async void Doctors_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(DoctorsSummaryPage));
        }

        private void MedicalRecords_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(MedicalRecordsPage), new MedicalRecordPageParameter
            {
                UserId = App.CurrentUser.Id,
                UserType = "Patient"
            });
        }

        private async void Appointments_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Appointments",
                Content = "Appointments button clicked.",
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };
            await dialog.ShowAsync();
        }

        private async void Profile_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(PatientProfilePage));
        }

        private async void Home_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = null;
        }

        private async void LoadNotificationsAsync()
        {
            try
            {
                var notifications = await _notificationViewModel.GetNotificationsByUserIdAsync(_currentUserId);
                NotificationsList.ItemsSource = notifications;
            }
            catch (Exception ex)
            {
                await ShowErrorDialogAsync("Failed to load notifications: " + ex.Message);
            }
        }

        private async Task ShowErrorDialogAsync(string message)
        {
            var dialog = new ContentDialog
            {
                Title = "Error",
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot // Important!
            };

            await dialog.ShowAsync();
        }
    }
}
