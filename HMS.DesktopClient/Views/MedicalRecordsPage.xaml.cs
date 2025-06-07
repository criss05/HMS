using HMS.DesktopClient.Utils;
using HMS.DesktopClient.ViewModels;
using HMS.Shared.Proxies.Implementations;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace HMS.DesktopClient.Views
{
    public sealed partial class MedicalRecordsPage : Page
    {
        public MedicalRecordHistoryViewModel? ViewModel { get; private set; }

        public MedicalRecordsPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is int patientId)
            {
                var proxy = new MedicalRecordProxy(App.CurrentUser!.Token);
                ViewModel = new MedicalRecordHistoryViewModel(proxy);
                RecordsGrid.DataContext = ViewModel;
            }

            if (e.Parameter is MedicalRecordPageParameter param)
            {
                var proxy = new MedicalRecordProxy(App.CurrentUser!.Token);

                if (param.UserType == "Patient")
                {
                    ViewModel = new MedicalRecordHistoryViewModel(proxy);
                    await ViewModel.LoadRecordsForPatientAsync(param.UserId); // You implement this
                }
                else if (param.UserType == "Doctor")
                {
                    ViewModel = new MedicalRecordHistoryViewModel(proxy);
                    await ViewModel.LoadRecordsForDoctorAsync(param.UserId); // You implement this
                }

                RecordsGrid.DataContext = ViewModel;
            }
        }
    }
}