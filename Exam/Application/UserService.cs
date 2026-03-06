using Application.Dtos.OwnerDTOs.Request;
using Application.Dtos.OwnerDTOs.Response;
using Application.Interface;
using AutoMapper;
using Domain.Models;
using Exam.App.Domain;
using Exam.App.Domain.Interface;
using Exam.App.Domain.Models;
using Exam.App.Services.Dtos.CageDTOs.Response;
using Microsoft.AspNetCore.Identity;

namespace Exam.App.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;

        }

        public async Task<IList<Vet>> GetAllVetsAsync()
        {
            var vets = await _unitOfWork.VetRepository.GetAllAsync();
            return vets.ToList();
        }

        public async Task<OwnerDetailsDTO> CreateOwner(OwnerCreateRequestDTO dto)
        {
            var owner = _mapper.Map<Owner>(dto);

            await _unitOfWork.OwnerRepository.AddAsync(owner);

            await _unitOfWork.CompleteAsync();

            return _mapper.Map<OwnerDetailsDTO>(owner);
        }

        public async Task<List<OwnerDTO>> GetOwnersByFullName(string fullName)
        {
            var owners = await _unitOfWork.OwnerRepository.GetByFullName(fullName);

            return _mapper.Map<List<OwnerDTO>>(owners.ToList());
        }

    }
}
