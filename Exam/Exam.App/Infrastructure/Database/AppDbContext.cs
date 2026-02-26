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
    public DbSet<Vet> Vets { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<Report> Reports { get; set; }

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



        // Patient → Species (1:N)
        modelBuilder.Entity<Patient>().HasOne(p => p.Species).WithMany().HasForeignKey(p => p.SpeciesId).OnDelete(DeleteBehavior.SetNull);

        // Patient → Owner (1:N, ApplicationUser)
        modelBuilder.Entity<Patient>().HasOne(p => p.Owner).WithMany().HasForeignKey(p => p.OwnerId).OnDelete(DeleteBehavior.Cascade);

        // Patient → Vet (1:N, Vet nasleđuje ApplicationUser)
        modelBuilder.Entity<Patient>().HasOne(p => p.Vet).WithMany(v => v.Patients).HasForeignKey(p => p.VetId).OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Appointment>().HasOne(a => a.Vet).WithMany(v => v.Appointments).HasForeignKey(a => a.VetId).OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Appointment>().HasOne(a => a.Patient).WithMany(p => p.Appointments).HasForeignKey(a => a.PatientId).OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Report>().HasKey(r => r.Id);

        modelBuilder.Entity<Report>().Property(r => r.Weight).IsRequired();

        modelBuilder.Entity<Report>().Property(r => r.Anamnesis).HasMaxLength(2000).IsRequired();

        modelBuilder.Entity<Report>().Property(r => r.CreatedAt).IsRequired();

        modelBuilder.Entity<Report>().Property(r => r.LastModifiedAt).IsRequired();

        modelBuilder.Entity<Report>().HasOne(r => r.Appointment).WithOne(a => a.Report).HasForeignKey<Report>(r => r.AppointmentId).OnDelete(DeleteBehavior.Cascade);

    }
}
