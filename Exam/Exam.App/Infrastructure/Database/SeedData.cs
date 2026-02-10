using Exam.App.Domain;
using Exam.App.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Exam.App.Infrastructure.Database;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var context = serviceProvider.GetRequiredService<AppDbContext>();

        var admin1 = new Vet
        {
            UserName = "john",
            Email = "john.doe@example.com",
            Name = "John",
            Surname = "Doe",
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        if (await userManager.FindByNameAsync(admin1.UserName) == null)
        {
            await userManager.CreateAsync(admin1, "John123!");
            await userManager.AddToRoleAsync(admin1, "Veterinar");
        }

        var admin2 = new Vet
        {
            UserName = "jane",
            Email = "jane.doe@example.com",
            Name = "Jane",
            Surname = "Doe",
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        if (await userManager.FindByNameAsync(admin2.UserName) == null)
        {
            await userManager.CreateAsync(admin2, "Jane123!");
            await userManager.AddToRoleAsync(admin2, "Veterinar");
        }


        var helper1 = new ApplicationUser
        {
            UserName = "marko",
            Email = "marko.petrovic@example.com",
            Name = "Marko",
            Surname = "Petrović",
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        if (await userManager.FindByNameAsync(helper1.UserName) == null)
        {
            await userManager.CreateAsync(helper1, "Marko123!");
            await userManager.AddToRoleAsync(helper1, "Pomocnik");
        }



        // --- Pacijenti ---
        if (!await context.Patients.AnyAsync())
        {

            context.Patients.AddRange(
                new Patient
                {
                    Name = "Rex",
                    SpeciesId = 1,
                    OwnerId = 3,
                    VetId = 1,
                    DateOfBirth = new DateTime(2015, 5, 12, 0, 0, 0, DateTimeKind.Utc)
                },
                new Patient
                {
                    Name = "Mica",
                    SpeciesId = 2,
                    OwnerId = 3,
                    VetId = 2,
                    DateOfBirth = new DateTime(2021, 8, 3, 0, 0, 0, DateTimeKind.Utc)
                },
                new Patient
                {
                    Name = "Kiki",
                    SpeciesId = 3,
                    OwnerId = 3,
                    VetId = 1,
                    DateOfBirth = new DateTime(2017, 2, 20, 0, 0, 0, DateTimeKind.Utc)
                }
            );

            await context.SaveChangesAsync();
        }

    }
}


