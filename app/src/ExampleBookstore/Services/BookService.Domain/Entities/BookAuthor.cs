using System;
using System.Collections.Generic;
using System.Text;
using D3SK.NetCore.Common.Entities;

namespace ExampleBookstore.Services.BookService.Domain.Entities
{
    public class BookAuthor : IManyToManyEntity
    {
        public int BookId { get; set; }

        public Book Book { get; set; }

        public int AuthorId { get; set; }

        public Author Author { get; set; }

        public BookAuthor()
        {
        }

        public BookAuthor(int bookId, int authorId)
        {
            BookId = bookId;
            AuthorId = authorId;
        }

        public IEnumerable<object> GetUniqueValues()
        {
            yield return BookId;
            yield return AuthorId;
        }
    }
}
