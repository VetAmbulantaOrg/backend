namespace Exam.App.Domain.Models
{
    public class Vet : ApplicationUser
    {
        public ICollection<Patient> Patients { get; set; }
        public ICollection<Appointment> Appointments { get; set; }

    }
}
