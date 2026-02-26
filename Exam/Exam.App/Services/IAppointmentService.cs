using Exam.App.Services.Dtos.AppointmentDTOs.Request;
using Exam.App.Services.Dtos.AppointmentDTOs.Response;

namespace Exam.App.Services
{
    public interface IAppointmentService
    {
        Task<List<AppointmentsByDayDto>> GetUpcomingAppointmentsGroupedByDayAsync(int vetId, DateTime from, DateTime to);
        Task<List<AppointmentsByDayDto>> GetAppointmentsByMonthAsync(AppointmentsByMonthRequestDto request);
        Task<AppointmentSummaryDto> CreateAppointmentAsync(CreateAppointmentDto dto);
        Task<AppointmentSummaryDto> CancelAppointmentAsync(CancelAppointmentDto dto);
    }
}
