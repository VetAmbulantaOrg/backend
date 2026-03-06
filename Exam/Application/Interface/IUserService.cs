using Application.Dtos.OwnerDTOs.Request;
using Application.Dtos.OwnerDTOs.Response;
using Exam.App.Domain;
using Exam.App.Domain.Models;

namespace Application.Interface
{
    public interface IUserService
    {
        Task<IList<Vet>> GetAllVetsAsync();
        Task<OwnerDetailsDTO> CreateOwner(OwnerCreateRequestDTO dto);
        Task<List<OwnerDTO>> GetOwnersByFullName(string fullName);
    }
}
