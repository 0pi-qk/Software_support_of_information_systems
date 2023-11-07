using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AnimeShop.Data;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeShop.Components
{
    public class CarouselMenu : ViewComponent
    {
        private readonly AppDbContext _context;
        public CarouselMenu(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var Anime = await _context.Anime.Where(x => x.WeeklyProduct).ToListAsync();
            return View(Anime);
        }
    }
}
