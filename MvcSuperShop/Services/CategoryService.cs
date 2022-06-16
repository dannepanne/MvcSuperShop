using MvcSuperShop.Data;

namespace MvcSuperShop.Services;

public class CategoryService : ICategoryService
{
    private readonly ApplicationDbContext _context;

    public CategoryService(ApplicationDbContext context)
    {
        _context = context;
    }
    public IEnumerable<Category> GetTrendingCategories(int numberOfCategories)
    {
        if(_context.Categories.Count() >= numberOfCategories)
            return _context.Categories.Take(numberOfCategories);
        return _context.Categories;
    }
}