using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AnimeShop.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeShop.Models
{
    public class ShoppingCart
    {
        private readonly AppDbContext _appDbContext;

        private ShoppingCart(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public string ShoppingCartId { get; set; }

        public List<ShoppingCartItem> ShoppingCartItems { get; set; }


        public static ShoppingCart GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext.Session;

            var context = services.GetService<AppDbContext>();
            string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();

            session.SetString("CartId", cartId);

            return new ShoppingCart(context) { ShoppingCartId = cartId };
        }

        public async Task AddToCartAsync(Anime Anime, int amount)
        {
            var shoppingCartItem =
                    await _appDbContext.ShoppingCartItems.SingleOrDefaultAsync(
                        s => s.Anime.Id == Anime.Id && s.ShoppingCartId == ShoppingCartId);

            if (shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCartItem
                {
                    ShoppingCartId = ShoppingCartId,
                    Anime = Anime,
                    Amount = 1
                };

                _appDbContext.ShoppingCartItems.Add(shoppingCartItem);
            }
            else
            {
                shoppingCartItem.Amount++;
            }

            await _appDbContext.SaveChangesAsync();
        }

        public async Task<int> RemoveFromCartAsync(Anime Anime)
        {
            var shoppingCartItem =
                    await _appDbContext.ShoppingCartItems.SingleOrDefaultAsync(
                        s => s.Anime.Id == Anime.Id && s.ShoppingCartId == ShoppingCartId);

            var localAmount = 0;

            if (shoppingCartItem != null)
            {
                if (shoppingCartItem.Amount > 1)
                {
                    shoppingCartItem.Amount--;
                    localAmount = shoppingCartItem.Amount;
                }
                else
                {
                    _appDbContext.ShoppingCartItems.Remove(shoppingCartItem);
                }
            }

            await _appDbContext.SaveChangesAsync();

            return localAmount;
        }

        public async Task<List<ShoppingCartItem>> GetShoppingCartItemsAsync()
        {
            return ShoppingCartItems ??
                   (ShoppingCartItems = await
                       _appDbContext.ShoppingCartItems.Where(c => c.ShoppingCartId == ShoppingCartId)
                           .Include(s => s.Anime)
                           .ToListAsync());
        }

        public async Task ClearCartAsync()
        {
            var cartItems = _appDbContext
                .ShoppingCartItems
                .Where(cart => cart.ShoppingCartId == ShoppingCartId);

            _appDbContext.ShoppingCartItems.RemoveRange(cartItems);

            await _appDbContext.SaveChangesAsync();
        }

        public decimal GetShoppingCartTotal()
        {
            var total = _appDbContext.ShoppingCartItems.Where(c => c.ShoppingCartId == ShoppingCartId)
                .Select(c => c.Anime.Price * c.Amount).Sum();
            return total;
        }

    }
}
