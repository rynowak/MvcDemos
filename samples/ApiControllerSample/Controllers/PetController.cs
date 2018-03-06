
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiControllerSample
{
    [Route("/pet")]
    public class PetController : ControllerBase
    {
        public PetController(BasicApiContext dbContext)
        {
            DbContext = dbContext;
        }

        public BasicApiContext DbContext { get; }

        [HttpGet("{id}", Name = "FindPetById")]
        public async Task<IActionResult> FindById(int id)
        {
            var pet = await DbContext.Pets
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.Tags)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (pet == null)
            {
                return new NotFoundResult();
            }

            return new ObjectResult(pet);
        }

        [HttpGet("findByCategory/{categoryId}")]
        public async Task<IActionResult> FindByCategory(int categoryId)
        {
            var pet = await DbContext.Pets
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.Tags)
                .FirstOrDefaultAsync(p => p.Category != null && p.Category.Id == categoryId);
            if (pet == null)
            {
                return new NotFoundResult();
            }

            return new JsonResult(pet);
        }

        [HttpGet("findByStatus")]
        public async Task<IActionResult> FindByStatus(string status)
        {
            var pet = await DbContext.Pets
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.Tags)
                .FirstOrDefaultAsync(p => p.Status == status);
            if (pet == null)
            {
                return new NotFoundResult();
            }

            return new ObjectResult(pet);
        }

        [HttpGet("findByTags")]
        public async Task<IActionResult> FindByTags(string[] tags)
        {
            var pet = await DbContext.Pets
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.Tags)
                .FirstOrDefaultAsync(p => p.Tags.Any(t => tags.Contains(t.Name)));
            if (pet == null)
            {
                return new NotFoundResult();
            }

            return new ObjectResult(pet);
        }
        
        [HttpPost]
        public async Task<IActionResult> AddPet([FromBody] Pet pet)
        {
            DbContext.Pets.Add(pet);
            await DbContext.SaveChangesAsync();

            return new CreatedAtRouteResult("FindPetById", new { id = pet.Id }, pet);
        }

        public ActionResult<Pet> EditPet([FromBody] Pet pet)
        {
            return Ok(pet);
        }
        
        [HttpPost("{id}/uploadImage")]
        public IActionResult UploadImage(int id, IFormFile file)
        {
            throw new NotImplementedException();
        }
        
        [HttpDelete("{id}")]
        public IActionResult DeletePet(int id)
        {
            throw new NotImplementedException();
        }
    }
}