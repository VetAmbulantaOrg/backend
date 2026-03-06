using System.Text.Json.Serialization;
using AutoMapper.Configuration.Annotations;

namespace Exam.App.Domain.Models
{
    public class Report
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }

        [JsonIgnore]
        public Appointment Appointment { get; set; }

        public double Weight { get; set; }
        public string Anamnesis { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime LastModifiedAt { get; set; }
    }

}
