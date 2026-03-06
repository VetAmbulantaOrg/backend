using System.Reflection.Emit;
using Domain.Models;
using Exam.App.Domain;
using Exam.App.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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

        var owner1 = new Owner
        {
            Id = 1,
            Name = "Marko",
            Surname = "Petrović",
            Adress = "Beogradska 12, Beograd",
            PhoneNumber = "0612345678"
        };
        var owner2 = new Owner
        {
            Id = 2,
            Name = "Jelena",
            Surname = "Jovanović",
            Adress = "Novosadska 45, Novi Sad",
            PhoneNumber = "0629876543"
        };
        var owner3 = new Owner
        {
            Id = 3,
            Name = "Milan",
            Surname = "Nikolić",
            Adress = "Zmaj Jovina 7, Niš",
            PhoneNumber = "063555444"
        };

        if (!await context.Owners.AnyAsync())
        {
            context.Owners.AddRange(owner1, owner2, owner3);
            await context.SaveChangesAsync();
        }



        // --- Pacijenti ---
        if (!await context.Patients.AnyAsync())
        {

            context.Patients.AddRange(
                new Patient
                {
                    Name = "Rex",
                    SpeciesId = 1,
                    OwnerId = 1,
                    VetId = 1,
                    DateOfBirth = new DateTime(2015, 5, 12, 0, 0, 0, DateTimeKind.Utc)
                },
                new Patient
                {
                    Name = "Mica",
                    SpeciesId = 2,
                    OwnerId = 2,
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


