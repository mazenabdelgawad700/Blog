using Blog.Domain.IBaseRepository;
using Blog.Infrastructure.Context;
using Blog.Shared.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Blog.Infrastructure.RepositoriesBase
{
    public class BaseRepository<T> : ReturnBaseHandler, IBaseRepository<T> where T : class
    {
        private readonly AppDbContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public BaseRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public async Task<ReturnBase<bool>> AddAsync(T entity)
        {
            try
            {
                if (entity is not null)
                {
                    await _dbSet.AddAsync(entity);
                    await _dbContext.SaveChangesAsync();
                }
                return Success(true, "");
            }
            catch (Exception ex)
            {
                return Failed<bool>(ex.InnerException.Message);
            }
        }
        public async Task<ReturnBase<bool>> AddRangeAsync(ICollection<T> entities)
        {
            try
            {
                if (entities is not null)
                {
                    await _dbSet.AddRangeAsync(entities);
                    await _dbContext.SaveChangesAsync();
                }
                return Success(true, "");
            }
            catch (Exception ex)
            {
                return Failed<bool>(ex.InnerException.Message);
            }
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _dbContext.Database.BeginTransactionAsync();
        }
        public async Task CommitAsync()
        {
            await _dbContext.Database.CommitTransactionAsync();
        }
        public async Task<ReturnBase<bool>> DeleteAsync(int id)
        {
            try
            {
                ReturnBase<T> getEntity = await GetByIdAsync(id);
                if (getEntity.Data is null)
                    return Failed<bool>(getEntity.Message);

                _dbSet.Remove(getEntity.Data);
                await _dbContext.SaveChangesAsync();

                return Success(true, "");
            }
            catch (Exception ex)
            {
                return Failed<bool>(ex.InnerException.Message);
            }
        }
        public async Task<ReturnBase<bool>> DeleteRangeAsync(ICollection<T> entities)
        {
            try
            {
                if (entities is null)
                    return Failed<bool>("Entities is null");

                _dbSet.RemoveRange(entities);
                await _dbContext.SaveChangesAsync();

                return Success(true, "");
            }
            catch (Exception ex)
            {
                return Failed<bool>(ex.InnerException.Message);
            }
        }
        public async Task<ReturnBase<T>> GetByIdAsync(int id)
        {
            try
            {
                if (id > 0)
                {
                    T? entity = await _dbSet.FindAsync(id);
                    if (entity is not null)
                        return Success(entity, "");

                    return Failed<T>("Invalid Id");
                }
                return Failed<T>("Invalid Id");
            }
            catch (Exception ex)
            {
                return Failed<T>(ex.InnerException.Message);
            }
        }
        public ReturnBase<IQueryable<T>> GetTableAsTracking()
        {
            try
            {
                return Success(_dbSet.AsTracking().AsQueryable(), "");
            }
            catch (Exception ex)
            {
                return Failed<IQueryable<T>>(ex.InnerException.Message);
            }
        }
        public ReturnBase<IQueryable<T>> GetTableNoTracking()
        {
            try
            {
                return Success(_dbSet.AsNoTracking().AsQueryable(), "");
            }
            catch (Exception ex)
            {
                return Failed<IQueryable<T>>(ex.InnerException.Message);
            }
        }
        public async Task<ReturnBase<bool>> UpdateAsync(T entity)
        {
            try
            {
                _dbSet.Update(entity);
                await _dbContext.SaveChangesAsync();
                return Success(true, "");
            }
            catch (Exception ex)
            {
                return Failed<bool>(ex.InnerException.Message);
            }
        }
        public async Task<ReturnBase<bool>> UpdateRangeAsync(ICollection<T> entities)
        {
            try
            {
                _dbSet.UpdateRange(entities);
                await _dbContext.SaveChangesAsync();
                return Success(true, "");
            }
            catch (Exception ex)
            {
                return Failed<bool>(ex.InnerException.Message);
            }
        }
        public async Task SaveChangesAsync()
        {
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException.Message);
            }
        }
        public async Task RollbackAsync()
        {
            try
            {
                await _dbContext.Database.RollbackTransactionAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException.Message);
            }
        }
    }
}
