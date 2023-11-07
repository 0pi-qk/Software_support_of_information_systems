using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AnimeShop.Models
{
    public class Categories
    {
        public Categories()
        {
            Anime = new HashSet<Anime>();
        }

        public int Id { get; set; }

        [StringLength(100, MinimumLength = 2)]
        [RegularExpression("([а-яА-Яa-zA-Z0-9 .&'-]+)", ErrorMessage = "Поле Имя должно содержать только буквы и цифры.")]
        [DataType(DataType.Text)]
        [Required]
        public string Name { get; set; }

        [StringLength(255, MinimumLength = 2)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public virtual ICollection<Anime> Anime { get; set; }

    }
}