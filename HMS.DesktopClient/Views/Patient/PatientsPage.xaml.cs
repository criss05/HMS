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
using HMS.DesktopClient.ViewModels.Patient;
using HMS.Shared.Services;
using HMS.Shared.Proxies.Implementations;
using HMS.Shared.DTOs.Patient;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace HMS.DesktopClient.Views.Patient
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PatientsPage : Window
    {
        public PatientViewModel ViewModel { get; set; }

        public PatientsPage()
        {
            this.InitializeComponent();
            ViewModel = new PatientViewModel(new PatientService(new PatientProxy(App.CurrentUser.Token)));
            PatientsGrid.DataContext = ViewModel;
            _ = ViewModel.InitializeAsync();
        }
    }
}
