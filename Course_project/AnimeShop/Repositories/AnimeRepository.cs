using Microsoft.EntityFrameworkCore;
using AnimeShop.Data;
using AnimeShop.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeShop.Repositories
{
    public class AnimeRepository : IAnimeRepository
    {
        private readonly AppDbContext _context;

        public AnimeRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Anime> Anime => _context.Anime.Include(p => p.Category).Include(p => p.Reviews); //include here

        public IEnumerable<Anime> AnimeOfTheWeek => _context.Anime.Where(p => p.WeeklyProduct).Include(p => p.Category);

        public void Add(Anime Anime)
        {
            _context.Add(Anime);
        }

        public IEnumerable<Anime> GetAll()
        {
            return _context.Anime.ToList();
        }

        public async Task<IEnumerable<Anime>> GetAllAsync()
        {
            return await _context.Anime.ToListAsync();
        }

        public async Task<IEnumerable<Anime>> GetAllIncludedAsync()
        {
            return await _context.Anime.Include(p => p.Category).Include(p => p.Reviews).ToListAsync();
        }

        public IEnumerable<Anime> GetAllIncluded()
        {
            return _context.Anime.Include(p => p.Category).Include(p => p.Reviews).ToList();
        }

        public Anime GetById(int? id)
        {
            return _context.Anime.FirstOrDefault(p => p.Id == id);
        }

        public async Task<Anime> GetByIdAsync(int? id)
        {
            return await _context.Anime.FirstOrDefaultAsync(p => p.Id == id);
        }

        public Anime GetByIdIncluded(int? id)
        {
            return _context.Anime.Include(p => p.Category).Include(p => p.Reviews).FirstOrDefault(p => p.Id == id);
        }

        public async Task<Anime> GetByIdIncludedAsync(int? id)
        {
            return await _context.Anime.Include(p => p.Category).Include(p => p.Reviews).FirstOrDefaultAsync(p => p.Id == id);
        }

        public bool Exists(int id)
        {
            return _context.Anime.Any(p => p.Id == id);
        }

        public void Remove(Anime Anime)
        {
            _context.Remove(Anime);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Update(Anime Anime)
        {
            _context.Update(Anime);
        }

    }
}
