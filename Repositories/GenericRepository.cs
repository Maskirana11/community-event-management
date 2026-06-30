using CommunityEvents.Data;
using CommunityEvents.Interfaces;
using CommunityEvents.Models;
using Microsoft.EntityFrameworkCore;

namespace CommunityEvents.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    protected readonly ApplicationDbContext _context;
    internal DbSet<T> dbSet;

    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
        this.dbSet = _context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await dbSet.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await dbSet.FindAsync(id);
    }

    public async Task AddAsync(T entity)
    {
        await dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public void Update(T entity)
    {
        dbSet.Update(entity);
        _context.SaveChanges();
    }

    public void Remove(T entity)
    {
        dbSet.Remove(entity);
        _context.SaveChanges();
    }
    
    public async Task<bool> ExistsAsync(int id)
    {
        return await dbSet.AnyAsync(e => e.Id == id);
    }
}
