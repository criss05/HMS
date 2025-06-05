using HMS.DesktopClient.ViewModels;
using HMS.Shared.Proxies.Implementations;
using Microsoft.UI.Xaml;

namespace HMS.DesktopClient.Views
{
    public sealed partial class MedicalRecordsPage : Window
    {
        public MedicalRecordHistoryViewModel ViewModel { get; }

        public MedicalRecordsPage(int patientId)
        {
            this.InitializeComponent();
            var proxy = new MedicalRecordProxy(App.CurrentUser!.Token);
            ViewModel = new MedicalRecordHistoryViewModel(patientId, proxy);
            RecordsGrid.DataContext = ViewModel;
        }
    }
}