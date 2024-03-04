using ECommerce.Business.Models.Dtos.SubCategories;

namespace ECommerce.Business.Models.Dtos.Categories
{
    public class CategoryListDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<SubCategoryListDto> SubCategories { get; set; }

    }
}
