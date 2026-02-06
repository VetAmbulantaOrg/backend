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


            if (search.MinAge.HasValue && search.MaxAge.HasValue)
            {
                query = query.Where(p =>
                    CalculateAge(p.DateOfBirth) >= search.MinAge.Value &&
                    CalculateAge(p.DateOfBirth) <= search.MaxAge.Value);
            }
            else if (search.MinAge.HasValue) // samo od
            {
                query = query.Where(p => CalculateAge(p.DateOfBirth) >= search.MinAge.Value);
            }
            else if (search.MaxAge.HasValue) // samo do
            {
                query = query.Where(p => CalculateAge(p.DateOfBirth) <= search.MaxAge.Value);
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


        private static int CalculateAge(DateTime dob)
        {
            var today = DateTime.Today;
            var age = today.Year - dob.Year;
            if (dob.Date > today.AddYears(-age)) age--;
            return age;
        }


    }
}
