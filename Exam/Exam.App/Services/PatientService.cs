using AutoMapper;
using Exam.App.Domain;
using Exam.App.Domain.Interface;
using Exam.App.Domain.Models;
using Exam.App.Services.Dtos.AnimalDTOs.Request;
using Exam.App.Services.Dtos.AnimalDTOs.Response;
using Exam.App.Services.Dtos.CageDTOs.Request;
using Exam.App.Services.Dtos.CageDTOs.Response;
using Exam.App.Services.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Exam.App.Services
{
    // Ensure CageService implements ICageService
    public class PatientService : IPatientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public PatientService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<IEnumerable<PatientResponseDto>> GetAllAsync()
        {
            IEnumerable<Patient> patients = await _unitOfWork.PatientRepository.GetAll();
            return _mapper.Map<List<PatientResponseDto>>(patients.ToList());
        }


        public async Task<PatientResponseDto> GetOneById(int id)
        {
            try
            {
                var patient = await _unitOfWork.PatientRepository.GetOne(id);

                if (patient == null)
                {
                    throw new NotFoundException(id);
                }

                // mapiranje entiteta u DTO
                var dto = _mapper.Map<PatientResponseDto>(patient);
                return dto;
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Došlo je do greške prilikom dobavljanja pacijenta.", ex);
            }
        }



        public async Task<PatientResponseDto> AddAsync(PatientCreateRequestDto patientDto)
        {
            try
            {
                var patient = _mapper.Map<Patient>(patientDto);

                if (patientDto.SpeciesId != null)
                {
                    var species = await _unitOfWork.AnimalRepository.GetOneAsync(patientDto.SpeciesId);

                    patient.Species = species;

                    if (species == null)

                        throw new NotFoundException(patientDto.SpeciesId);
                }

                patient.SpeciesId = patientDto.SpeciesId;

                await _unitOfWork.PatientRepository.AddAsync(patient);

                await _unitOfWork.CompleteAsync();

                return _mapper.Map<PatientResponseDto>(patient);
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Došlo je do greške prilikom dodavanja pacijenta.", ex);
            }
        }

        public async Task<PatientResponseDto> UpdateAsync(PatientUpdateRequestDto patientDto)
        {
            try
            {
                var patient = await _unitOfWork.PatientRepository.GetOneAsync(patientDto.Id);

                if (patient == null)
                    throw new NotFoundException(patientDto.Id);

                if (patientDto.SpeciesId != null)
                {
                    var species = await _unitOfWork.AnimalRepository.GetOneAsync(patientDto.SpeciesId);

                    patient.Species = species;

                    if (species == null)

                        throw new NotFoundException(patientDto.SpeciesId);

                    // 🚨 Provera: da li je životinja već u tom kavezu
                    if (patient.SpeciesId == patientDto.SpeciesId)
                        throw new InvalidOperationException(
                            $"Za pacijenta sa ID {patient.Id} je već postavljena vrsta {species.Name}.");
                }

                _mapper.Map(patientDto, patient);

                _unitOfWork.PatientRepository.Update(patient);
                await _unitOfWork.CompleteAsync();

                return _mapper.Map<PatientResponseDto>(patient);
            }
            catch (NotFoundException)
            {
                throw; // propagiraj dalje
            }
            catch (InvalidOperationException)
            {
                throw; // propagiraj dalje
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Došlo je do greške prilikom izmene životinje.", ex);
            }
        }


        public async Task DeleteAsync(int id)
        {
            try
            {
                var patient = await _unitOfWork.PatientRepository.GetOneAsync(id);

                if (patient == null)
                {
                    throw new NotFoundException(id);
                }

                _unitOfWork.PatientRepository.Delete(patient);

                await _unitOfWork.CompleteAsync();
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Došlo je do greške prilikom dodavanja pacijenta.", ex);
            }
        }
    }
}
