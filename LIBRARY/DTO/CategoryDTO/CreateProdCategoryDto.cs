namespace LIBRARY.Shared.DTO.CategoryDTO
{
    public class CreateProdCategoryDto
    {
        public string Name { get; set; }
        public List<CreateProductDto> Products { get; set; }
    }
}
