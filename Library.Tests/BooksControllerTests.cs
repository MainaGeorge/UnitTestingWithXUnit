using LibraryApp.Api.Controllers;
using LibraryApp.Api.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Library.Tests
{
    public class BooksControllerTests
    {
        private readonly BooksController _booksController;
        private readonly IBookService _service;

        public BooksControllerTests()
        {
            _service = new BookService();
            _booksController = new BooksController(_service);
        }
        [Fact]
        public void GetAll_ReturnsAListOfBooks()
        {
            //arrange

            //act
            var getAllResponse = _booksController.GetAll();

            var bookListObject = getAllResponse.Result as OkObjectResult;

            var bookList = bookListObject!.Value as List<Book>;


            //assert
            Assert.IsType<OkObjectResult>(getAllResponse.Result);
            Assert.IsType<List<Book>>(bookListObject.Value);
            Assert.Equal(BookService.GetTestBookItems().Count(), bookList.Count);
        }

        [Theory]
        [InlineData("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200")]
        public void GetBookById_ReturnsABook_IfIdIsValid(string validGuidString)
        {
            //act
            var validGuid = new Guid(validGuidString);

            //arrange
            var validResponse = _booksController.GetById(validGuid);
            var response = validResponse.Result as OkObjectResult;
            var book = response!.Value as Book;


            //assert
            Assert.IsType<OkObjectResult>(validResponse.Result);
            Assert.IsType<Book>(response!.Value);
            Assert.Equal(book.Id, validGuid);

        }

        [Theory]
        [InlineData("ab2bd817-48cd-4cf3-a90a-53ea0cd9c245")]
        public void GetBooksById_ReturnsNotFound_IfIdIsInvalid(string invalidGuidString)
        {
            //arrange
            var invalidGuid = new Guid(invalidGuidString);

            //act
            var response = _booksController.GetById(invalidGuid);

            //assert
            Assert.IsType<NotFoundResult>(response.Result);
        }

        [Fact]
        public void PostBook_ReturnsCreatedAtAction_IfBookIsValid()
        {
            //arrange

            var validBook = new Book
            {
                Author = "Nuria Martinez",
                Id = new Guid(),
                Title = "Being a mother"
            };
            //act
            var postResponse = _booksController.PostBook(validBook);

            var responseResult = postResponse as CreatedAtActionResult;
            var createdBook = responseResult!.Value as Book;

            //assert
            Assert.IsType<CreatedAtActionResult>(postResponse);
            Assert.Equal(createdBook!.Author, validBook.Author);
            Assert.Equal(createdBook.Title, validBook.Title);
        }

        [Fact]
        public void PostBook_ReturnsBadRequest_IfBookIsInvalid()
        {
            //arrange

            var validBook = new Book
            {
                Author = "Nuria Martinez",
                Id = new Guid(),
            };

            //act

            _booksController.ModelState.AddModelError("Title", "This is a required field");
            var postResponse = _booksController.PostBook(validBook);

            //assert
            Assert.IsType<BadRequestObjectResult>(postResponse);

        }

        [Theory]
        [InlineData("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200")]
        public void DeleteBook_ReturnsNoContent_UponSuccessfulDeletion(string validGuidString)
        {
            //arrange
            var validGuid = new Guid(validGuidString);

            //act
            var deleteResponse = _booksController.DeleteBook(validGuid);

            //assert
            Assert.IsType<NoContentResult>(deleteResponse);

        }

        [Theory]
        [InlineData("ab2bd817-78cd-4cf3-a80a-53ea0cd9c800")]
        public void DeleteBook_ReturnsNotFound_UponUnsuccessfulDeletion(string invalidGuidString)
        {
            //arrange
            var validGuid = new Guid(invalidGuidString);

            //act
            var deleteResponse = _booksController.DeleteBook(validGuid);

            //assert
            Assert.IsType<NotFoundResult>(deleteResponse);
            Assert.Equal(5, _service.GetAll().Count());
        }
    }
}
