using HMS.DesktopClient.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace HMS.DesktopClient.Views.Patient
{
    public sealed partial class PatientProfilePage : Page
    {
        public PatientProfileViewModel ViewModel { get; set; }

        public PatientProfilePage()
        {
            this.InitializeComponent();
            ViewModel = new PatientProfileViewModel(App.CurrentUser, App.CurrentPatient);
            this.DataContext = ViewModel;
        }

        private async void OnUpdateButtonClick(object sender, RoutedEventArgs e)
        {
            bool success = await ViewModel.UpdatePatientAsync();

            if (this.XamlRoot == null)
                return; // Or delay dialog, or fallback to simpler alert mechanism

            var dialog = new ContentDialog
            {
                Title = success ? "Success" : "Error",
                Content = success ? "Profile updated successfully." : "Failed to update profile.",
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };

            await dialog.ShowAsync();
        }

        private void LogOutClick(object sender, RoutedEventArgs e)
        {
            // Clear the current user and navigate to the login page
            App.CurrentUser = null;
            App.CurrentPatient = null;
            // Navigate to the login page
            var loginWindow = new LoginPage();
            loginWindow.Activate();

            // close the current window
            if (Window.Current != null)
            {
                Window.Current.Close();
            }
            else
            {
                // If Window.Current is null, you might need to handle it differently
                // depending on your application's structure.
                Console.WriteLine("Error: Current window is not available.");
            }
        }
    }
}
