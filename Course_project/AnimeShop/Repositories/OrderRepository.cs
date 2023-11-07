using AnimeShop.Data;
using AnimeShop.Models;
using System;
using System.Threading.Tasks;

namespace AnimeShop.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;
        private readonly ShoppingCart _shoppingCart;


        public OrderRepository(AppDbContext context, ShoppingCart shoppingCart)
        {
            _context = context;
            _shoppingCart = shoppingCart;
        }

        public async Task CreateOrderAsync(Order order)
        {
            order.OrderPlaced = DateTime.Now;
            decimal totalPrice = 0M;

            var shoppingCartItems = _shoppingCart.ShoppingCartItems;

            foreach (var shoppingCartItem in shoppingCartItems)
            {
                var orderDetail = new OrderDetail()
                {
                    Amount = shoppingCartItem.Amount,
                    AnimeId = shoppingCartItem.Anime.Id,
                    Order = order,
                    Price = shoppingCartItem.Anime.Price,
                    
                };
                totalPrice += orderDetail.Price * orderDetail.Amount;
                _context.OrderDetails.Add(orderDetail);
            }

            order.OrderTotal = totalPrice;
            _context.Orders.Add(order);

            await _context.SaveChangesAsync();
        }
    }
}
