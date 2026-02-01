using System.Diagnostics.Metrics;
using Delivery.Infrastructure.Repositories;
using Exam.App.Domain.Interface;
using Exam.App.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Exam.App.Infrastructure.Database.Repositories
{
    public class AnimalRepository : GenericRepository<AnimalSpecies>, IAnimalRepository
    {
        private readonly AppDbContext _dbContext;

        public AnimalRepository(AppDbContext dbContext) : base(dbContext)
        {

            _dbContext = dbContext;
        }

    }
}
