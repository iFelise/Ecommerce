using AutoMapper;
using BACK_END.Data;
using BACK_END.Mapper;
using LIBRARY.Shared.DTO.CategoryDTO;
using LIBRARY.Shared.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BACK_END.Controllers
{
    [ApiController]
    [Route("api/v1/category")]
    public class CategoryController : ControllerBase // Cambio: usar ControllerBase en lugar de Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CategoryController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories() // Cambio: PascalCase
        {
            try
            {
                var categories = await _context.Categories
                    .Include(c => c.ProdCategories)
                    .ToListAsync();

                var result = _mapper.Map<List<CategoryResponseDto>>(categories);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error al listar las categorías: " + ex.Message });
            }
        }

        [HttpGet("{id:int}")] // Agregado: endpoint faltante para obtener por ID
        public async Task<IActionResult> GetCategoryById(int id)
        {
            try
            {
                var category = await _context.Categories
                    .Include(c => c.ProdCategories)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (category == null)
                {
                    return NotFound();
                }

                var result = _mapper.Map<CategoryResponseDto>(category);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error al obtener la categoría: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var category = _mapper.Map<Category>(dto);
                _context.Add(category);
                await _context.SaveChangesAsync();

                var result = _mapper.Map<CategoryResponseDto>(category); // Cambio: devolver DTO en lugar de entidad
                return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, result);
            }
            catch (DbUpdateException dbEx)
            {
                if (dbEx.InnerException?.Message.Contains("duplicate") == true)
                    return BadRequest(new { message = "Ya hay un registro con el mismo Nombre" });
                return BadRequest(new { message = dbEx.InnerException?.Message ?? dbEx.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error al intentar crear la categoría: " + ex.Message });
            }
        }

        [HttpPut("{id:int}")] // Cambio: agregar {id} en la ruta
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryDto dto)
        {
            try
            {
                if (id != dto.Id) // Validación adicional
                {
                    return BadRequest(new { message = "El ID de la URL no coincide con el ID del objeto" });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var category = await _context.Categories.FindAsync(dto.Id);
                if (category == null)
                {
                    return NotFound();
                }

                _mapper.Map(dto, category);
                _context.Update(category);
                await _context.SaveChangesAsync();

                return Ok(_mapper.Map<CategoryResponseDto>(category));
            }
            catch (DbUpdateException dbEx)
            {
                if (dbEx.InnerException?.Message.Contains("duplicate") == true)
                    return BadRequest(new { message = "Ya hay un registro con el mismo Nombre" });
                return BadRequest(new { message = dbEx.InnerException?.Message ?? dbEx.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error al intentar actualizar la categoría: " + ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCategoryById(int id) // Cambio: PascalCase
        {
            try
            {
                var category = await _context.Categories.FindAsync(id);
                if (category == null)
                {
                    return NotFound();
                }

                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error al eliminar la categoría: " + ex.Message });
            }
        }

        // Endpoints adicionales que el servicio espera
        [HttpGet("{categoryId:int}/prodcategories")]
        public async Task<IActionResult> GetProdCategoriesByCategoryId(int categoryId)
        {
            try
            {
                var category = await _context.Categories
                    .Include(c => c.ProdCategories)
                    .FirstOrDefaultAsync(c => c.Id == categoryId);

                if (category == null)
                {
                    return NotFound();
                }

                var result = _mapper.Map<List<ProdCategoryResponseDto>>(category.ProdCategories);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error al obtener las subcategorías: " + ex.Message });
            }
        }
    }
}