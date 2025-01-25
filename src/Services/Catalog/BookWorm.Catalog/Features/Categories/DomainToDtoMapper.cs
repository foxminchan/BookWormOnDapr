using BookWorm.Catalog.Domain;

namespace BookWorm.Catalog.Features.Categories;

public static class DomainToDtoMapper
{
    public static CategoryDto ToCategoryDto(this Category category)
    {
        return new(category.Id, category.Name);
    }

    public static IReadOnlyList<CategoryDto> ToCategoryDtos(this List<Category>? categories)
    {
        return categories?.Select(ToCategoryDto).ToList() ?? [];
    }
}
