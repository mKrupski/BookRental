﻿using System.ComponentModel.DataAnnotations;

namespace BookRental.Domain.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        [Display(Name = "Category Name")]
        public string Name { get; set; }
    }
}