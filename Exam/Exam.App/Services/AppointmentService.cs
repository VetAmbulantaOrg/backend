using AutoMapper;
using Exam.App.Domain.Interface;
using Exam.App.Domain.Models;
using Exam.App.Infrastructure;
using Exam.App.Infrastructure.Database;
using Exam.App.Infrastructure.Database.Repositories;
using Exam.App.Services;
using Exam.App.Services.Dtos.AppointmentDTOs.Request;
using Exam.App.Services.Dtos.AppointmentDTOs.Response;
using Microsoft.EntityFrameworkCore;

namespace Exam.App.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AppointmentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
        }

        public async Task<List<AppointmentsByDayDto>> GetAppointmentsForCurrentMonthAsync(int vetId)
        {
            var today = DateTime.UtcNow.Date; // ili DateTime.Now ako radiš u lokalnom vremenu
            var from = new DateTime(today.Year, today.Month, 1).ToUniversalTime();
            var to = from.AddMonths(1).AddDays(-1);

            return await GetUpcomingAppointmentsGroupedByDayAsync(vetId, from, to);
        }


        public async Task<List<AppointmentsByDayDto>> GetUpcomingAppointmentsGroupedByDayAsync(int vetId, DateTime from, DateTime to)
        {
            var appointments = await _unitOfWork.AppointmentRepository.GetUpcomingAppointmentsAsync(vetId, from, to);


            var mapped = _mapper.Map<List<AppointmentSummaryDto>>(appointments);

            var grouped = mapped
                .GroupBy(a => a.StartAt.Date)
                .Select(g => new AppointmentsByDayDto
                {
                    Date = g.Key,
                    Appointments = g.OrderBy(a => a.StartAt).ToList()
                })
                .OrderBy(g => g.Date)
                .ToList();

            return grouped;
        }

        public async Task<AppointmentSummaryDto> CreateAppointmentAsync(CreateAppointmentDto dto)
        {
            // 1. Validacija dostupnosti veterinara
            var vetAvailable = await _unitOfWork.AppointmentRepository.IsVetAvailableAsync(dto.VetId, dto.StartAt, dto.DurationMinutes);

            if (!vetAvailable)
            {
                throw new InvalidOperationException("Veterinar nije dostupan u traženom terminu.");
            }

            // 2. (Opcionalno) Validacija dostupnosti pacijenta
            var patientAvailable = await _unitOfWork.AppointmentRepository.IsPatientAvailableAsync(dto.PatientId, dto.StartAt, dto.DurationMinutes);

            if (!patientAvailable)
            {
                throw new InvalidOperationException("Pacijent već ima zakazan pregled u traženom terminu.");
            }
            var patient = await _unitOfWork.PatientRepository.GetOneAsync(dto.PatientId);

            // 3. Mapiranje DTO → entitet
            var appointment = _mapper.Map<Appointment>(dto);

            appointment.Patient = patient;

            // 4. Snimanje u bazu
            await _unitOfWork.AppointmentRepository.AddAsync(appointment);

            // 5. Commit transakcije
            await _unitOfWork.CompleteAsync();

            // 6. Vraćanje odgovora
            return _mapper.Map<AppointmentSummaryDto>(appointment);
        }
    }
}


