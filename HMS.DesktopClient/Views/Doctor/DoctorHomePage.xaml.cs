using HMS.DesktopClient.Utils;
using HMS.DesktopClient.ViewModels.Notification;
using HMS.DesktopClient.Views.Patient;
using HMS.Shared.Proxies.Implementations;
using HMS.Shared.Services;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace HMS.DesktopClient.Views.Doctor
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DoctorHomePage : Window
    {
        private readonly NotificationViewModel _notificationViewModel;
        private int _currentUserId = App.CurrentUser.Id;
        private string _token = App.CurrentUser.Token;

        public DoctorHomePage()
        {
            this.InitializeComponent();
            _notificationViewModel = new NotificationViewModel(new NotificationService(new NotificationProxy(_token)));
            LoadNotificationsAsync();
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

        private void Appointments_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Appointments",
                Content = "Appointments button clicked.",
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };
        }

        private void MedicalRecords_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(MedicalRecordsPage), new MedicalRecordPageParameter
            {
                UserId = App.CurrentUser.Id,
                UserType = "Doctor"
            });
        }

        private void Patients_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(PatientsDisplayPage));
        }

        private void Equipments_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Equipments",
                Content = "Equipments button clicked.",
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };
        }

        private void Rooms_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Rooms",
                Content = "Rooms button clicked.",
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };
        }

        private void Schedule_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Schedule",
                Content = "Schedule button clicked.",
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };
        }
        private void Profile_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(DoctorProfilePage));
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = null;
        }

    }
}
