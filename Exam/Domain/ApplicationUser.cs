using Microsoft.AspNetCore.Identity;

namespace Exam.App.Domain;

public class ApplicationUser : IdentityUser<int>
{
    public required string Name { get; set; }
    public required string Surname { get; set; }
}