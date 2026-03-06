using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exam.App.Domain.Models;

namespace Application.Dtos.OwnerDTOs.Request
{
    public class OwnerCreateRequestDTO
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Adress { get; set; }
        public string PhoneNumber { get; set; }
    }
}
