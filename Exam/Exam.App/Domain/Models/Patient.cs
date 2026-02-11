using System.Text.Json.Serialization;

namespace Exam.App.Domain.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SpeciesId { get; set; }
        public AnimalSpecies Species { get; set; }
        public int OwnerId { get; set; }
        public ApplicationUser Owner { get; set; }
        public int? VetId { get; set; }
        public Vet? Vet { get; set; }
        public DateTime DateOfBirth { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}
