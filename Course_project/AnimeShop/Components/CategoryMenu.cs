using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AnimeShop.Data;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeShop.Components
{
    public class CategoryMenu : ViewComponent
    {
        private readonly AppDbContext _context;
        public CategoryMenu(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _context.Categories.OrderBy(c => c.Name).ToListAsync();
            return View(categories);
        }
    }
}
