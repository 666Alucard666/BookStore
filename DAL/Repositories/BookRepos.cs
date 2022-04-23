using System.Linq.Expressions;
using Core.Models;
using DAL.Abstractions.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class BookRepos : BaseRepository,IGenericRepos<Book>, IDisposable
{
    private AppDbContext context;
    public BookRepos(AppDbContext context)
    {
        this.context = context;
    }

    public void Create(Book item)
    {
        context.Books.Add(item);
    }

    public Book FirstOrDefault(Expression<Func<Book, bool>> predicate)
    {
        return context.Books.FirstOrDefault(predicate);
    }

    public Book FirstOrDefault()
    {
        return context.Books.FirstOrDefault();
    }

    public Task<Book> FirstOrDefaultAsync()
    {
        return  context.Books.FirstOrDefaultAsync();
    }

    public Task<Book> FirstOrDefaultAsync(Expression<Func<Book, bool>> predicate)
    {
        return context.Books.FirstOrDefaultAsync(predicate);
    }

    public Book FindById(int id)
    {
        return context.Books.Find(id);
    }

    public IQueryable<Book> GetAll()
    {
        return context.Books.AsNoTracking();
    }

    public IEnumerable<Book> Get(Func<Book, bool> predicate)
    {
        return context.Books.Where(predicate).ToList();
    }

    public async Task<bool> Any(Expression<Func<Book, bool>> predicate)
    {
        return context.Books.Any(predicate);
    }
    public async Task<bool> Any()
    {
        return context.Books.Any();
    }
    public void Update(Book item)
    {
        context.Entry(item).State = EntityState.Modified;
        this.Save();
    }

    public void RemoveById(int id)
    {
        context.Books.Remove(this.FindById(id));
    }

    public void Remove(Book item)
    {
        context.Books.Remove(item);
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