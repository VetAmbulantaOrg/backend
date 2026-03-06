using Delivery.Infrastructure.Repositories;
using Exam.App.Domain.Enum;
using Exam.App.Domain.Interface;
using Exam.App.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Exam.App.Infrastructure.Database.Repositories
{
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
    {
        private readonly AppDbContext _dbContext;

        public AppointmentRepository(AppDbContext dbContext) : base(dbContext)
        {

            _dbContext = dbContext;
        }

        public async Task<List<Appointment>> GetUpcomingAppointmentsAsync(int vetId, DateTime from, DateTime to)
        {
            return await _dbContext.Appointments
                .AsNoTracking()
                .Include(a => a.Patient)
                .Include(a => a.Patient.Species)
                .Include(a => a.Report)
                .Where(a => a.VetId == vetId
                            && a.StartAt >= from
                            && a.StartAt <= to)
                .OrderBy(a => a.StartAt)
                .ToListAsync();
        }

        public async Task<Appointment> GetOne(int id)
        {
            return await _dbContext.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Patient.Species)
                .Include(a => a.Report)
                .Include(a => a.Vet)
                .Where(a => a.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> IsVetAvailableAsync(int vetId, DateTime startAt, int durationMinutes)
        {
            var endAt = startAt.AddMinutes(durationMinutes);

            return !await _dbContext.Appointments
                .AsNoTracking()
                .Where(a => a.VetId == vetId && a.Status == AppointmentStatus.Scheduled)
                .AnyAsync(a =>
                    // preklapanje termina
                    (startAt < a.StartAt.AddMinutes(a.DurationMinutes)) &&
                    (endAt > a.StartAt)
                );
        }


        public async Task<bool> IsPatientAvailableAsync(int patientId, DateTime startAt, int durationMinutes) 
        { 
            var endAt = startAt.AddMinutes(durationMinutes);

            return !await _dbContext.Appointments
                .Where(a => a.PatientId == patientId && a.Status == AppointmentStatus.Scheduled)
                .AnyAsync(a => startAt < a.StartAt.AddMinutes(a.DurationMinutes) && endAt > a.StartAt); 
        }

    }
}
