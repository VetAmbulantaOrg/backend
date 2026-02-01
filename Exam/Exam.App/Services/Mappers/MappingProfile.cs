using AutoMapper;
using Exam.App.Domain;
using Exam.App.Domain.Models;
using Exam.App.Services.Dtos;
using Exam.App.Services.Dtos.AnimalDTOs.Request;
using Exam.App.Services.Dtos.AnimalDTOs.Response;
using Exam.App.Services.Dtos.CageDTOs.Request;
using Exam.App.Services.Dtos.CageDTOs.Response;

namespace Exam.App.Services.Mappers
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, ProfileDto>();
            CreateMap<AnimalCreateRequestDto, AnimalSpecies>();
            CreateMap<AnimalUpdateRequestDto, AnimalSpecies>();
            CreateMap<AnimalSpecies, AnimalResponseDto>();
            CreateMap<PatientCreateRequestDto, Patient>();
            CreateMap<PatientUpdateRequestDto, Patient>();
            CreateMap<Patient,PatientResponseDto>();
        }
    }
}
