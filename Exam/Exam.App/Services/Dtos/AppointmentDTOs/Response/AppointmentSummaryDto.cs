using Exam.App.Services.Dtos.PatientDTOs.Response;

namespace Exam.App.Services.Dtos.AppointmentDTOs.Response
{
    public class AppointmentSummaryDto
    {
        public int Id { get; set; }
        public DateTime StartAt { get; set; }
        public int DurationMinutes { get; set; }
        public PatientSummaryDto Patient { get; set; }
    }

}
