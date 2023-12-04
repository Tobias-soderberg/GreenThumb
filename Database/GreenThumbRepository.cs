using Microsoft.EntityFrameworkCore;
using System.Reflection;
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

    public void Update(int id, T newEntity)
    {
        T? entityToUpdate = Get(id);                                //Get the object to update
        var oldProps = entityToUpdate.GetType().GetProperties();    //Get the Array of properties from the object
        for (int i = 0; i < oldProps.Count(); i++)                  //Loop through all the properties
        {
            PropertyInfo? propInfo = entityToUpdate.GetType()       //Get type of object
                .GetProperty(oldProps[i].ToString().Split(" ")[1]); //Get the property name as a string, [1] is because you get for example "System.String Name"
                                                                    //and dont want the reference
            var propValue = newEntity.GetType().GetProperty(propInfo.Name).GetValue(newEntity); //Get the value of the property with name we got from above

            if (i != 0 && propInfo != null && propValue != null)    //Null checks != 0 because we dont want to change id (Have to make sure all keys are not changed)
            {
                entityToUpdate.GetType().GetProperty(propInfo.Name).SetValue(entityToUpdate, propValue); //Updates the new value into the entityToUpdate
            }
        }
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