using AnimeShop.Models;
using System.Threading.Tasks;

namespace AnimeShop.Repositories
{
    public interface IOrderRepository
    {
        Task CreateOrderAsync(Order order);

    }
}
