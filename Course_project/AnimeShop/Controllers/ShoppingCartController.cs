using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AnimeShop.Repositories;
using AnimeShop.Models;
using AnimeShop.ViewModels;
using AnimeShop.Data;

namespace AnimeShop.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IAnimeRepository _AnimeRepository;
        private readonly AppDbContext _context;
        private readonly ShoppingCart _shoppingCart;

        public ShoppingCartController(IAnimeRepository AnimeRepository,
            ShoppingCart shoppingCart, AppDbContext context)
        {
            _AnimeRepository = AnimeRepository;
            _shoppingCart = shoppingCart;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _shoppingCart.GetShoppingCartItemsAsync();
            _shoppingCart.ShoppingCartItems = items;

            var shoppingCartViewModel = new ShoppingCartViewModel
            {
                ShoppingCart = _shoppingCart,
                ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal()
            };

            return View(shoppingCartViewModel);
        }

        public async Task<IActionResult> AddToShoppingCart(int AnimeId)
        {
            var selectedAnime = await _AnimeRepository.GetByIdAsync(AnimeId);

            if (selectedAnime != null)
            {
                await _shoppingCart.AddToCartAsync(selectedAnime, 1);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> RemoveFromShoppingCart(int AnimeId)
        {
            var selectedAnime = await _AnimeRepository.GetByIdAsync(AnimeId);

            if (selectedAnime != null)
            {
                await _shoppingCart.RemoveFromCartAsync(selectedAnime);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ClearCart()
        {
            await _shoppingCart.ClearCartAsync();

            return RedirectToAction("Index");
        }

    }
}