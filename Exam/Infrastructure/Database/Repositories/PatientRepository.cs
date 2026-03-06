using Delivery.Infrastructure.Repositories;
using Domain.Models;
using Exam.App.Domain.Interface;
using Exam.App.Domain.Models;
using Exam.App.Services.Dtos.PatientDTOs.Request;
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

        public async Task<List<Patient>> SearchPatientDetailsAsync(PatientSearchFilter filter)
        {
            var query = _dbContext.Patients
                .Include(p => p.Species)
                .Include(p => p.Owner)
                .Include(p => p.Vet)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.FullNameVet))
            {
                var fullNameVet = filter.FullNameVet.ToLower();
                query = query.Where(p =>
                    (p.Vet.Name + " " + p.Vet.Surname).ToLower().Contains(fullNameVet));
            }

            if (!string.IsNullOrWhiteSpace(filter.PetName))
                query = query.Where(p => p.Name != null &&
                                         p.Name.ToLower().Contains(filter.PetName.ToLower()));

            if (!string.IsNullOrWhiteSpace(filter.Species))
                query = query.Where(p => p.Species != null &&
                                         p.Species.Name.ToLower().Contains(filter.Species.ToLower()));

            var today = DateTime.UtcNow.Date;

            if (filter.MinAge.HasValue && filter.MaxAge.HasValue)
            {
                var maxDob = today.AddYears(-filter.MinAge.Value);
                var minDob = today.AddYears(-filter.MaxAge.Value);

                query = query.Where(p =>
                    p.DateOfBirth <= maxDob &&
                    p.DateOfBirth >= minDob
                );
            }
            else if (filter.MinAge.HasValue)
            {
                var maxDob = today.AddYears(-filter.MinAge.Value);
                query = query.Where(p => p.DateOfBirth <= maxDob);
            }
            else if (filter.MaxAge.HasValue)
            {
                var minDob = today.AddYears(-filter.MaxAge.Value);
                query = query.Where(p => p.DateOfBirth >= minDob);
            }

            if (!string.IsNullOrWhiteSpace(filter.SortType))
            {
                query = filter.SortType switch
                {
                    "NameAsc" => query.OrderBy(p => p.Name),
                    "NameDesc" => query.OrderByDescending(p => p.Name),
                    "SpeciesAsc" => query.OrderBy(p => p.Species.Name),
                    "SpeciesDesc" => query.OrderByDescending(p => p.Species.Name),
                    "VetAsc" => query.OrderBy(p => p.Vet.Name),
                    "VetDesc" => query.OrderByDescending(p => p.Vet.Name),
                    "AgeAsc" => query.OrderBy(p => p.DateOfBirth),
                    "AgeDesc" => query.OrderByDescending(p => p.DateOfBirth),
                    _ => query.OrderBy(p => p.Name)
                };
            }

            return await query.ToListAsync();
        }


        public async Task<List<Patient>> GetAll()
        {
            return await _dbContext.Patients
                .Include(p => p.Species)
                .Include(p => p.Owner)
                .Include(p => p.Vet)
                .ToListAsync();
        }

        public async Task<Patient> GetOne(int Id)
        {
            return await _dbContext.Patients
                .Include(p => p.Species)
                .Include(p => p.Owner)
                .Include(p => p.Vet)
                .FirstOrDefaultAsync(p => p.Id == Id);
        }

        public async Task<List<Patient>> GetByVet(int VetId)
        {
            return await _dbContext.Patients
                .Include(p => p.Species)
                .Include(p => p.Owner)
                .Include(p => p.Vet)
                .Where(p => p.VetId == VetId)
                .ToListAsync();
        }
    }
}
