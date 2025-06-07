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
using HMS.DesktopClient.ViewModels.Equipment;
using HMS.Shared.Services;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace HMS.DesktopClient.Views.Doctor
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EquipmentPage : Page
    {
        public EquipmentAllViewModel ViewModel;

        public EquipmentPage()
        {
            this.InitializeComponent();

            this.ViewModel = new EquipmentAllViewModel(new EquipmentService(new EquipmentProxy(App.CurrentUser.Token)));
            this.DataContext = ViewModel;

            _ = ViewModel.LoadAllEquipment();
        }
    }
}
