using System;
using System.Collections.Generic;
using LibraryApp.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        public readonly IBookService BookService;

        public BooksController(IBookService bookService)
        {
            BookService = bookService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Book>> GetAll()
        {
            return Ok(BookService.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<Book> GetById(Guid id)
        {
            var book = BookService.GetById(id);

            if (book == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(book);
            }
        }

        [HttpPost]
        public ActionResult PostBook([FromBody] Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            BookService.Add(book);

            return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteBook(Guid id)
        {
            var book = BookService.GetById(id);

            if (book == null)
            {
                return NotFound();
            }
            else
            {
                BookService.Remove(id);
                return NoContent();
            }
        }
    }
}
