using Microsoft.EntityFrameworkCore;
using MVC_Project.Contract;
using MVC_Project.IRrepository;
using MVC_Project.Models;
using System.Linq.Expressions;

namespace MVC_Project.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly NewDBContext _dbContext;
        public ProductRepository(NewDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<dynamic> AddProduct(Product product)
        {
            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();
            return product;
        }
       
        public async Task<dynamic> GetProductById(int id)
        {
            return await _dbContext.Products.FindAsync(id);
        }

        public async Task<ProductResult> GetProducts(Pagination pagination)
        {
            IQueryable<Product> query = _dbContext.Products;
            //if (!string.IsNullOrEmpty(pagination.SortColumn))
            //{
            //    var param = Expression.Parameter(typeof(Product), "p");
            //    var property = Expression.Property(param, pagination.SortColumn);
            //    var lambda = Expression.Lambda<Func<Product, object>>(Expression.Convert(property, typeof(object)), param);

            //    query = pagination.SortDesc ? query.OrderByDescending(lambda) : query.OrderBy(lambda);
            //}

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
            var products = await query.ToListAsync();
            var totalProductsCount =  _dbContext.Products.Count();
            return new ProductResult { Products = products, TotalProductsCount = totalProductsCount };
        }


        public async Task Delete(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);
            if (product != null)
            {
                _dbContext.Products.Remove(product);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task update(Product product)
        {
            _dbContext.Entry(product).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

    }
}
