using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exam.App.Domain.Models;

namespace Application.Dtos.OwnerDTOs.Response
{
    public class OwnerDetailsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Adress { get; set; }
        public int PhoneNumber { get; set; }
        public ICollection<Patient> Pets { get; set; }
    }
}
