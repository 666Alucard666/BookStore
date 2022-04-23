using System.Linq.Expressions;
using Core.Models;
using DAL.Abstractions.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class OrderRepos : BaseRepository, IGenericRepos<Order>, IDisposable
{
    private AppDbContext context;
    public OrderRepos(AppDbContext context)
    {
        this.context = context;
    }
    public void Create(Order item)
    {
        context.Orders.Add(item);
    }

    public Order FirstOrDefault(Expression<Func<Order, bool>> predicate)
    {
        return context.Orders.FirstOrDefault(predicate);
    }

    public Order FirstOrDefault()
    {
        return context.Orders.FirstOrDefault();
    }

    public Task<Order> FirstOrDefaultAsync()
    {
        return  context.Orders.FirstOrDefaultAsync();
    }

    public Task<Order> FirstOrDefaultAsync(Expression<Func<Order, bool>> predicate)
    {
        return context.Orders.FirstOrDefaultAsync(predicate);
    }

    public Order FindById(int id)
    {
        return context.Orders.Find(id);
    }

    public IQueryable<Order> GetAll()
    {
        return context.Orders.AsNoTracking();
    }

    public IEnumerable<Order> Get(Func<Order, bool> predicate)
    {
        return context.Orders.Where(predicate).ToList();
    }

    public async Task<bool> Any(Expression<Func<Order, bool>> predicate)
    {
        return context.Orders.Any(predicate);
    }
    public async Task<bool> Any()
    {
        return context.Orders.Any();
    }
    public void Update(Order item)
    {
        context.Entry(item).State = EntityState.Modified;
        this.Save();
    }

    public void RemoveById(int id)
    {
        context.Orders.Remove(this.FindById(id));
    }

    public void Remove(Order item)
    {
        context.Orders.Remove(item);
    }
    public  void Save()
    { 
        context.SaveChangesAsync();
    }
    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                context.Dispose();
            }

            // TODO: освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить метод завершения
            // TODO: установить значение NULL для больших полей
            this.disposedValue = true;
        }
    }

    public bool disposedValue;

    // // TODO: переопределить метод завершения, только если "Dispose(bool disposing)" содержит код для освобождения неуправляемых ресурсов
    // ~UserRepository()
    // {
    //     // Не изменяйте этот код. Разместите код очистки в методе "Dispose(bool disposing)".
    //     Dispose(disposing: false);
    // }

    public void Dispose()
    {
        // Не изменяйте этот код. Разместите код очистки в методе "Dispose(bool disposing)".
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}