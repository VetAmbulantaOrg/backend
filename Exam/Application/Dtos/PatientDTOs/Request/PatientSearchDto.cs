namespace Exam.App.Services.Dtos.PatientDTOs.Request
{
    public class PatientSearchDto
    {
        public int? VetId { get; set; }
        public string? FullNameVet { get; set; }
        public string? PetName { get; set; }
        public string? Species { get; set; }
        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }
        public string? SortType { get; set; }

    }
}
