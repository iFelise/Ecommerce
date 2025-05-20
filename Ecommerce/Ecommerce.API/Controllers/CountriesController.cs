using Ecommerce.API.Data;
using Ecommerce.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.API.Controllers
{
    [ApiController]
    [Route("api/countries")]
    public class CountriesController : ControllerBase
    {
        private readonly DataContext _context;

        public CountriesController(DataContext context)
        {
            _context = context;
        }

        [HttpPut]
        public async Task<ActionResult> Put(Country country)
        {
            _context.Update(country);

            await _context.SaveChangesAsync();

            return Ok(country);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var afectedRows = await _context.Countries.Where(c => c.Id == id).ExecuteDeleteAsync();

            if (afectedRows == 0)
            {
                return NotFound();
            }

            return NoContent();
        }

    

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            return Ok(await _context.Countries.Include(c => c.States).ToListAsync());
        }

        [HttpPost]

        public async Task<ActionResult> Post(Country country)
        {
            try
            {
                _context.Update(country);
                await _context.SaveChangesAsync();
                return Ok(country);
            }

            catch (DbUpdateException update)
            {
                if (update.InnerException.Message.Contains("duplicate")) return BadRequest("Ya hay un registro con el mismo Nombre");

                return BadRequest(update.InnerException.Message);
            }

            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
            ;
        }
    } 
}
