using AnimeShop.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimeShop.Repositories
{
    public interface IAnimeRepository
    {
        IEnumerable<Anime> Anime { get; }
        IEnumerable<Anime> AnimeOfTheWeek { get; }

        Anime GetById(int? id);
        Task<Anime> GetByIdAsync(int? id);

        Anime GetByIdIncluded(int? id);
        Task<Anime> GetByIdIncludedAsync(int? id);

        bool Exists(int id);

        IEnumerable<Anime> GetAll();
        Task<IEnumerable<Anime>> GetAllAsync();

        IEnumerable<Anime> GetAllIncluded();
        Task<IEnumerable<Anime>> GetAllIncludedAsync();

        void Add(Anime Anime);
        void Update(Anime Anime);
        void Remove(Anime Anime);

        void SaveChanges();
        Task SaveChangesAsync();

    }
}
