using AutoMapper;
using BACK_END.Data;
using LIBRARY.Shared.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LIBRARY.Shared.DTO.ProductDTO;

namespace BACK_END.Controllers
{
    [ApiController]
    [Route("api/v1/product")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly CloudinaryService _cloudinary;
        private readonly IMapper _mapper;

        public ProductController(ApplicationDbContext context, CloudinaryService cloudinary, IMapper mapper)
        {
            _context = context;
            _cloudinary = cloudinary;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> getAllProducts()
        {
            try
            {
                var products = await _context.Products
                    .Include(p => p.ProductImages)
                    .ToListAsync();

                var response = _mapper.Map<List<ProductResponseDto>>(products);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest("Error al listar los productos: " + ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> getProductById(int id)
        {
            var product = await _context.Products
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return NotFound();

            var response = _mapper.Map<ProductResponseDto>(product);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> createProduct([FromForm] ProductCreateDto dto)
        {
            try
            {
                var uploadResult = await _cloudinary.UploadImageAsync(dto.ImageFile);

                var product = _mapper.Map<Product>(dto);

                product.ProductImages = new List<ProductImage>
                {
                    new ProductImage
                    {
                        ImageUrl = uploadResult.SecureUrl.ToString(),
                        ImageId = uploadResult.PublicId
                    }
                };

                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                var response = _mapper.Map<ProductResponseDto>(product);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest("Error al crear el producto: " + ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> updateProduct(int id, [FromForm] ProductCreateDto dto)
        {
            try
            {
                var product = await _context.Products
                    .Include(p => p.ProductImages)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (product == null)
                    return NotFound("Producto no encontrado");

                // Actualiza campos del producto
                _mapper.Map(dto, product);

                // Si viene una nueva imagen, reemplaza
                if (dto.ImageFile != null)
                {
                    // Remove all existing images
                    if (product.ProductImages != null && product.ProductImages.Any())
                    {
                        foreach (var image in product.ProductImages.ToList())
                        {
                            await _cloudinary.DeleteImageAsync(image.ImageId);
                            _context.ProductImages.Remove(image);
                        }
                    }

                    var uploadResult = await _cloudinary.UploadImageAsync(dto.ImageFile);

                    // Initialize if null
                    if (product.ProductImages == null)
                    {
                        product.ProductImages = new List<ProductImage>();
                    }

                    // Add new image
                    product.ProductImages.Add(new ProductImage
                    {
                        ImageUrl = uploadResult.SecureUrl.ToString(),
                        ImageId = uploadResult.PublicId
                    });
                }

                await _context.SaveChangesAsync();

                var response = _mapper.Map<ProductResponseDto>(product);
                return Ok(response);
            }
            catch (DbUpdateException dbEx)
            {
                return BadRequest(dbEx.InnerException?.Message ?? dbEx.Message);
            }
            catch (Exception ex)
            {
                return BadRequest("Error al intentar actualizar el producto: " + ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> deleteProductById(int id)
        {
            try
            {
                var product = await _context.Products
                    .Include(p => p.ProductImages)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (product == null)
                    return NotFound();

                if (product.ProductImages != null && product.ProductImages.Any())
                {
                    foreach (var image in product.ProductImages.ToList())
                    {
                        await _cloudinary.DeleteImageAsync(image.ImageId);
                        _context.ProductImages.Remove(image);
                    }
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest("Error al eliminar el producto: " + ex.Message);
            }
        }
    }
}
