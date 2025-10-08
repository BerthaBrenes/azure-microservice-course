using Microsoft.EntityFrameworkCore;

namespace Wpm.Management.Api.DataAccess;

public class ManagementDBContext(DbContextOptions<ManagementDBContext> options): DbContext(options)
{
    public DbSet<Pet> Pets { get; set; }
    public DbSet<Breed> Breeds { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Breed>().HasData(
            new Breed(1, "Labrador"),
            new Breed(2, "German Shepherd"),
            new Breed(3, "Golden Retriever"),
            new Breed(4, "Bulldog"),
            new Breed(5, "Beagle")
        );
        modelBuilder.Entity<Pet>().HasData(
            [
            new Pet { Id = 1, Name = "Buddy", Age = 3, BreedId = 1 },
            new Pet { Id = 2, Name = "Kirara", Age = 2, BreedId = 3 },
            new Pet { Id = 3, Name = "Bob", Age = 4, BreedId = 2 },
            ]
        );
    }
}

public static class ManagementDbContextExtensions
{
    public static void EnsureDbIsCreated(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetService<ManagementDBContext>();
        dbContext!.Database.EnsureCreated();
    }
}
public class Pet
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public int BreedId { get; set; }
    public Breed Breed { get; set; }
}
public record Breed(int Id, string Name);