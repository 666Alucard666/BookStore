using BLL.Abstractions.ServiceInterfaces;
using Core.DTO_Models;
using Core.Models;
using DAL.Abstractions.Interfaces;

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
                Image = book.Image,
                Created = book.Created,
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
                    return false;
                }
            }
            return true;
        }

        public async Task<bool> EditBookInfo(BookDTO updateBook)
        {
            if (updateBook==null)
            {
                return false;
            }

            var book = _unitOfWork.BookRepository.FindById(updateBook.Id);
            if (book==null)
            {
                return false;
            }
            book.Author = !string.IsNullOrEmpty(updateBook.Author) ? updateBook.Author : book.Author;
            book.Name = !string.IsNullOrEmpty(updateBook.Name) ? updateBook.Name : book.Name;
            book.Genre = !string.IsNullOrEmpty(updateBook.Genre) ? updateBook.Genre : book.Genre;
            book.Publishing = !string.IsNullOrEmpty(updateBook.Publishing) ? updateBook.Publishing : book.Publishing;
            book.Price = updateBook.Price!=0.0 ? updateBook.Price : book.Price;
            book.Image = !string.IsNullOrEmpty(updateBook.Image) ? updateBook.Image : book.Image;
            using (_unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    await _unitOfWork.BookRepository.Update(book);
                    await _unitOfWork.SaveAsync();

                    await _unitOfWork.CommitTransactionAsync();
                }
                catch
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return false;
                }
            }
            return true;
        }

        public async Task<BookDTO> GetBook(int id)
        {
            if (id==0)
            {
                return null;
            }
            var book = await _unitOfWork.BookRepository.FirstOrDefaultAsync(b => b.BookId == id);
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
                Image = book.Image,
                Created = book.Created,
            };
        }
        public IEnumerable<BookDTO> GetAllBooks()
        {
            var collection = _unitOfWork.BookRepository.GetAll().ToList();
            var result = new List<BookDTO>();
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
                    Image = book.Image,
                    Created = book.Created,
                };
                result.Add(bookDTO);
            }

            return result;
        }
        public IEnumerable<BookDTO> GetAllBooksByFilter(BookFilter filter)
        {
            var collection = _unitOfWork.BookRepository.GetAll().ToList();
            var result = new List<BookDTO>();
            if (!String.IsNullOrEmpty(filter.Author) && !(filter.Author == "_"))
            {
                collection = collection.Where(b => b.Author == filter.Author).ToList();
            }

            if (!String.IsNullOrEmpty(filter.Genre)&& !(filter.Genre == "_"))
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
                    Image = book.Image,
                    Created = book.Created,
                };
                result.Add(bookDTO);
            }
            return result;
        }
    }
}
