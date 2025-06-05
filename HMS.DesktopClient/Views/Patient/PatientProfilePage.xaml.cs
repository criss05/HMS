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

            if (success)
            {
                // Optionally notify the user
                ContentDialog dialog = new ContentDialog
                {
                    Title = "Success",
                    Content = "Profile updated successfully.",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                };
                await dialog.ShowAsync();
            }
            else
            {
                ContentDialog dialog = new ContentDialog
                {
                    Title = "Error",
                    Content = "Failed to update profile.",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                };
                await dialog.ShowAsync();
            }
        }
    }
}
