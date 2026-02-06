namespace Exam.App.Domain.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IAnimalRepository AnimalRepository { get; set; }
        IPatientRepository PatientRepository { get; set; }
        IVetRepository VetRepository { get; set; }
        Task<int> CompleteAsync();
    }
}
