using HMS.DesktopClient.ViewModels;
using HMS.Shared.Proxies.Implementations;
using Microsoft.UI.Xaml;

namespace HMS.DesktopClient.Views.MedicalRecord
{
    public sealed partial class MedicalRecordCreatePage : Window
    {
        public MedicalRecordCreationFormViewModel ViewModel { get; }

        public MedicalRecordCreatePage()
        {
            this.InitializeComponent();
            ViewModel = new MedicalRecordCreationFormViewModel(
                new DoctorProxy(App.CurrentUser!.Token),
                new ProcedureProxy(App.CurrentUser!.Token),
                new DepartmentProxy(App.CurrentUser!.Token),
                new PatientProxy(App.CurrentUser!.Token));
            this.DataContext = ViewModel;
            _ = ViewModel.InitializeAsync();
        }
    }
    public partial class MedicalRecordDetailPage : Window
    {
        public MedicalRecordDetailPage()
        {
            InitializeComponent();
        }
    }
}