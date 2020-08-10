using System;
using System.Collections.Generic;

namespace LibraryApp.Api.Services
{
    public interface IBookService
    {
        IEnumerable<Book> GetAll();
        Book GetById(Guid id);

        void Remove(Guid id);

        void Add(Book newBook);
    }
}
