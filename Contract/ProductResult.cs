using MVC_Project.Models;

namespace MVC_Project.Contract
{
    public class ProductResult
    {
        public List<Product> Products { get; set; }
        public int TotalProductsCount { get; set; }
    }
}
