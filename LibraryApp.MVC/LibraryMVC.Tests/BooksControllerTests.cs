using Library.API.Data.Models;
using LibraryApp.Controllers;
using LibraryApp.Data.MockData;
using LibraryApp.Data.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace LibraryMVC.Tests
{
    public class BooksControllerTests
    {
        [Fact]
        public void IndexTests()
        {
            //arrange
            var mockRepo = new Mock<IBookService>();
            mockRepo.Setup(n => n.GetAll()).Returns(MockData.GetTestBookItems());
            var controller = new BooksController(mockRepo.Object);

            //act
            var indexResult = controller.Index();

            //
            var viewResult = Assert.IsType<ViewResult>(indexResult);
            var viewResultBooks = Assert.IsAssignableFrom<List<Book>>(viewResult!.ViewData.Model);
            Assert.Equal(5, viewResultBooks.Count);
        }

        [Theory]
        [InlineData("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200")]
        public void DetailsTests_ValidBookId(string validGuid)
        {
            //arrange
            var id = new Guid(validGuid);
            var mockRepo = new Mock<IBookService>();
            mockRepo.Setup(n => n.GetById(id))
                .Returns(MockData.GetTestBookItems().FirstOrDefault(b => b.Id == id));
            var controller = new BooksController(mockRepo.Object);

            //act
            var detailsResult = controller.Details(id);

            //assert
            var detailViewResult = Assert.IsType<ViewResult>(detailsResult);
            var detailViewBook = Assert.IsType<Book>(detailViewResult.ViewData.Model);
            Assert.Equal(detailViewBook.Id, id);

        }

        [Theory]
        [InlineData("ab1bd847-98cd-4cf5-a80a-79ea0cd9c200")]
        public void DetailsTests_InvalidBookId(string invalidGuid)
        {
            //arrange
            var id = new Guid(invalidGuid);
            var mockRepo = new Mock<IBookService>();
            mockRepo.Setup(n => n.GetById(id))
                .Returns(MockData.GetTestBookItems().FirstOrDefault(b => b.Id == id));
            var controller = new BooksController(mockRepo.Object);

            //act
            var detailsResult = controller.Details(id);

            //assert
            Assert.IsType<NotFoundResult>(detailsResult);

        }

        [Fact]
        public void CreateTest_ValidBook()
        {
            //arrange
            var mockRepo = new Mock<IBookService>();
            var controller = new BooksController(mockRepo.Object);
            var book = new Book
            {
                Id = new Guid(),
                Title = "How to get away with murder",
                Author = "Oliver Maina"
            };

            // act
            var createResult = controller.Create(book);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(createResult);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Null(redirectResult.ControllerName);
        }

        [Fact]
        public void CreateTest_InvalidBook()
        {
            //arrange
            var mockRepo = new Mock<IBookService>();
            var controller = new BooksController(mockRepo.Object);
            var book = new Book
            {
                Id = new Guid(),
                Title = "How to get away with murder",
            };

            // act
            controller.ModelState.AddModelError("Author", "Author is required");
            var badRequestResult = controller.Create(book);

            // Assert
            var badResult = Assert.IsType<BadRequestObjectResult>(badRequestResult);

        }

    }
}
