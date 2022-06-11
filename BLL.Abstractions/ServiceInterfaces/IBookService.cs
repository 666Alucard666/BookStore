using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTO_Models;
using Core.Models;

namespace BLL.Abstractions.ServiceInterfaces
{
    public interface IBookService
    {
        Task<bool> CreateBook(BookDTO book);
        Task<bool> DeleteBook(BookDTO book);
        Task<bool> EditBookInfo(BookDTO updateBook);
        IEnumerable<BookDTO> GetAllBooksByFilter(BookFilter filter);
        IEnumerable<BookDTO> GetAllBooks();
        Task<BookDTO> GetBook(int id);
    }
}
