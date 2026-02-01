namespace Exam.App.Services.Dtos.PatientDTOs.Request
{
    public class AnimalSearchDto
    {
        public string? Species { get; set; }
        public string? CageCode { get; set; }
        public bool FilterNonCaged { get; set; } = false;
        public string? SortType { get; set; }

    }
}
