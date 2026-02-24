namespace Exam.App.Services.Dtos.AppointmentDTOs.Request
{
    public class CancelAppointmentDto
    {
        public int AppointmentId { get; set; }
        public int VetId { get; set; }
        public string Reason { get; set; }
    }

}
