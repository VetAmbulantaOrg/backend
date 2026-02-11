using Exam.App.Domain.Enum;

namespace Exam.App.Domain.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public DateTime StartAt { get; set; }    // datum i vreme početka
        public int DurationMinutes { get; set; } // opcionalno
        public AppointmentStatus Status { get; set; }      
        public int VetId { get; set; }
        public Vet Vet { get; set; }
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
    }
}
