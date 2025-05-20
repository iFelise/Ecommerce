using Ecommerce.API.Data;
using Ecommerce.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.API.Controllers
{
    [ApiController]
    [Route("api/states")]
    public class StateController : ControllerBase
    {
        private readonly DataContext _context;

        public StateController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _context.States.Include(x=> x.Cities).ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(State state)
        {
            try
            {
                _context.Update(state);
                await _context.SaveChangesAsync();
                return Ok(state);
            }

            catch (DbUpdateException update )
            {
                if (update.InnerException.Message.Contains("duplicate"))return BadRequest("Ya hay un registro con el mismo Nombre");

                return BadRequest(update.InnerException.Message);
            }

            catch (Exception ex){

                return BadRequest(ex.Message);
            }

        }

        [HttpPut]
        public async Task<IActionResult> PutAsync(State state)
        {
            try
            {
                _context.Update(state);
                await _context.SaveChangesAsync();
                return Ok(state);
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
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var afectedRows = await _context.States.Where(c => c.Id == id).ExecuteDeleteAsync();

            if (afectedRows == 0)
            {
                return NotFound();
            }

            return NoContent();
        }


    }
}
