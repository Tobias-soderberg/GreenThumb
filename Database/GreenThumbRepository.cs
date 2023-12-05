using Microsoft.EntityFrameworkCore;
using System.Windows;

namespace GreenThumb.Database;

internal class GreenThumbRepository<T> where T : class
{
    private readonly GreenThumbDbContext _context;
    private readonly DbSet<T> _dbSet;
    public GreenThumbRepository(GreenThumbDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public List<T> GetAll()
    {
        return _dbSet.ToList();
    }

    public List<T>? GetAllInclude(string tableName)
    {
        try
        {
            return _dbSet.Include(tableName).ToList();
        }
        catch
        {
            MessageBox.Show("Cant include that table!");
            return null;
        }

    }
    public T? Get(int id)
    {
        return _dbSet.Find(id);
    }
    public void Add(T entity)
    {
        _dbSet.Add(entity);
    }

    public void Update(T entity)
    {
        _context.Attach(entity);
        _dbSet.Update(entity);
        return;
    }

    public void Delete(int id)
    {
        T? entityToDelete = Get(id);
        if (entityToDelete != null)
        {
            _dbSet.Remove(entityToDelete);
        }
    }
    public void Complete()
    {
        _context.SaveChanges();
    }
}
