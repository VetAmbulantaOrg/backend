using Application.Interface;
using Exam.App.Services;
using Exam.App.Services.Dtos.CageDTOs.Request;
using Exam.App.Services.Dtos.PatientDTOs.Request;
using Exam.App.Services.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Exam.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : Controller
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService cageService)
        {
            _patientService = cageService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _patientService.GetAllAsync();

            return Ok(result);
        }

        [HttpGet("getByVet")]
        public async Task<IActionResult> GetByVetAsync(int vetId)
        {
            var result = await _patientService.GetByVet(vetId);

            return Ok(result);
        }

        [HttpPost("search")]
        public async Task<IActionResult> GetBySearch([FromBody] PatientSearchDto searchDto , int page = 1, int pageSize = 10)
        {
            var result = await _patientService.SearchPatientDetailsAsync(searchDto, page, pageSize);
            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetOneByIdAsync(int id)
        {
            var result = await _patientService.GetOneById(id);

            return Ok(result);
        }

        [Authorize(Roles = "Veterinar,Pomocnik")]
        [HttpPost]

        public async Task<IActionResult> AddPatient(PatientCreateRequestDto patientDto)
        {
            var result = await _patientService.AddAsync(patientDto);
            return Ok(result);
        }

        [Authorize(Roles = "Veterinar,Pomocnik")]
        [HttpPut]
        public async Task<IActionResult> UpdatePatient(PatientUpdateRequestDto patientDto)
        {
            try
            {
                var result = await _patientService.UpdateAsync(patientDto);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message); // 🚨 ovde vraćaš poruku
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Došlo je do greške prilikom izmene pacijenta.");
            }
        }


        [Authorize(Roles = "Veterinar,Pomocnik")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnimal(int id)
        {
            await _patientService.DeleteAsync(id);
            return NoContent();
        }
    }
}
