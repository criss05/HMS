using HMS.DesktopClient.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace HMS.DesktopClient.Views.Patient
{
    public sealed partial class PatientProfilePage : Page
    {
        public PatientProfileViewModel ViewModel { get; set; }

        public PatientProfilePage()
        {
            this.InitializeComponent();
            ViewModel = new PatientProfileViewModel(App.CurrentUser);
            this.DataContext = ViewModel;
        }
    }
}
