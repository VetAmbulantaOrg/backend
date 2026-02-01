using Delivery.Infrastructure.Repositories;
using Exam.App.Domain.Interface;
using Exam.App.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Exam.App.Infrastructure.Database.Repositories
{
    public class PatientRepository : GenericRepository<Patient>, IPatientRepository
    {
        private readonly AppDbContext _dbContext;

        public PatientRepository(AppDbContext dbContext) : base(dbContext) 
        {

            _dbContext = dbContext;
        }

        public async Task<List<Patient>> GetAll()
        {
            return await _dbContext.Patients
                .Include(p => p.Species)
                .ToListAsync();
        }

        public async Task<Patient> GetOne(int Id)
        {
            return await _dbContext.Patients
                .Include(p => p.Species)
                .FirstOrDefaultAsync(p => p.Id == Id);
        }

    }
}
