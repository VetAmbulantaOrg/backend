using Domain.Interface;

namespace Exam.App.Domain.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IAnimalRepository AnimalRepository { get; set; }
        IPatientRepository PatientRepository { get; set; }
        IOwnerRepository OwnerRepository { get; set; }
        IVetRepository VetRepository { get; set; }
        IAppointmentRepository AppointmentRepository { get; set; }
        IReportRepository ReportRepository { get; set; }
        Task<int> CompleteAsync();
    }
}
