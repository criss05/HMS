using HMS.DesktopClient.Views.Patient;
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
using Windows.Foundation;
using Windows.Foundation.Collections;
using HMS.DesktopClient.Views;
using System.Net.Http;
using HMS.DesktopClient.APIClients;
using Microsoft.Extensions.DependencyInjection;
using HMS.Shared.Enums;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace HMS.DesktopClient.Views
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AdminHomePage : Window
    {
        private readonly UserApiClient _userApiClient;

        public AdminHomePage()
        {
            _userApiClient = App.Services.GetRequiredService<UserApiClient>();
            this.InitializeComponent();
        }

        private async void Procedures_Click(object sender, RoutedEventArgs e)
        {
            //MainFrame.Navigate(typeof(ProceduresDisplayPage));
        }

        private async void Rooms_Click(object sender, RoutedEventArgs e)
        {
            // MainFrame.Navigate(typeof(RoomsDisplayPage));
        }

        private async void Equipments_Click(object sender, RoutedEventArgs e)
        {
            // MainFrame.Navigate(typeof(EquipmentDisplayPage));
        }

        private void Users_Click(object sender, RoutedEventArgs e)
        {
            //MainFrame.Navigate(typeof(UsersDisplayPage));
        }

        private async void Appointments_Click(object sender, RoutedEventArgs e)
        {
            // MainFrame.Navigate(typeof(AppointmentsDisplayPage));
        }

        private async void Schedules_Click(object sender, RoutedEventArgs e)
        {
            // MainFrame.Navigate(typeof(SchedulesDisplayPage));
        }

        private async void Shifts_Click(object sender, RoutedEventArgs e)
        {
            // MainFrame.Navigate(typeof(ShiftsDisplayPage));
        }

        private async void MedicalRecords_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(MedicalRecordsPage));
        }

        private async void Home_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = null; 
        }

        private async void Profile_Click(object sender, RoutedEventArgs e)
        {
           MainFrame.Navigate(typeof(AdminProfilePage));
        }

        private void LogsButton_Click(object sender, RoutedEventArgs e)
        {
            var adminDashboard = new LogsPage();
            adminDashboard.Activate();
        }

        private void ShiftsButton_Click(object sender, RoutedEventArgs e)
        {
            HttpClient httpClient = App.Services.GetRequiredService<HttpClient>();

            if (App.CurrentUser != null)
            {
                var authToken = App.CurrentUser.Token;
                var userRole = (int)App.CurrentUser.Role;

                var shiftWindow = new ShiftView(httpClient, authToken, userRole);
                shiftWindow.Activate();
            }
            else
            {
                Console.WriteLine("Error: User not logged in.");
                // TODO: Implement proper handling for not logged in user.
            }
        }
    }
}
