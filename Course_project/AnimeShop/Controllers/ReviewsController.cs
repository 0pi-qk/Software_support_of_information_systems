using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AnimeShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AnimeShop.Data;

namespace AnimeShop.Controllers
{
    [Authorize]
    public class ReviewsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ReviewsController(AppDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminIndex()
        {
            var reviews = await _context.Reviews.Include(r =>r.Anime).Include(r => r.User).ToListAsync();
            return View(reviews);
        }

        // GET: Reviews
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            bool isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

            if (isAdmin)
            {
                var allReviews = _context.Reviews.Include(r =>r.Anime).Include(r => r.User).ToList();
                return View(allReviews);
            }
            else
            {
                var reviews = _context.Reviews.Include(r =>r.Anime).Include(r => r.User)
                    .Where(r => r.User == user).ToList();
                return View(reviews);
            }
        }

        // GET: Reviews
        [AllowAnonymous]
        public async Task<IActionResult> ListAll()
        {
            var reviews = await _context.Reviews.Include(r =>r.Anime).Include(r => r.User).ToListAsync();
            return View(reviews);
        }

        private async Task<List<Reviews>> SortReviews(string sortBy, bool isDescending)
        {
            var reviewsList = _context.Reviews.Include(r =>r.Anime).Include(r => r.User);
            IQueryable<Reviews> result;

            if (sortBy == null || sortBy == "")
            {
                result = reviewsList;
            }

            if (isDescending == false)
            {
                switch (sortBy.ToLower())
                {
                    case "date":
                        result = reviewsList.OrderBy(x => x.Date);
                        break;
                    case "grade":
                        result = reviewsList.OrderBy(x => x.Grade);
                        break;
                    case "title":
                        result = reviewsList.OrderBy(x => x.Title);
                        break;
                    case "anime name":
                        result = reviewsList.OrderBy(x => x.Anime.Name);
                        break;
                    default:
                        result = reviewsList.OrderBy(x => x.Anime.Id);
                        break;
                }
            }
            else
            {
                switch (sortBy.ToLower())
                {
                    case "date":
                        result = reviewsList.OrderByDescending(x => x.Date);
                        break;
                    case "grade":
                        result = reviewsList.OrderByDescending(x => x.Grade);
                        break;
                    case "title":
                        result = reviewsList.OrderByDescending(x => x.Title);
                        break;
                    case "anime name":
                        result = reviewsList.OrderByDescending(x => x.Anime.Name);
                        break;
                    default:
                        result = reviewsList.OrderByDescending(x => x.Anime.Id);
                        break;
                }
            }

            //Partial view?
            return await result.ToListAsync();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> AjaxListReviews(string sortBy, bool isDescending)
        {
            var listOfReviews = await SortReviews(sortBy, isDescending);

            return PartialView(listOfReviews);
        }

        // GET: Reviews
        [AllowAnonymous]
        public async Task<IActionResult> AnimeReviews(int? AnimeId)
        {
            if (AnimeId == null)
            {
                return NotFound();
            }
            var anime = _context.Anime.FirstOrDefault(x => x.Id == AnimeId);
            if (anime == null)
            {
                return NotFound();
            }
            var reviews = await _context.Reviews.Include(r =>r.Anime).Include(r => r.User).Where(x => x.Anime.Id == anime.Id).ToListAsync();
            if (reviews == null)
            {
                return NotFound();
            }
            ViewBag.AnimeName = anime.Name;
            ViewBag.AnimeId = anime.Id;

            return View(reviews);
        }

        // GET: Reviews/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reviews = await _context.Reviews
                .Include(r =>r.Anime)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (reviews == null)
            {
                return NotFound();
            }

            return View(reviews);
        }

        // GET: Reviews/Create
        public IActionResult CreateWithAnime(int? AnimeId)
        {
            var review = new Reviews();

            if (AnimeId == null)
            {
                return NotFound();
            }

            var anime = _context.Anime.FirstOrDefault(p => p.Id == AnimeId);
            
            if (anime == null)
            {
                return NotFound();
            }

            review.Anime = anime;
            review.AnimeId = anime.Id;
            ViewData["AnimeId"] = new SelectList(_context.Anime.Where(p => p.Id == AnimeId), "Id", "Name");
            var listOfNumbers = new List<int>() { 1, 2, 3, 4, 5 };
            var listOfGrades = listOfNumbers.Select(x => new { Id = x, Value = x.ToString() });
            ViewData["Grade"] = new SelectList(listOfGrades, "Id", "Value");

            return View(review);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateWithAnime(int AnimeId, Reviews reviews)
        {
            if (AnimeId != reviews.AnimeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(HttpContext.User);
                reviews.UserId = userId;
                reviews.Date = DateTime.Now;

                _context.Add(reviews);
                await _context.SaveChangesAsync();
                return Redirect($"AnimeReviews?AnimeId={AnimeId}");
            }
            var listOfNumbers = new List<int>() { 1, 2, 3, 4, 5 };
            var listOfGrades = listOfNumbers.Select(x => new { Id = x, Value = x.ToString() });
            ViewData["Grade"] = new SelectList(listOfGrades, "Id", "Value", reviews.Grade);
            ViewData["AnimeId"] = new SelectList(_context.Anime.Where(p => p.Id == AnimeId), "Id", "Name", reviews.AnimeId);
            return View(reviews);
        }

        // GET: Reviews/Create
        public IActionResult Create()
        {
            var listOfNumbers = new List<int>() { 1, 2, 3, 4, 5 };
            var listOfGrades = listOfNumbers.Select(x => new { Id = x, Value = x.ToString() });
            ViewData["Grade"] = new SelectList(listOfGrades, "Id", "Value");
            ViewData["AnimeId"] = new SelectList(_context.Anime, "Id", "Name");
            return View();
        }

        // POST: Reviews/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Grade,AnimeId")] Reviews reviews)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(HttpContext.User);
                reviews.UserId = userId;

                reviews.Date = DateTime.Now;
                _context.Add(reviews);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            var listOfNumbers = new List<int>() { 1, 2, 3, 4, 5 };
            var listOfGrades = listOfNumbers.Select(x => new { Id = x, Value = x.ToString() });
            ViewData["Grade"] = new SelectList(listOfGrades, "Id", "Value", reviews.Grade);
            ViewData["AnimeId"] = new SelectList(_context.Anime, "Id", "Name", reviews.AnimeId);
            
            return View(reviews);
        }

        // GET: Reviews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reviews = await _context.Reviews.SingleOrDefaultAsync(m => m.Id == id);
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var userRoles = await _userManager.GetRolesAsync(user);
            bool isAdmin = userRoles.Any(r => r == "Admin");

            if (reviews == null)
            {
                return NotFound();
            }

            if (isAdmin == false)
            {
                var userId = _userManager.GetUserId(HttpContext.User);
                if (reviews.UserId != userId)
                {
                    return BadRequest("У вас нет прав для редактирования этого обзора.");
                }
            }
            var listOfNumbers = new List<int>() { 1, 2, 3, 4, 5 };
            var listOfGrades = listOfNumbers.Select(x => new { Id = x, Value = x.ToString() });
            ViewData["Grade"] = new SelectList(listOfGrades, "Id", "Value", reviews.Grade);
            ViewData["AnimeId"] = new SelectList(_context.Anime, "Id", "Name", reviews.AnimeId);
            return View(reviews);
        }

        // POST: Reviews/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Grade,Date,AnimeId")] Reviews reviews)
        {
            if (id != reviews.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(HttpContext.User);
                try
                {
                    if (reviews.Date == null)
                    {
                        reviews.Date = DateTime.Now;
                    }
                    reviews.UserId = userId;

                    _context.Update(reviews);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewsExists(reviews.Id))
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
            var listOfNumbers = new List<int>() { 1, 2, 3, 4, 5 };
            var listOfGrades = listOfNumbers.Select(x => new { Id = x, Value = x.ToString() });
            ViewData["Grade"] = new SelectList(listOfGrades, "Id", "Value", reviews.Grade);
            ViewData["AnimeId"] = new SelectList(_context.Anime, "Id", "Name", reviews.AnimeId);
            return View(reviews);
        }

        // GET: Reviews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reviews = await _context.Reviews
                .Include(r =>r.Anime)
                .SingleOrDefaultAsync(m => m.Id == id);
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var userRoles = await _userManager.GetRolesAsync(user);
            bool isAdmin = userRoles.Any(r => r == "Admin");

            if (reviews == null)
            {
                return NotFound();
            }

            if (isAdmin == false)
            {
                var userId = _userManager.GetUserId(HttpContext.User);
                if (reviews.UserId != userId)
                {
                    return BadRequest("У вас нет прав для редактирования этого обзора.");
                }
            }

            return View(reviews);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reviews = await _context.Reviews.SingleOrDefaultAsync(m => m.Id == id);
            _context.Reviews.Remove(reviews);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ReviewsExists(int id)
        {
            return _context.Reviews.Any(e => e.Id == id);
        }

    }
}
