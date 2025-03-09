using MVC_Project.Models;

namespace MVC_Project.Contract
{
    public class CategoryResult
    {
        public List<Category> Categories { get; set; }
        public int TotalCategoriesCount { get; set; }
    }
}
