using Exam.App.Domain;
using Exam.App.Domain.Interface;
using Exam.App.Services.Dtos.CageDTOs.Response;
using Microsoft.AspNetCore.Identity;

namespace Exam.App.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<IList<ApplicationUser>> GetAllVetsAsync()
        {
            return await _userManager.GetUsersInRoleAsync("Veterinar");
        }

    }
}
