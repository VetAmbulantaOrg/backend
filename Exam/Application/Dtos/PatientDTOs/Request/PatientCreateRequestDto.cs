using Exam.App.Domain.Models;

namespace Exam.App.Services.Dtos.CageDTOs.Request
{
    public class PatientCreateRequestDto
    {
        public string Name { get; set; }
        public int SpeciesId { get; set; }
        public int OwnerId { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
