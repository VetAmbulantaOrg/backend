using System.ComponentModel.DataAnnotations;
using Exam.App.Domain.Enum;
using Exam.App.Domain.Models;

namespace Exam.App.Services.Dtos.AppointmentDTOs.Request
{
    public class CreateAppointmentDto
    {
        [Required]
        public int VetId { get; set; }

        [Required]
        public int PatientId { get; set; }

        [Required]
        public DateTime StartAt { get; set; } // ISO 8601 datetime, store in UTC

        [Range(1, 1440)]
        public int DurationMinutes { get; set; } = 30;

        // Optional status if client can set it on creation, otherwise set server side
        [Range(0, 2)]
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;
    }

}
