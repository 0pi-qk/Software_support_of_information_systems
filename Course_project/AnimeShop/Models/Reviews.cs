using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AnimeShop.Models
{
    public class Reviews
    {
        public int Id { get; set; }

        [StringLength(100, MinimumLength = 2)]
        [RegularExpression("([а-яА-Яa-zA-Z0-9 .&'-]+)", ErrorMessage = "Поле Название должно содержать только буквы и цифры.")]
        [DataType(DataType.Text)]
        [Required]
        public string Title { get; set; }

        [StringLength(500, MinimumLength = 2)]
        [DataType(DataType.MultilineText)]
        [Required]
        public string Description { get; set; }

        [Range(1, 5)]
        public int Grade { get; set; }

        public DateTime Date { get; set; }

        [DisplayName("Select Anime")]
        public int AnimeId { get; set; }

        public virtual Anime Anime { get; set; }

        public string UserId { get; set; }

        public IdentityUser User { get; set; }

    }
}