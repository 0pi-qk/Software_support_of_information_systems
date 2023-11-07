using AnimeShop.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AnimeShop.ViewModels
{
    public class SearchAnimeViewModel
    {
        [Required]
        [DisplayName("Serach")]
        public string SearchText { get; set; }

        public IEnumerable<Anime> AnimeList { get; set; }

    }
}
