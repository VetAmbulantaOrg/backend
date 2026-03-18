using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Delivery.Infrastructure.Repositories;
using Domain.Interface;
using Domain.Models;
using Exam.App.Domain.Interface;
using Exam.App.Domain.Models;
using Exam.App.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Repositories
{
    public class OwnerRepository : GenericRepository<Owner>, IOwnerRepository
    {
        private readonly AppDbContext _dbContext;

        public OwnerRepository(AppDbContext dbContext) : base(dbContext)
        {

            _dbContext = dbContext;
        }

        public async Task<List<Owner>> GetByFullName(string fullName)
        {
            var normalized = string.Join(" ", fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries)).ToLower();

            return await _dbContext.Owners
                .Where(o => EF.Functions.Like(
                    (o.Name.Trim() + " " + o.Surname.Trim()).ToLower(),
                    $"%{normalized}%"))
                .ToListAsync();
        }


        public async Task<Owner> GetOneWithPatient(int id)
        {
            return await _dbContext.Owners
                .Include(o => o.Pets)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

    }
}
