
using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiControllerSample
{
    [Route("/pet")]
    public class PetController : ControllerBase
    {
        public PetController(BasicApiContext dbContext, IMapper mapper)
        {
            DbContext = dbContext;
            Mapper = mapper;
        }

        public BasicApiContext DbContext { get; }

        public IMapper Mapper { get; }

        [HttpGet("{id}", Name = "FindPetById")]
        public async Task<ActionResult<Pet>> FindById(int id)
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

            return pet;
        }

        [HttpGet("findByCategory/{categoryId}")]
        public async Task<ActionResult<Pet>> FindByCategory(int categoryId)
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

            return pet;
        }

        [HttpGet("findByStatus")]
        public async Task<ActionResult<Pet>> FindByStatus(string status)
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

            return pet;
        }

        [HttpGet("findByTags")]
        public async Task<ActionResult<Pet>> FindByTags(string[] tags)
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

            return pet;
        }

        [HttpPost]
        public async Task<ActionResult<Pet>> AddPet([FromBody] AddPetDto dto)
        {
            var pet = Mapper.Map<Pet>(dto);
            DbContext.Pets.Add(pet);

            try
            {
                await DbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (PetExists(pet.Id))
            {
                return Conflict();
            }

            return new CreatedAtRouteResult("FindPetById", new { id = pet.Id }, pet);
        }

        [HttpPut]
        public async Task<ActionResult<Pet>> EditPet(int id, [FromBody] EditPetDto dto)
        {
            var pet = await DbContext.Pets.FindAsync(id);
            if (pet == null)
            {
                return NotFound();
            }

            Mapper.Map(dto, pet);
            DbContext.Entry(pet).State = EntityState.Modified;

            try
            {
                await DbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (PetExists(pet.Id))
            {
                return NotFound();
            }

            return NoContent();
        }
        
        [HttpPost("{id}/uploadImage")]
        public IActionResult UploadImage(int id, IFormFile file)
        {
            throw new NotImplementedException();
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePet(int id)
        {
            var pet = await DbContext.Pets.FindAsync(id);
            if (pet == null)
            {
                return new NotFoundResult();
            }

            DbContext.Pets.Remove(pet);
            await DbContext.SaveChangesAsync();

            return NoContent();
        }

        private bool PetExists(int id)
        {
            return DbContext.Pets.Any(e => e.Id == id);
        }
    }
}