using System.Linq.Expressions;
using Core.Models;
using DAL.Abstractions.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class UserRepos: BaseRepository,IGenericRepos<User>, IDisposable
{
    private AppDbContext context;
    public UserRepos(AppDbContext context)
    {
        this.context = context;
    }

    public void Create(User item)
    {
        context.Users.Add(item);
    }

    public User FirstOrDefault(Expression<Func<User, bool>> predicate)
    {
        return context.Users.FirstOrDefault(predicate);
    }

    public User FirstOrDefault()
    {
        return context.Users.FirstOrDefault();
    }

    public Task<User> FirstOrDefaultAsync()
    {
        return context.Users.FirstOrDefaultAsync();
    }
    public Task<User> FirstOrDefaultAsync(Expression<Func<User, bool>> predicate)
    {
        return context.Users.FirstOrDefaultAsync(predicate);
    }
    public User FindById(int id)
    {
        return context.Users.Find(id);
    }

    public IQueryable<User> GetAll()
    {
        return context.Users.AsNoTracking();
    }

    public IEnumerable<User> Get(Func<User, bool> predicate)
    {
        return context.Users.Where(predicate).ToList();
    }

    public async Task<bool> Any(Expression<Func<User, bool>> predicate)
    {
        return context.Users.Any(predicate);
    }
    public async Task<bool> Any()
    {
        return context.Users.Any();
    }
    public async void Update(User item)
    {
        context.Entry(item).State = EntityState.Modified;
        this.Save();
    }

    public void RemoveById(int id)
    {
        context.Users.Remove(this.FindById(id));
    }
    
    public void Remove(User item)
    {
        context.Users.Remove(item);
    }

    public async void Save()
    {
        await context.SaveChangesAsync();
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