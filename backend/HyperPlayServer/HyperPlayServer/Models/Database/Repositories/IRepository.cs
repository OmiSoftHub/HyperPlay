﻿using System.Linq.Expressions;

namespace HyperPlayServer.Models.Database.Repositories;
public interface IRepository<TEntity, TId> where TEntity : class
{
    Task<ICollection<TEntity>> GetAllAsync();
    IQueryable<TEntity> GetQueryable(bool asNoTracking = true);
    Task<TEntity> GetByIdAsync(object id);
    Task<TEntity> InsertAsync(TEntity entity);
    Task<List<TEntity>> FindAsync(Expression <Func<TEntity, bool>> predicate);
    void Delete(TEntity entity);
    Task<bool> ExistsAsync(object id);
    void Update(TEntity entity);

}
