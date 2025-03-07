using System.ComponentModel.DataAnnotations;

namespace MVC_Project.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
