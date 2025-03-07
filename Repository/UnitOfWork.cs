using Microsoft.EntityFrameworkCore;
using MVC_Project.IRepository;
using MVC_Project.IRrepository;

namespace MVC_Project.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private NewDBContext _dbContext;
        public ICategoryRepository CategoryRepository {  get; set; }

        public IProductRepository ProductRepository { get; private set; }

        public UnitOfWork(NewDBContext dbContext)
        {
            _dbContext = dbContext;
            ProductRepository = new ProductRepository(dbContext);
            CategoryRepository = new CategoryRepository(dbContext);
        }
        public async Task<int> SaveAsync()
        {
            var res = await _dbContext.SaveChangesAsync();
            return res;
        }
    }
}
