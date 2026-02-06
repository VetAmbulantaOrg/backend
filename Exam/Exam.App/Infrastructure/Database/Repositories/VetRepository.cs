using Delivery.Infrastructure.Repositories;
using Exam.App.Domain.Interface;
using Exam.App.Domain.Models;

namespace Exam.App.Infrastructure.Database.Repositories
{
    public class VetRepository : GenericRepository<Vet>, IVetRepository
    {
        private readonly AppDbContext _dbContext;

        public VetRepository(AppDbContext dbContext) : base(dbContext)
        {

            _dbContext = dbContext;
        }
    }
}
