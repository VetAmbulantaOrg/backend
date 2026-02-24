using System.Security.Claims;
using Exam.App.Services;
using Exam.App.Services.Dtos.AppointmentDTOs.Request;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Veterinar,Pomocnik")]
        [HttpGet("vet/{vetId}/monthly")]
        public async Task<IActionResult> GetMonthlyAppointments(int vetId)
        {
            var grouped = await _appointmentService.GetAppointmentsForCurrentMonthAsync(vetId);
            return Ok(grouped);
        }

        // POST api/appointments
        [Authorize(Roles = "Veterinar,Pomocnik")]
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

        [Authorize(Roles = "Veterinar")]
        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> Cancel([FromBody] CancelAppointmentDto dto)
        {
            var result = await _appointmentService.CancelAppointmentAsync(dto);
            return Ok(result);
        }

    }

}
