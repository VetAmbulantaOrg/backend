using Exam.App.Domain.Models;

namespace Exam.App.Services.Dtos.CageDTOs.Request
{
    public class PatientUpdateRequestDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SpeciesId { get; set; }
        public AnimalSpecies Species { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
