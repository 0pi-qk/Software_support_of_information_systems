namespace AnimeShop.Models
{
    public class ShoppingCartItem
    {
        public int ShoppingCartItemId { get; set; }
        public Anime Anime { get; set; }
        public int Amount { get; set; }
        public string ShoppingCartId { get; set; }
    }
}
