using Delivery.Infrastructure.Repositories;
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

        public async Task<List<Patient>> SearchPatientDetailsAsync(PatientSearchDto search)
        {
            var query = _dbContext.Patients
                .Include(p => p.Species)
                .Include(p => p.Owner)
                .Include(p => p.Vet)
                .AsQueryable();


            if (!string.IsNullOrWhiteSpace(search.FullNameVet))
            {
                var fullNameVet = search.FullNameVet.ToLower();

                query = query.Where(p =>
                    (p.Vet.Name + " " + p.Vet.Surname).ToLower().Contains(fullNameVet));
            }



            if (!string.IsNullOrWhiteSpace(search.PetName))
                query = query.Where(p => p.Name != null &&
                                         p.Name.ToLower().Contains(search.PetName.ToLower()));


            if (!string.IsNullOrWhiteSpace(search.Species))
                query = query.Where(p => p.Species != null &&
                                         p.Species.Name.ToLower().Contains(search.Species.ToLower()));


            var today = DateTime.UtcNow.Date;


            if (search.MinAge.HasValue && search.MaxAge.HasValue)
            {
                var maxDob = today.AddYears(-search.MinAge.Value); // najstariji dozvoljeni datum rođenja
                var minDob = today.AddYears(-search.MaxAge.Value); // najmlađi dozvoljeni datum rođenja

                query = query.Where(p =>
                    p.DateOfBirth <= maxDob && // pacijent mora biti stariji od MinAge
                    p.DateOfBirth >= minDob    // pacijent mora biti mlađi od MaxAge
                );
            }
            else if (search.MinAge.HasValue) // samo od
            {
                var maxDob = today.AddYears(-search.MinAge.Value);
                query = query.Where(p => p.DateOfBirth <= maxDob);
            }
            else if (search.MaxAge.HasValue) // samo do
            {
                var minDob = today.AddYears(-search.MaxAge.Value);
                query = query.Where(p => p.DateOfBirth >= minDob);
            }




            if (!string.IsNullOrWhiteSpace(search.SortType))
            {
                query = search.SortType switch
                {
                    "NameAsc" => query.OrderBy(p => p.Name),
                    "NameDesc" => query.OrderByDescending(p => p.Name),
                    "SpeciesAsc" => query.OrderBy(p => p.Species),
                    "SpeciesDesc" => query.OrderByDescending(p => p.Species),
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
