using BLL.Abstractions.ServiceInterfaces;
using Core.DTO_Models;
using Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FluentResults;
using Microsoft.AspNetCore.Authorization;

namespace BookStoreUI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/book")]
    [Produces("application/json")]
    public class BookController : Controller
    {
        private readonly IBookService _bookService;
        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }
        [HttpPost("CreateBook")]
        public async Task <ActionResult> Create(BookDTO book)
        {
            if (book == null)
            {
                return BadRequest("Wrong book data!");
            }
            var b = await _bookService.CreateBook(book);

            if (!b)
            {
                return BadRequest("Book cannot be created!");
            }

            return Ok();
        }


        [HttpPost("EditBookPrice")]
        public async Task<ActionResult> EditPrice([FromBody]EdPrice price)
        {
            var b = await _bookService.EditPrice(price.Id, price.Price);

            if (!b)
            {
                return BadRequest("Price cannot be changed!");
            }

            return Ok();
        }

        [HttpDelete("DeleteBook")]
        public async Task<ActionResult> Delete(BookDTO book)
        {
            if (book == null)
            {
                return BadRequest("Wrong book data!");
            }
            var b = await _bookService.DeleteBook(book);
            if (!b)
            {
                return BadRequest("Book cannot be deleted!");
            }

            return Ok();
        }
        [HttpGet("GetBooksByFilter")]
        public ActionResult<BookDTO> GetBooksByFilter([FromBody]BookFilter filter)
        {
            var b =  _bookService.GetAllBooksByFilter(filter).ToList();
            return Ok(b);
        }
        [HttpGet("GetBook")]
        public async Task< Result<BookDTO>> GetBook(string name)
        {
            var b = await _bookService.GetBook(name);
            if (b==null)
            {
                return Result.Fail("Book not found");
            }
            return Result.Ok(b);
        }
    }
}
