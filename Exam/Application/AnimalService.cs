using System.Security.Claims;
using Application.Interface;
using AutoMapper;
using Exam.App.Domain;
using Exam.App.Domain.Interface;
using Exam.App.Domain.Models;
using Exam.App.Services.Dtos;
using Exam.App.Services.Dtos.AnimalDTOs.Request;
using Exam.App.Services.Dtos.AnimalDTOs.Response;
using Exam.App.Services.Dtos.CageDTOs.Request;
using Exam.App.Services.Dtos.CageDTOs.Response;
using Exam.App.Services.Dtos.PatientDTOs.Request;
using Exam.App.Services.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Exam.App.Services
{
    public class AnimalService : IAnimalService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public AnimalService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }
        public async Task<IEnumerable<AnimalResponseDto>> GetAllAsync()
        {
            IEnumerable<AnimalSpecies> animals = await _unitOfWork.AnimalRepository.GetAllAsync();
            return _mapper.Map<List<AnimalResponseDto>>(animals.ToList());
        }



        public async Task<AnimalResponseDto> GetOneById(int id)
        {
            try
            {
                var animal = await _unitOfWork.AnimalRepository.GetOneAsync(id);

                if (animal == null)
                {
                    throw new NotFoundException(id);
                }

                // mapiranje entiteta u DTO
                var dto = _mapper.Map<AnimalResponseDto>(animal); 
                return dto;
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Došlo je do greške prilikom dobavljanja životinje.", ex);
            }
        }


        public async Task<AnimalResponseDto> AddAsync(AnimalCreateRequestDto animalDto)
        {
            try
            {
                var animal = _mapper.Map<AnimalSpecies>(animalDto);

                await _unitOfWork.AnimalRepository.AddAsync(animal);

                await _unitOfWork.CompleteAsync();

                return _mapper.Map<AnimalResponseDto>(animal);
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Došlo je do greške prilikom dodavanja životinje.", ex);
            }
        }


        public async Task<AnimalResponseDto> UpdateAsync(AnimalUpdateRequestDto animalDto)
        {
            try
            {
                var animal = await _unitOfWork.PatientRepository.GetOneAsync(animalDto.Id);

                if (animal == null)
                {
                    throw new NotFoundException(animalDto.Id);
                }
                _mapper.Map(animalDto, animal);

                _unitOfWork.PatientRepository.Update(animal);

                await _unitOfWork.CompleteAsync();

                return _mapper.Map<AnimalResponseDto>(animal);
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Došlo je do greške prilikom dodavanja pacijenta.", ex);
            }
        }


        public async Task DeleteAsync(int id)
        {
            try
            {
                var animal = await _unitOfWork.AnimalRepository.GetOneAsync(id);

                if (animal == null)
                {
                    throw new NotFoundException(id);
                }

                _unitOfWork.AnimalRepository.Delete(animal);

                await _unitOfWork.CompleteAsync();
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Došlo je do greške prilikom brisanja životinje.", ex);
            }
        }
    }
}
