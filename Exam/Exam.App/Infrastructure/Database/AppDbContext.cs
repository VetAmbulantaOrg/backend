using Exam.App.Domain;
using Exam.App.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Exam.App.Infrastructure.Database;

public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<AnimalSpecies> AnimalSpecies { get; set; }
    public DbSet<Patient> Patients { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed Roles
        modelBuilder.Entity<IdentityRole<int>>().HasData(
            new IdentityRole<int> { Id = 1, Name = "Veterinar", NormalizedName = "VETERINAR" },
            new IdentityRole<int> { Id = 2, Name = "Pomocnik", NormalizedName = "POMOCNIK" },
            new IdentityRole<int> { Id = 3, Name = "User", NormalizedName = "USER" }
        );



        // Seed Buildings
        modelBuilder.Entity<AnimalSpecies>(e =>
        {
            e.HasData(
              new AnimalSpecies { Id = 1, Name = "Pas" },
              new AnimalSpecies { Id = 2, Name = "Mačka" },
              new AnimalSpecies { Id = 3, Name = "Papagaj" },
              new AnimalSpecies { Id = 4, Name = "Kornjača" },
              new AnimalSpecies { Id = 5, Name = "Zec" },
              new AnimalSpecies { Id = 6, Name = "Hrčak" }
            );
        });



    }
}