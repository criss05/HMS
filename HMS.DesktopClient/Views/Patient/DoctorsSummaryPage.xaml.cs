using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using HMS.DesktopClient.ViewModels.Doctor;
using HMS.Shared.Proxies.Implementations;
using HMS.Shared.Services;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace HMS.DesktopClient.Views.Patient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DoctorsSummaryPage : Page
    {
        private readonly DoctorSummaryViewModel _viewModel;

        public DoctorsSummaryPage()
        {
            this.InitializeComponent();

            this._viewModel = new DoctorSummaryViewModel(new DoctorService(new DoctorProxy(App.CurrentUser.Token)));
            _ = LoadDoctorsAsync();
        }

        private async Task LoadDoctorsAsync()
        {
            try
            {
                var doctors = await _viewModel.LoadDoctorsSummary();
                DoctorsListView.ItemsSource = doctors;
            }
            catch (Exception ex)
            {
                var dialog = new ContentDialog()
                {
                    Title = "Error loading doctors",
                    Content = ex.Message,
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                };
                await dialog.ShowAsync();
            }
        }
    }
}
