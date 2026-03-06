using Application.Dtos.OwnerDTOs.Request;
using Application.Dtos.OwnerDTOs.Response;
using Application.Interface;
using Exam.App.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Exam.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpGet("Vet")]
        public async Task<IActionResult> GetAllVetsAsync()
        {
            var result = await _userService.GetAllVetsAsync();

            return Ok(result);
        }

        [HttpGet("owner")]
        public async Task<IActionResult> GetOwnersByFullName(string fullName)
        {
            var result = await _userService.GetOwnersByFullName(fullName);

            return Ok(result);
        }

        [Authorize(Roles = "Veterinar,Pomocnik")]
        [HttpPost("owner")]
        public async Task<IActionResult> CreateOwner([FromBody] OwnerCreateRequestDTO dto)
        {
            var result = await _userService.CreateOwner(dto);

            return Ok(result);
        }
    }
}
