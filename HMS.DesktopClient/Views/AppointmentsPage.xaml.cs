using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using HMS.DesktopClient.ViewModels;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace HMS.DesktopClient.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 
    public sealed partial class AppointmentsPage : Page
    {
        private readonly AppointmentsCalendarViewModel _viewModel;

        public AppointmentsPage()
        {
            this.InitializeComponent();
            _viewModel = new AppointmentsCalendarViewModel();
            this.DataContext = _viewModel;
        }

        private async void DoctorCalendar_SelectedDatesChanged(CalendarView sender, CalendarViewSelectedDatesChangedEventArgs args)
        {
            if (args.AddedDates.Count > 0)
            {
                var selectedDate = args.AddedDates[0].Date;
                
                if (App.CurrentDoctor != null)
                {
                    var doctorId = App.CurrentDoctor.Id;
                    var allAppointments = await _viewModel.GetAppointmentsForDoctorAsync(doctorId);
                    var appointmentsForDay = allAppointments
                        .Where(a => a.DateTime.Date == selectedDate)
                        .ToList();

                    AppointmentsGrid.ItemsSource = appointmentsForDay;
                }
                else if (App.CurrentPatient != null)
                {
                    var patientId = App.CurrentPatient.Id;
                    var allAppointments = await _viewModel.GetAppointmentsForPatientAsync(patientId);
                    var appointmentsForDay = allAppointments
                        .Where(a => a.DateTime.Date == selectedDate)
                        .ToList();
                    AppointmentsGrid.ItemsSource = appointmentsForDay;
                }
                else if (App.CurrentAdmin != null)
                {
                    var allAppointments = await _viewModel.GetAllAppointmentsAsync();
                    var appointmentsForDay = allAppointments
                        .Where(a => a.DateTime.Date == selectedDate)
                        .ToList();
                    AppointmentsGrid.ItemsSource = appointmentsForDay;
                }
                else
                {
                    AppointmentsGrid.ItemsSource = null; // No user context available
                }


            }
        }

        private void DoctorCalendar_CalendarViewDayItemChanging(CalendarView sender, CalendarViewDayItemChangingEventArgs args)
        {
            var date = args.Item.Date.Date;

            if (_viewModel.AppointmentCounts.TryGetValue(date, out int count))
            {
                var color = count switch
                {
                    <= 1 => Colors.LightGreen,
                    <= 5 => Colors.Orange,
                    _ => Colors.Red
                };
                args.Item.Background = new SolidColorBrush(color);
            }
        }
    }
}
