using HMS.DesktopClient.ViewModels.Patient;
using HMS.Shared.DTOs.Patient;
using HMS.Shared.Proxies.Implementations;
using HMS.Shared.Services;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace HMS.DesktopClient.Views
{
    public sealed partial class RegisterPage : Window
    {
        public RegisterPage()
        {
            this.InitializeComponent();
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dto = new PatientCreateDto
                {
                    Email = EmailBox.Text,
                    Password = PasswordBox.Password,
                    Name = NameBox.Text,
                    CNP = CnpBox.Text,
                    PhoneNumber = PhoneNumberBox.Text,
                    Role = "Patient", // Fixed value
                    BloodType = (BloodTypeBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "",
                    EmergencyContact = EmergencyContactBox.Text,
                    Allergies = AllergiesBox.Text,
                    Weight = float.TryParse(WeightBox.Text, out var w) ? w : 0,
                    Height = float.TryParse(HeightBox.Text, out var h) ? h : 0,
                    BirthDate = BirthDatePicker.Date.DateTime,
                    Address = AddressBox.Text
                };

                RegisterViewModel registerViewModel = new RegisterViewModel(new PatientService(new PatientProxy("")));
                PatientCreateDto patientCreateDto = await registerViewModel.RegisterPatientAsync(dto);
                ErrorTextBlock.Text = "Registration successful! Redirecting to login...";
                ErrorTextBlock.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Colors.Green);
                ErrorTextBlock.Visibility = Visibility.Visible;

                await Task.Delay(3000);

                var loginPage = new LoginPage();
                loginPage.Activate();

                this.Close();
            }
            catch (Exception ex)
            {
                ErrorTextBlock.Text = $"Error: {ex.Message}";
                ErrorTextBlock.Visibility = Visibility.Visible;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var loginPage = new LoginPage();
            loginPage.Activate();
            this.Close();
        }
    }
}
