using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wpm.Management.Api.DataAccess;

namespace Wpm.Management.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PetsController(ManagementDBContext dbContext, ILogger<PetsController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllPets()
    {
        var pets = await dbContext.Pets.Include(p => p.Breed).ToListAsync();
        return Ok(pets);
    }

    [HttpGet("{id}", Name = nameof(GetPetById))]
    public async Task<IActionResult> GetPetById(int id)
    {
        var pet = await dbContext.Pets.Include(p => p.Breed)
            .Where(p => p.Id == id)
            .FirstOrDefaultAsync();
        if (pet == null)
        {
            logger.LogError("Pet not found");
            return NotFound();
        }
        return Ok(pet);
    }
    [HttpPost]

    public async Task<IActionResult> CreatePet(NewPet pet)
    {
        var newPet = pet.ToPet();
        await dbContext.Pets.AddAsync(newPet);
        await dbContext.SaveChangesAsync();
        return CreatedAtRoute(nameof(GetPetById), new { id = newPet.Id }, pet);
    }
}
public record NewPet(string Name, int Age, int BreedId)
{
    public Pet ToPet()
    {
        return new Pet
        {
            Name = Name,
            Age = Age,
            BreedId = BreedId
        };
    }
}