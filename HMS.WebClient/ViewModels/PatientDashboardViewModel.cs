using HMS.Shared.DTOs;
using System.Collections.Generic;

namespace HMS.WebClient.ViewModels
{
    public class PatientDashboardViewModel
    {
        public IEnumerable<AppointmentDto> UpcomingAppointments { get; set; } = new List<AppointmentDto>();
        public IEnumerable<MedicalRecordDto> RecentMedicalRecords { get; set; } = new List<MedicalRecordDto>();
        public IEnumerable<NotificationDto> Notifications { get; set; } = new List<NotificationDto>();
    }
}