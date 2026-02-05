using Exam.App.Domain;
using Exam.App.Domain.Models;

namespace Exam.App.Services.Dtos.CageDTOs.Response
{
    public class PatientResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ApplicationUser Owner { get; set; }
        public ApplicationUser Vet { get; set; }
        public AnimalSpecies Species { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
