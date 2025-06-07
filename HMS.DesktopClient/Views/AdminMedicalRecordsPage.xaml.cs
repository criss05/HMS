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
using HMS.DesktopClient.ViewModels.MedicalRecord;
using HMS.Shared.Services;
using HMS.Shared.Proxies.Implementations;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace HMS.DesktopClient.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AdminMedicalRecordsPage : Page
    {
        public MedicalRecordFullViewModel ViewModel { get; }

        public AdminMedicalRecordsPage()
        {
            this.InitializeComponent();

            this.ViewModel = new MedicalRecordFullViewModel(new MedicalRecordService(new MedicalRecordProxy(App.CurrentUser.Token)));
            this.DataContext = ViewModel;

            _ = ViewModel.LoadDataAsync();
        }
    }
}
