using HMS.DesktopClient.ViewModels;
using System.Net.Http;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using HMS.Shared.Enums;
using System;
using Microsoft.UI.Xaml.Navigation;
using static HMS.DesktopClient.Views.AdminHomePage;

namespace HMS.DesktopClient.Views
{
    public partial class ShiftView : Page
    {
        private ShiftViewModel viewModel;

        public ShiftView()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is ShiftViewParams navParams)
            {
                viewModel = new ShiftViewModel(navParams.HttpClient, navParams.Token);
                viewModel.UserRole = navParams.UserRole;
                this.DataContext = viewModel;
                _ = viewModel.LoadShiftsAsync();
            }
        }
    }
}