using Exam.App.Services;
using Exam.App.Services.Dtos.AppointmentDTOs.Request;
using Microsoft.AspNetCore.Mvc;

namespace Exam.App.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentsController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        // GET api/appointments/vet/{vetId}/monthly
        [HttpGet("vet/{vetId}/monthly")]
        public async Task<IActionResult> GetMonthlyAppointments(int vetId)
        {
            var grouped = await _appointmentService.GetAppointmentsForCurrentMonthAsync(vetId);
            return Ok(grouped);
        }

        // POST api/appointments
        [HttpPost]
        public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var created = await _appointmentService.CreateAppointmentAsync(dto);
                return Ok(created);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
    }

}
