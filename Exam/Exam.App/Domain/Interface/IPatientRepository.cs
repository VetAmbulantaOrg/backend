using Exam.App.Domain.Models;

namespace Exam.App.Domain.Interface
{
    public interface IPatientRepository : IGenericRepository<Patient>
    {
        Task<List<Patient>> GetAll();
        Task<Patient> GetOne(int Id);
    }
}
