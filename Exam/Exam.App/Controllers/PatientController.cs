using Exam.App.Services;
using Exam.App.Services.Dtos.CageDTOs.Request;
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOneByIdAsync(int id)
        {
            var result = await _patientService.GetOneById(id);

            return Ok(result);
        }

        [Authorize(Roles = "Veterinar")]
        [HttpPost]

        public async Task<IActionResult> AddCage(PatientCreateRequestDto patientDto)
        {
            var result = await _patientService.AddAsync(patientDto);
            return Ok(result);
        }

        [Authorize(Roles = "Veterinar")]
        [HttpPut]
        public async Task<IActionResult> UpdateAnimal(PatientUpdateRequestDto patientDto)
        {
            var result = await _patientService.UpdateAsync(patientDto);
            return Ok(result);
        }

        [Authorize(Roles = "Veterinar")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnimal(int id)
        {
            await _patientService.DeleteAsync(id);
            return NoContent();
        }
    }
}
