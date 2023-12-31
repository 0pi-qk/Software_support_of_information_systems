﻿using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AnimeShop.Models
{
    public class Anime
    {
        public Anime()
        {
            Reviews = new HashSet<Reviews>();
        }

        public int Id { get; set; }

        [StringLength(100, MinimumLength = 2)]
        [RegularExpression("([а-яА-Яa-zA-Z0-9 .&'-]+)", ErrorMessage = "Поле Имя должно содержать только буквы и цифры.")]
        [DataType(DataType.Text)]
        [Required]
        public string Name { get; set; }

        [Range(0, 1000)]
        [DataType(DataType.Currency)]
        [Required]
        public decimal Price { get; set; }

        [StringLength(255, MinimumLength = 2)]
        [DataType(DataType.MultilineText)]
        [Required]
        public string Description { get; set; }

        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }

        public bool WeeklyProduct { get; set; }

        [DisplayName("Select Category")]
        public int CategoriesId { get; set; }

        public virtual Categories Category { get; set; }

        public virtual ICollection<Reviews> Reviews { get; set; }

    }
}
