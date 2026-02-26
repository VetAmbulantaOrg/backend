using Exam.App.Domain.Models;

namespace Exam.App.Domain.Interface
{
    public interface IAppointmentRepository : IGenericRepository<Appointment>
    {
        Task<List<Appointment>> GetUpcomingAppointmentsAsync(int vetId, DateTime from, DateTime to);
        Task<bool> IsVetAvailableAsync(int vetId, DateTime startAt, int durationMinutes);
        Task<bool> IsPatientAvailableAsync(int patientId, DateTime startAt, int durationMinutes);
        Task<Appointment> GetOne(int id);
    }
}
