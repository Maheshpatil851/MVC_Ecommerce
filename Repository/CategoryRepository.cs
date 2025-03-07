using Microsoft.EntityFrameworkCore;
using MVC_Project.Contract;
using MVC_Project.IRrepository;
using MVC_Project.Models;

namespace MVC_Project.Repository
{
    public class CategoryRepository : RepositoryBase<Category> , ICategoryRepository
    {
        private NewDBContext _dbContext;
        public CategoryRepository(NewDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<dynamic> AddCategory(Category entity)
        {
            _dbContext.Categories.Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
        public async Task<dynamic> GetCategoryById(int id)
        {
            return await _dbContext.Categories.FindAsync(id);
        }

        public async Task<IEnumerable<Category>> GetCategories(Pagination pagination)
        {
            IQueryable<Category> query = _dbContext.Categories;

            if (!string.IsNullOrEmpty(pagination.SortColumn))
            {
                if (pagination.SortDesc)
                {
                    query = query.OrderByDescending(x => EF.Property<object>(x, pagination.SortColumn));
                }
                else
                {
                    query = query.OrderBy(x => EF.Property<object>(x, pagination.SortColumn));
                }
            }

            if (!pagination.SkipPagination)
            {
                query = query.Skip((pagination.PageNumber - 1) * pagination.PageSize)
                             .Take(pagination.PageSize);
            }
            return await query.ToListAsync();
        }


        public async Task Delete(int id)
        {
            var category = await _dbContext.Categories.FindAsync(id);
            if (category != null)
            {
                _dbContext.Categories.Remove(category);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task update(Category category)
        {
            _dbContext.Entry(category).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

    }

}
