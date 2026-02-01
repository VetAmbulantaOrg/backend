using Exam.App.Domain.Interface;
using Exam.App.Infrastructure.Database;
using Exam.App.Infrastructure.Database.Repositories;

namespace Exam.App.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        public IAnimalRepository AnimalRepository { get; set; }
        public IPatientRepository PatientRepository { get; set; }
        public UnitOfWork(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            AnimalRepository = new AnimalRepository(dbContext);
            PatientRepository = new PatientRepository(dbContext);

        }

        public Task<int> CompleteAsync()
        {
            return _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
