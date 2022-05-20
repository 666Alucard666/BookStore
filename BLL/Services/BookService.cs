using BLL.Abstractions.ServiceInterfaces;
using Core.DTO_Models;
using Core.Models;
using DAL.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class BookService : IBookService
    {
        private IUnitOfWork _unitOfWork;
        public BookService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> CreateBook(BookDTO book)
        {
            if (book == null || string.IsNullOrEmpty(book.Name)
                || string.IsNullOrEmpty(book.Author)|| string.IsNullOrEmpty(book.Genre))
            {
                return false;
            }

            if (await _unitOfWork.BookRepository.Any(b => b.Name.Equals(book.Name) && b.Author.Equals(book.Author)))
            {
                return false;
            }

            var bookcreate = new Book()
            {
                Name = book.Name,
                Author = book.Author,
                Genre = book.Genre,
                Price = book.Price,
                Publishing = book.Publishing,
                AmountOnStore = book.AmountOnStore,
                OrdersBook = new List<OrdersBooks>()
            };

            using (_unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    _unitOfWork.BookRepository.Create(bookcreate);
                    await _unitOfWork.SaveAsync();

                    await _unitOfWork.CommitTransactionAsync();
                }
                catch
                {
                    await _unitOfWork.RollbackTransactionAsync();
                }
            }
            return true;
        }

        public async Task<bool> DeleteBook(BookDTO book)
        {
            if (book == null || string.IsNullOrEmpty(book.Name)
                || string.IsNullOrEmpty(book.Author))
            {
                return false;
            }

            if (!await _unitOfWork.BookRepository.Any(b => b.Name.Equals(book.Name) && b.Author.Equals(book.Author)))
            {
                return false;
            }

            var delbook = await _unitOfWork.BookRepository.FirstOrDefaultAsync(b=>b.Name.Equals(book.Name)&& b.Author.Equals(book.Author));
            if (delbook == null)
            {
                return false;
            }
            using (_unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    _unitOfWork.BookRepository.Remove(delbook);
                    await _unitOfWork.SaveAsync();

                    await _unitOfWork.CommitTransactionAsync();
                }
                catch
                {
                    await _unitOfWork.RollbackTransactionAsync();
                }
            }
            return true;
        }

        public async Task<bool> EditPrice(int id, double price)
        {
            if (!await _unitOfWork.BookRepository.Any(b=>b.BookId == id))
            {
                return false;
            }

            var needbook = await _unitOfWork.BookRepository.FirstOrDefaultAsync(b => b.BookId == id);
            
            if (needbook == null)
            {
                return false;
            }
            
            needbook.Price = price;

            using (_unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    await _unitOfWork.BookRepository.Update(needbook);
                    await _unitOfWork.SaveAsync();

                    await _unitOfWork.CommitTransactionAsync();
                }
                catch
                {
                    await _unitOfWork.RollbackTransactionAsync();
                }
            }
            return true;
        }

        public async Task<BookDTO> GetBook(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }
            var book = await _unitOfWork.BookRepository.FirstOrDefaultAsync(b => b.Name == name);
            if (book == null)
            {
                return null;
            }
            return new BookDTO
            {
                AmountOnStore = book.AmountOnStore,
                Name = book.Name,
                Genre = book.Genre,
                Author = book.Author,
                Id = book.BookId,
                Price = book.Price,
                Publishing = book.Publishing,
            };
        }

        public IEnumerable<BookDTO> GetAllBooksByFilter(BookFilter filter)
        {
            var collection = _unitOfWork.BookRepository.GetAll().ToList();
            var result = new List<BookDTO>();
            if (!String.IsNullOrEmpty(filter.Author))
            {
                collection = collection.Where(b => b.Author == filter.Author).ToList();
            }

            if (!String.IsNullOrEmpty(filter.Genre))
            {
                collection = collection.Where(b => b.Genre.Equals(filter.Genre)).ToList();
            }

            if (filter.StartPrice>0 || filter.EndPrice<99999)
            {
                collection = collection.Where(b => b.Price >= filter.StartPrice && b.Price <= filter.EndPrice).ToList();
            }

            if (collection.Count==0)
            {
                return result;
            }

            foreach (var book in collection)
            {
                var bookDTO = new BookDTO
                {
                    Id = book.BookId,
                    AmountOnStore = book.AmountOnStore,
                    Author = book.Author,
                    Genre = book.Genre,
                    Price = book.Price,
                    Publishing = book.Publishing,
                    Name = book.Name,
                };
                result.Add(bookDTO);
            }

            return result;
        }
    }
}
