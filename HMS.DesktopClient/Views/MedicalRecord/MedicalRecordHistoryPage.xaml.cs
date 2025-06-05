<?xml version="1.0" encoding="utf-8"?>
<ResourceDictionary
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:local="using:HMS.DesktopClient.Views.MedicalRecord">

</ResourceDictionary>
using HMS.DesktopClient.ViewModels;
using HMS.Shared.Proxies.Implementations;
using Microsoft.UI.Xaml;

namespace HMS.DesktopClient.Views.MedicalRecord
{
    public sealed partial class MedicalRecordHistoryPage : Window
    {
        public MedicalRecordHistoryViewModel ViewModel { get; }

        public MedicalRecordHistoryPage(int doctorId)
        {
            this.InitializeComponent();
            var proxy = new MedicalRecordProxy(App.CurrentUser!.Token);
            ViewModel = new MedicalRecordHistoryViewModel(doctorId, proxy);
            this.DataContext = ViewModel;
        }
    }
}
