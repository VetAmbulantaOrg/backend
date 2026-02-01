using Exam.App.Services.Dtos.AnimalDTOs.Request;
using Exam.App.Services.Dtos.AnimalDTOs.Response;
using Exam.App.Services.Dtos.PatientDTOs.Request;

namespace Exam.App.Services
{
    public interface IAnimalService
    {
        Task<AnimalResponseDto> AddAsync(AnimalCreateRequestDto animalDto);
        Task<AnimalResponseDto> GetOneById(int id);
        Task<IEnumerable<AnimalResponseDto>> GetAllAsync();
        Task<AnimalResponseDto> UpdateAsync(AnimalUpdateRequestDto animalDto);
        Task DeleteAsync(int id);

    }
}
