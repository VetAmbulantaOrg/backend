using Exam.App.Domain;

namespace Exam.App.Services
{
    public interface IUserService
    {
        Task<IList<ApplicationUser>> GetAllVetsAsync();
    }
}
