using System.ComponentModel.DataAnnotations;

namespace MVC_Project.Models
{
        public class Product
        {
            [Key]
            public int ProductId { get; set; }
            [Required]
            public string Name { get; set; }
            [Required]
            public int? CategoryId { get; set; }
            public string? Description { get; set; }
            [Required]
            [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
            public decimal Price { get; set; }                  
            public decimal? DiscountPrice { get; set; }         
            public int? StockQuantity { get; set; }             
            public string? ImageUrl { get; set; }
            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;            
            public DateTime? ModifiedAt { get; set; }
            public bool IsActive { get; set; }                  
            public virtual Category? Category { get; set; }  
        }
}
