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
                var doctorProxy = new DoctorProxy(App.CurrentUser.Token);
                var patientProxy = new PatientProxy(App.CurrentUser.Token);
                var procedureProxy = new ProcedureProxy(App.CurrentUser.Token);
                ViewModel = new MedicalRecordHistoryViewModel(proxy, doctorProxy, patientProxy, procedureProxy);
                RecordsGrid.DataContext = ViewModel;
            }

            if (e.Parameter is MedicalRecordPageParameter param)
            {
                var proxy = new MedicalRecordProxy(App.CurrentUser!.Token);
                var doctorProxy = new DoctorProxy(App.CurrentUser.Token);
                var patientProxy = new PatientProxy(App.CurrentUser.Token);
                var procedureProxy = new ProcedureProxy(App.CurrentUser.Token);

                if (param.UserType == "Patient")
                {
                    ViewModel = new MedicalRecordHistoryViewModel(proxy, doctorProxy, patientProxy, procedureProxy);
                    RecordsGrid.Columns[1].Header = "Doctor";
                    await ViewModel.LoadRecordsForPatientAsync(param.UserId);
                }
                else if (param.UserType == "Doctor")
                {
                    ViewModel = new MedicalRecordHistoryViewModel(proxy, doctorProxy, patientProxy, procedureProxy);
                    RecordsGrid.Columns[1].Header = "Patient";
                    await ViewModel.LoadRecordsForDoctorAsync(param.UserId);
                }

                RecordsGrid.DataContext = ViewModel;
            }
        }
    }
}