using Exam.App.Domain;
using Exam.App.Domain.Models;

namespace Exam.App.Services
{
    public interface IUserService
    {
        Task<IList<Vet>> GetAllVetsAsync();
    }
}
