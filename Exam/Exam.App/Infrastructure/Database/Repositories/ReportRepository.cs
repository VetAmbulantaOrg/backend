using Delivery.Infrastructure.Repositories;
using Exam.App.Domain.Interface;
using Exam.App.Domain.Models;

namespace Exam.App.Infrastructure.Database.Repositories
{
    public class ReportRepository : GenericRepository<Report>, IReportRepository
    {
        private readonly AppDbContext _dbContext;

        public ReportRepository(AppDbContext dbContext) : base(dbContext)
        {

            _dbContext = dbContext;
        }
    }
}
