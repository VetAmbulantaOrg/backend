using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Exam.App.Domain.Models;

namespace Domain.Models
{
    public class Owner
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Adress { get; set; }
        public string PhoneNumber { get; set; }

        [JsonIgnore]
        public ICollection<Patient> Pets { get; set; }
    }
}
