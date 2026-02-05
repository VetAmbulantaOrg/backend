using System.Text.Json.Serialization;

namespace Exam.App.Domain.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SpeciesId { get; set; }
        public ApplicationUser Owner { get; set; }
        public ApplicationUser? Vet { get; set; }
        public AnimalSpecies Species { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
