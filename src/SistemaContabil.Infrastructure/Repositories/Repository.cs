using Microsoft.EntityFrameworkCore;
using SistemaContabil.Domain.Interfaces;
using SistemaContabil.Infrastructure.Data;
using System.Linq.Expressions;

namespace SistemaContabil.Infrastructure.Repositories;

/// Implementação genérica do repositório
/// <typeparam name="T">Tipo da entidade</typeparam>
public class Repository<T> : IRepository<T> where T : class
{
    protected readonly SistemaContabilDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(SistemaContabilDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public virtual async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.FirstOrDefaultAsync(predicate);
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Obtém o próximo valor de uma sequência Oracle
    /// </summary>
    protected async Task<int> GetNextSequenceValueAsync(string sequenceName)
    {
        var sql = $"SELECT {sequenceName}.NEXTVAL FROM DUAL";
        var result = await _context.Database.SqlQueryRaw<int>(sql).ToListAsync();
        return result.FirstOrDefault();
    }

    /// <summary>
    /// Obtém o próximo valor de uma sequência Oracle (long)
    /// </summary>
    protected async Task<long> GetNextSequenceValueLongAsync(string sequenceName)
    {
        var sql = $"SELECT {sequenceName}.NEXTVAL FROM DUAL";
        var result = await _context.Database.SqlQueryRaw<long>(sql).ToListAsync();
        return result.FirstOrDefault();
    }

    public virtual async Task<T> UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task<bool> RemoveAsync(T entity)
    {
        _dbSet.Remove(entity);
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }

    public virtual async Task<bool> RemoveByIdAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity == null) return false;
        return await RemoveAsync(entity);
    }

    public virtual async Task<bool> ExistsAsync(int id)
    {
        return await _dbSet.FindAsync(id) != null;
    }

    public virtual async Task<int> CountAsync()
    {
        return await _dbSet.CountAsync();
    }

    public virtual async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.CountAsync(predicate);
    }
}
