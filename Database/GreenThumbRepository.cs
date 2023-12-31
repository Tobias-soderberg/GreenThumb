﻿using Microsoft.EntityFrameworkCore;
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

    //Send argument as a string for example "Garden.Plants" to include Users Garden and the Plants in it
    public List<T>? GetAllInclude(params string[] navigationProperties)
    {
        try
        {
            IQueryable<T> query = _dbSet;   //Set to Queryable to build onto the LINQ method so it will be include().include().... until all sent in are included
                                            //Almost as a string builder if I understand correct

            foreach (var navigationProperty in navigationProperties)
            {
                query = query.Include(navigationProperty); // Build the query
            }
            return query.ToList(); //Execute and make it to a list
        }
        catch
        {
            MessageBox.Show("Can't include one or more tables!");
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
        _context.Attach(entity); //Attach to the context to make the program look at the object for changes and update if there are any
        _dbSet.Update(entity);
        return;
    }

    public void Delete(T entityToDelete)
    {
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
