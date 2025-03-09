using MVC_Project.Contract;
using MVC_Project.IRepository;
using MVC_Project.Models;

namespace MVC_Project.IRrepository
{
    public interface ICategoryRepository 
    {
        Task<dynamic> AddCategory(Category entity);
        Task<dynamic> GetCategoryById(int id);
        Task<CategoryResult> GetCategories(Pagination pagination);
        Task Delete(int id);
        Task update(Category category);
    }
}
