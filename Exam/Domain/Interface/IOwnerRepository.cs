using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Exam.App.Domain.Interface;
using Exam.App.Domain.Models;

namespace Domain.Interface
{
    public interface IOwnerRepository : IGenericRepository<Owner>
    {
        Task<List<Owner>> GetByFullName(string fullName);
        Task<Owner> GetOneWithPatient(int id);
    }
}
