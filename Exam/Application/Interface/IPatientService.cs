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

namespace Application.Interface
{
    public interface IPatientService
    {
        Task<PagedResultDto<PatientResponseDto>> SearchPatientDetailsAsync(PatientSearchDto searchDto, int page, int pageSize);
        Task<IEnumerable<PatientResponseDto>> GetAllAsync();
        Task<List<PatientResponseDto>> GetByVet(int VetId);
        Task<PatientResponseDto> GetOneById(int id);
        Task<PatientResponseDto> AddAsync(PatientCreateRequestDto cageDto);
        Task<PatientResponseDto> UpdateAsync(PatientUpdateRequestDto cageDto);
        Task DeleteAsync(int id);
    }
}
