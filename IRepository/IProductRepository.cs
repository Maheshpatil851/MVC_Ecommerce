using MVC_Project.Contract;
using MVC_Project.IRepository;
using MVC_Project.Models;

namespace MVC_Project.IRrepository
{
    public interface IProductRepository 
    {
        Task<dynamic> AddProduct(Product product);
        Task<dynamic> GetProductById(int id);
        Task<ProductResult> GetProducts(Pagination pagination);
        Task Delete(int id);
        Task update(Product product);
    }
}
