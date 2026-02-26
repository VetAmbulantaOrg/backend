namespace Exam.App.Domain.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IAnimalRepository AnimalRepository { get; set; }
        IPatientRepository PatientRepository { get; set; }
        IVetRepository VetRepository { get; set; }
        IAppointmentRepository AppointmentRepository { get; set; }
        IReportRepository ReportRepository { get; set; }
        Task<int> CompleteAsync();
    }
}
