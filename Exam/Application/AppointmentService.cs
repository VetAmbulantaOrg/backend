using Application.Interface;
using AutoMapper;
using Exam.App.Domain.Enum;
using Exam.App.Domain.Interface;
using Exam.App.Domain.Models;
using Exam.App.Services.Dtos.AppointmentDTOs.Request;
using Exam.App.Services.Dtos.AppointmentDTOs.Response;
using Exam.App.Services.Dtos.ReportDTO_s.Request;
using Exam.App.Services.Exceptions;

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

        public async Task<List<AppointmentsByDayDto>> GetAppointmentsByMonthAsync(AppointmentsByMonthRequestDto request)
        {
            var from = new DateTime(request.Year, request.Month, 1).ToUniversalTime();
            var to = from.AddMonths(1);

            return await GetUpcomingAppointmentsGroupedByDayAsync(request.VetId, from, to);
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
            // 0. Validacija da termin mora biti u budućnosti
            if (dto.StartAt <= DateTime.UtcNow) 
            { 
                throw new InvalidOperationException("Pregled se mora zakazati u budućem vremenu."); 
            }

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

        public async Task<AppointmentSummaryDto> CancelAppointmentAsync(CancelAppointmentDto dto)
        {
            var appointment = await _unitOfWork.AppointmentRepository.GetOne(dto.AppointmentId);
            if (appointment == null)
            {
                throw new NotFoundException(dto.AppointmentId);
            }

            if (appointment.VetId != dto.VetId)
            {
                throw new UnauthorizedAccessException("Veterinar može otkazati samo svoje termine.");
            }

            appointment.Status = 0;
            appointment.CancellationReason = dto.Reason;

            _unitOfWork.AppointmentRepository.Update(appointment);

            await _unitOfWork.CompleteAsync();

            return _mapper.Map<AppointmentSummaryDto>(appointment);
        }

        public async Task<AppointmentSummaryDto> SubmitReportAsync(AppointmentReportDto dto)
        {
            var appointment = await _unitOfWork.AppointmentRepository.GetOneAsync(dto.AppointmentId);

            if (appointment == null || appointment.VetId != dto.VetId)
                throw new UnauthorizedAccessException("Pregled ne postoji ili nije vaš.");

            var report = new Report
            {
                AppointmentId = appointment.Id,
                Weight = dto.Weight,
                Anamnesis = dto.Anamnesis,
                CreatedAt = DateTime.UtcNow,
                LastModifiedAt = DateTime.UtcNow
            };

            // Save the report (assuming a ReportRepository exists)
            await _unitOfWork.ReportRepository.AddAsync(report);

            appointment.Report = report;
            appointment.Status = AppointmentStatus.Completed;

             _unitOfWork.AppointmentRepository.Update(appointment);

            await _unitOfWork.CompleteAsync();

            return _mapper.Map<AppointmentSummaryDto>(appointment);
        }


        public async Task<AppointmentSummaryDto> UpdateReportAsync(AppointmentReportDto dto)
        {
            var appointment = await _unitOfWork.AppointmentRepository.GetOne(dto.AppointmentId);

            if (appointment == null || appointment.VetId != dto.VetId)
                throw new UnauthorizedAccessException("Pregled ne postoji ili nije vaš.");

            // Provera roka od 3 radna dana
            if (!CanModifyReport(appointment.Report.CreatedAt))
                throw new InvalidOperationException("Izveštaj se više ne može menjati.");

            appointment.Report.Weight = dto.Weight;
            appointment.Report.Anamnesis = dto.Anamnesis;
            appointment.Report.LastModifiedAt = DateTime.UtcNow;

            _unitOfWork.ReportRepository.Update(appointment.Report);

            await _unitOfWork.CompleteAsync();

            return _mapper.Map<AppointmentSummaryDto>(appointment);
        }


        private bool CanModifyReport(DateTime createdAt)
        {
            var date = createdAt.Date;
            int daysCount = 0;

            while (daysCount < 3)
            {
                date = date.AddDays(1);
                if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                {
                    daysCount++;
                }
            }

            return DateTime.UtcNow.Date <= date;
        }


    }
}


