using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AnimeShop.Models;
using AnimeShop.Repositories;
using Microsoft.AspNetCore.Authorization;
using AnimeShop.ViewModels;
using AnimeShop.Data;

namespace AnimeShop.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AnimeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IAnimeRepository _animeRepo;
        private readonly ICategoryRepository _categoryRepo;

        public AnimeController(AppDbContext context, IAnimeRepository animeRepo, ICategoryRepository categoryRepo)
        {
            _context = context;
            _animeRepo = animeRepo;
            _categoryRepo = categoryRepo;
        }

        // GET: Anime
        public async Task<IActionResult> Index()
        {
            return View(await _animeRepo.GetAllIncludedAsync());
        }

        // GET: Anime
        [AllowAnonymous]
        public async Task<IActionResult> ListAll()
        {
            var model = new SearchAnimeViewModel()
            {
                AnimeList = await _animeRepo.GetAllIncludedAsync(),
                SearchText = null
            };

            return View(model);
        }

        private async Task<List<Anime>> GetAnimeearchList(string userInput)
        {
            userInput = userInput.ToLower().Trim();

            var result = _context.Anime.Include(p => p.Category)
                .Where(p => p
                    .Name.ToLower().Contains(userInput))
                    .Select(p => p).OrderBy(p => p.Name);

            return await result.ToListAsync();
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> AjaxSearchList(string searchString)
        {
            var AnimeList = await GetAnimeearchList(searchString);
            
            return PartialView(AnimeList);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ListAll([Bind("SearchText")] SearchAnimeViewModel model)
        {
            var Anime = await _animeRepo.GetAllIncludedAsync();
            if (model.SearchText == null || model.SearchText == string.Empty)
            {
                model.AnimeList = Anime;
                return View(model);
            }

            var input = model.SearchText.Trim();
            if (input == string.Empty || input == null)
            {
                model.AnimeList = Anime;
                return View(model);
            }
            var searchString = input.ToLower();

            if (string.IsNullOrEmpty(searchString))
            {
                model.AnimeList = Anime;
            }
            else
            {
                model.AnimeList = new List<Anime>();
            }
            return View(model);
        }

        // GET: Anime
        [AllowAnonymous]
        public async Task<IActionResult> ListCategory(string categoryName)
        {
            bool categoryExtist = _context.Categories.Any(c => c.Name == categoryName);
            if (!categoryExtist)
            {
                return NotFound();
            }

            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Name == categoryName);

            if (category == null)
            {
                return NotFound();
            }

            bool anyAnime = await _context.Anime.AnyAsync(x => x.Category == category);
            if (!anyAnime)
            {
                return NotFound($"Товары в категории : {categoryName} не обнаружены");
            }

            var Anime = _context.Anime.Where(x => x.Category == category)
                .Include(x => x.Category).Include(x => x.Reviews);

            ViewBag.CurrentCategory = category.Name;
            return View(Anime);
        }

        // GET: Anime/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Anime = await _animeRepo.GetByIdIncludedAsync(id);

            if (Anime == null)
            {
                return NotFound();
            }

            return View(Anime);
        }

        // GET: Anime/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> DisplayDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Anime = await _animeRepo.GetByIdIncludedAsync(id);

            double score;
            if (_context.Reviews.Any(x => x.AnimeId == id))
            {
                var review = _context.Reviews.Where(x => x.AnimeId == id);
                score = review.Average(x => x.Grade);
                score = Math.Round(score, 2);
            }
            else
            {
                score = 0;
            }
            ViewBag.AverageReviewScore = score;

            if (Anime == null)
            {
                return NotFound();
            }

            return View(Anime);
        }

        // GET: Anime
        [AllowAnonymous]
        public async Task<IActionResult> SearchAnime()
        {
            var model = new SearchAnimeViewModel()
            {
                AnimeList = await _animeRepo.GetAllIncludedAsync(),
                SearchText = null
            };

            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SearchAnime([Bind("SearchText")] SearchAnimeViewModel model)
        {
            var Anime = await _animeRepo.GetAllIncludedAsync();
            var search = model.SearchText.ToLower();

            if (string.IsNullOrEmpty(search))
            {
                model.AnimeList = Anime;
            }
            else
            {
                    model.AnimeList = new List<Anime>();
            }
            return View(model);
        }

        // GET: Anime/Create
        public IActionResult Create()
        {
            ViewData["CategoriesId"] = new SelectList(_categoryRepo.GetAll(), "Id", "Name");
            return View();
        }

        // POST: Anime/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Description,ImageUrl,WeeklyProduct,CategoriesId")] Anime Anime)
        {
            if (ModelState.IsValid)
            {
                _animeRepo.Add(Anime);
                await _animeRepo.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["CategoriesId"] = new SelectList(_categoryRepo.GetAll(), "Id", "Name", Anime.CategoriesId);
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
            ViewData["CategoriesId"] = new SelectList(_categoryRepo.GetAll(), "Id", "Name", Anime.CategoriesId);
            return View(Anime);
        }

        // POST: Anime/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Description,ImageUrl,WeeklyProduct,CategoriesId")] Anime Anime)
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
            ViewData["CategoriesId"] = new SelectList(_categoryRepo.GetAll(), "Id", "Name", Anime.CategoriesId);
            return View(Anime);
        }

        // GET: Anime/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Anime = await _animeRepo.GetByIdIncludedAsync(id);

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
