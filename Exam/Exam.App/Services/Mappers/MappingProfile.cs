using AutoMapper;
using Exam.App.Domain;
using Exam.App.Domain.Enum;
using Exam.App.Domain.Models;
using Exam.App.Services.Dtos;
using Exam.App.Services.Dtos.AnimalDTOs.Request;
using Exam.App.Services.Dtos.AnimalDTOs.Response;
using Exam.App.Services.Dtos.AppointmentDTOs.Request;
using Exam.App.Services.Dtos.AppointmentDTOs.Response;
using Exam.App.Services.Dtos.CageDTOs.Request;
using Exam.App.Services.Dtos.CageDTOs.Response;
using Exam.App.Services.Dtos.PatientDTOs.Response;

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
            CreateMap<Patient,PatientSummaryDto>()
            .ForMember(d => d.Species, opt => opt.MapFrom(s => s.Species.Name))
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src =>
                DateTime.Today.Year - src.DateOfBirth.Year -
                (DateTime.Today.DayOfYear < src.DateOfBirth.DayOfYear ? 1 : 0)
            ));

            CreateMap<Appointment, AppointmentSummaryDto>()
                .ForMember(d => d.Patient, opt => opt.MapFrom(s => s.Patient));

            CreateMap<CreateAppointmentDto, Appointment>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status != default ? src.Status : AppointmentStatus.Scheduled));

        }
    }
}

