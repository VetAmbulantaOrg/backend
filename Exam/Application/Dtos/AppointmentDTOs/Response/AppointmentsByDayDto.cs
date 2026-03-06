namespace Exam.App.Services.Dtos.AppointmentDTOs.Response
{
    public class AppointmentsByDayDto
    {
        public DateTime Date { get; set; } // samo datum bez vremena
        public List<AppointmentSummaryDto> Appointments { get; set; }
    }

}
