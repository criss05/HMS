 using HMS.DesktopClient.ViewModels;
using HMS.Shared.DTOs;
using Microsoft.UI.Xaml;

namespace HMS.DesktopClient.Views.MedicalRecord
{
    public sealed partial class MedicalRecordDetailPage : Window
    {
        public MedicalRecordsDetailViewModel ViewModel { get; }

        public MedicalRecordDetailPage(MedicalRecordDto record)
        {
            this.InitializeComponent();
            ViewModel = new MedicalRecordsDetailViewModel(record);
            this.DataContext = ViewModel;
        }
    }
}