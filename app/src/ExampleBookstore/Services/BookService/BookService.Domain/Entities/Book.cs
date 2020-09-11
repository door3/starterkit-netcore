using System;
using System.Collections.Generic;
using D3SK.NetCore.Common.Entities;
using D3SK.NetCore.Common.Extensions;
using D3SK.NetCore.Domain.Entities;

namespace ExampleBookstore.Services.BookService.Domain.Entities
{
    public class Book : DomainConcurrencyEntityBase, IRootEntity, IReferenceCodeEntity
    {
        public string Isbn { get; set; }

        public string ReferenceCode { get; set; }

        public string Title { get; set; }

        public DateTimeOffset? PublishDate { get; set; }

        public ICollection<BookAuthor> Authors { get; set; } = new HashSet<BookAuthor>();

        public void AddAuthor(int authorId)
        {
            Authors.AddIfMissing(new BookAuthor(Id, authorId), x => x.AuthorId == authorId);
        }

        public void RemoveAuthor(int authorId)
        {
            Authors.RemoveIfExists(x => x.AuthorId == authorId);
        }
    }
}
