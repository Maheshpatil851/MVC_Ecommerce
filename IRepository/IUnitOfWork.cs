using MVC_Project.IRrepository;
using MVC_Project.Models;

namespace MVC_Project.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository CategoryRepository { get; }
        IProductRepository ProductRepository { get; }
        Task<int> SaveAsync();
    }
}
