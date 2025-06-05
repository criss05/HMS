using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using HMS.DesktopClient.APIClients;
using HMS.DesktopClient.Views.Patient;
using HMS.Shared.DTOs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace HMS.DesktopClient.Views
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Window
    {
        private readonly UserApiClient userApiClient;
        public LoginPage()
        {
            this.userApiClient = App.Services.GetRequiredService<UserApiClient>();
            this.InitializeComponent();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UserWithTokenDto userWithToken = await this.userApiClient.Login(this.Email.Text, this.Password.Password);

                if (userWithToken == null)
                {
                    this.errorMessage.Text = "Invalid username or password.";
                    this.errorMessage.Visibility = Visibility.Visible;
                    return;
                }

                App.CurrentUser = userWithToken; // Store the user with token in the current application state

            }
            catch (Exception ex)
            {
                this.errorMessage.Text = ex.Message;
                this.errorMessage.Visibility = Visibility.Visible;
                return;
            }

            this.errorMessage.Visibility = Visibility.Collapsed;

            if(App.CurrentUser.Role == Shared.Enums.UserRole.Patient)
            {
                var patientHomePage = new PatientHomePage();
                patientHomePage.Activate();
            }
            else if(App.CurrentUser.Role == Shared.Enums.UserRole.Doctor)
            {
                var doctorHomePage = new DoctorHomePage();
                doctorHomePage.Activate();
            }
            else if(App.CurrentUser.Role == Shared.Enums.UserRole.Admin)
            {
                var adminHomePage = new AdminHomePage();
                adminHomePage.Activate();
            }
            this.Close();
        }
    }
}
