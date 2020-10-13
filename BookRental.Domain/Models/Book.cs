using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BookRental.Domain.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        [MaxLength(80)]
        public string Publisher { get; set; }
        [Required]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        [Required]
        [DefaultValue(0)]
        public BookStatus Status { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }
}
