namespace Exam.App.Services.Dtos.ReportDTO_s.Request
{
    public class AppointmentReportDto
    {
        public int AppointmentId { get; set; }
        public double Weight { get; set; } // kilaža ljubimca
        public string Anamnesis { get; set; } // slobodan tekst
        public int VetId { get; set; }
    }

}
