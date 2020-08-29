using System.Collections.Generic;
using D3SK.NetCore.Common.Entities;
using D3SK.NetCore.Common.Stores;

namespace ExampleBookstore.Services.BookService.Domain.Entities
{
    public class BookAuthor : IManyToManyEntity
    {
        public int BookId { get; set; }

        [UpdateStrategy(NullOnAdd = true)]
        public Book Book { get; set; }

        public int AuthorId { get; set; }

        [UpdateStrategy(NullOnAdd = true)]
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
