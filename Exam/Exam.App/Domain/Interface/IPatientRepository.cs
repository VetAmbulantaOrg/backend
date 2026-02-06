using Exam.App.Domain.Models;
using Exam.App.Services.Dtos.PatientDTOs.Request;

namespace Exam.App.Domain.Interface
{
    public interface IPatientRepository : IGenericRepository<Patient>
    {
        Task<List<Patient>> GetAll();
        Task<Patient> GetOne(int Id);
        Task<List<Patient>> SearchPatientDetailsAsync(PatientSearchDto search);
    }
}
