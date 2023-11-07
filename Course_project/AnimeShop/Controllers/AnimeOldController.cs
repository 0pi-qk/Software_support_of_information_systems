using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AnimeShop.Data;
using AnimeShop.Models;
using AnimeShop.Repositories;

namespace AnimeShop.Controllers
{
    public class AnimeOldController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IAnimeRepository _animeRepo;

        public AnimeOldController(AppDbContext context, IAnimeRepository animeRepo)
        {
            _context = context;
            _animeRepo = animeRepo;
        }

        // GET: Anime
        public async Task<IActionResult> Index()
        {
            return View(await _animeRepo.GetAllAsync());
        }

        // GET: Anime/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Anime = await _animeRepo.GetByIdAsync(id);

            if (Anime == null)
            {
                return NotFound();
            }

            return View(Anime);
        }

        // GET: Anime/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Anime/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Description,ImageUrl,WeeklyProduct")] Anime Anime)
        {
            if (ModelState.IsValid)
            {
                _animeRepo.Add(Anime);
                await _animeRepo.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(Anime);
        }

        // GET: Anime/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Anime = await _animeRepo.GetByIdAsync(id);
            if (Anime == null)
            {
                return NotFound();
            }
            return View(Anime);
        }

        // POST: Anime/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Description,ImageUrl,WeeklyProduct")] Anime Anime)
        {
            if (id != Anime.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _animeRepo.Update(Anime);
                    await _animeRepo.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnimeExists(Anime.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(Anime);
        }

        // GET: Anime/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Anime = await _animeRepo.GetByIdAsync(id);

            if (Anime == null)
            {
                return NotFound();
            }

            return View(Anime);
        }

        // POST: Anime/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Anime = await _animeRepo.GetByIdAsync(id);
            _animeRepo.Remove(Anime);
            await _animeRepo.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool AnimeExists(int id)
        {
            return _animeRepo.Exists(id);
        }
    }
}
