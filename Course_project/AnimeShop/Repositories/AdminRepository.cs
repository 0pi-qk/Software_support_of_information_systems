using AnimeShop.Data;
using System;
using System.Linq;

namespace AnimeShop.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly AppDbContext _context;
        private readonly IServiceProvider _serviceProvider;

        public AdminRepository(AppDbContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
        }

        public void ClearDatabase()
        {
           
            var reviews = _context.Reviews.ToList();
            _context.Reviews.RemoveRange(reviews);

            var shoppingCartItems = _context.ShoppingCartItems.ToList();
            _context.ShoppingCartItems.RemoveRange(shoppingCartItems);

            var users = _context.Users.ToList();
            var userRoles = _context.UserRoles.ToList();

            foreach (var user in users)
            {
                if (!userRoles.Any(r => r.UserId == user.Id))
                {
                    _context.Users.Remove(user);
                }
            }

            var orderDetails = _context.OrderDetails.ToList();
            _context.OrderDetails.RemoveRange(orderDetails);

            var orders = _context.Orders.ToList();
            _context.Orders.RemoveRange(orders);

            var Anime = _context.Anime.ToList();
            _context.Anime.RemoveRange(Anime);

            var categories = _context.Categories.ToList();
            _context.Categories.RemoveRange(categories);

            _context.SaveChanges();
        }

    }
}
