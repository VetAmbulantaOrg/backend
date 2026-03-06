using System.Security.Claims;
using Exam.App.Services.Dtos;

namespace Application.Interface;

public interface IAuthService
{
    Task Register(RegistrationDto data);
    Task<string> Login(LoginDto data);
    Task<ProfileDto> GetProfile(ClaimsPrincipal userPrincipal);
}