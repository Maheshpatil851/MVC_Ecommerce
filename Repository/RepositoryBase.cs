using Microsoft.EntityFrameworkCore;
using MVC_Project.Contract;
using MVC_Project.IRepository;
using System.Linq.Expressions;

namespace MVC_Project.Repository
{
    public class RepositoryBase<T> : IBase<T> where T : class
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;

        public RepositoryBase(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T> FindOneAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken)
        {
            return await _dbSet.FirstOrDefaultAsync(filter, cancellationToken);
        }

        public async Task<IEnumerable<T>> FindManyAsync(Expression<Func<T, bool>> filter, Pagination pagination, CancellationToken cancellationToken)
        {
            IQueryable<T> query = _dbSet.AsQueryable();
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (!pagination.SkipPagination)
            {
                query = query.Skip((pagination.PageNumber - 1) * pagination.PageSize)
                             .Take(pagination.PageSize);
            }
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

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<bool> UpdateOneAsync(T entity, CancellationToken cancellationToken)
        {
            _dbSet.Update(entity);
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<bool> UpdateManyAsync(IEnumerable<T> entities, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    _dbSet.UpdateRange(entities);
                    var result = await _context.SaveChangesAsync(cancellationToken) > 0;
                    if (result)
                    {
                        await transaction.CommitAsync(cancellationToken);
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw new Exception("An error occurred while updating entities.", ex);
                }
            }
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken)
        {
            if (filter != null)
            {
                return await _dbSet.CountAsync(filter, cancellationToken);
            }
            return await _dbSet.CountAsync(cancellationToken);
        }

        public async Task CreateOneAsync(T entity, CancellationToken cancellationToken)
        {
            try
            {
                await _dbSet.AddAsync(entity, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating entities.", ex);
            }

        }

        public async Task<IEnumerable<T>> CreateManyAsync(IEnumerable<T> entities, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    await _dbSet.AddRangeAsync(entities, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);
                    return entities;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw new Exception("An error occurred while creating entities.", ex);
                }
            }
        }


    }
}
