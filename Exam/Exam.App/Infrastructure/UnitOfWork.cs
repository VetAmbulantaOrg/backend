using Exam.App.Domain;
using Exam.App.Domain.Interface;
using Exam.App.Infrastructure.Database;
using Exam.App.Infrastructure.Database.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Exam.App.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        public IAnimalRepository AnimalRepository { get; set; }
        public IPatientRepository PatientRepository { get; set; }
        public IVetRepository VetRepository { get; set; }
        public IAppointmentRepository AppointmentRepository { get; set; }
        public UnitOfWork(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            AnimalRepository = new AnimalRepository(dbContext);
            PatientRepository = new PatientRepository(dbContext);
            VetRepository = new VetRepository(dbContext);
            AppointmentRepository = new AppointmentRepository(dbContext);

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
