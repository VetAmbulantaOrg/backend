using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class PatientSearchFilter
    {
        public string? FullNameVet { get; set; }
        public string? PetName { get; set; }
        public string? Species { get; set; }
        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }
        public string? SortType { get; set; }
    }

}
