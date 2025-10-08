using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Wpm.Management.Api.DataAccess;

namespace Wpm.Management.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BreedsController(ManagementDBContext dbContext, ILogger<BreedsController> logger) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllBreeds()
        {
            var breeds = await dbContext.Breeds.ToListAsync();
            return Ok(breeds);
        }

        [HttpGet("{id}", Name = nameof(GetBreedById))]
        public async Task<IActionResult> GetBreedById(int id)
        {
            var breed = await dbContext.Breeds
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync();
            if (breed == null)
            {
                return NotFound();
            }
            return Ok(breed);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBreed(NewBreed breed)
        {
            try
            {
                int count = dbContext.Breeds.Count();
                var newBreed = breed.ToBreed(count);
                await dbContext.Breeds.AddAsync(newBreed);
                await dbContext.SaveChangesAsync();
                return CreatedAtRoute(nameof(GetBreedById), new { id = newBreed.Id }, breed);
            }
            catch(Exception ex)
            {
                logger.LogError(ex.ToString());
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }

        }
    }
}

public record NewBreed(string Name)
{
    public Breed ToBreed(int count)
    {
        return new Breed(count + 1, Name);
    }
}