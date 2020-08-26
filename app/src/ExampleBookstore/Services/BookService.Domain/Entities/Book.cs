using System;
using System.Collections.Generic;
using System.Text;
using D3SK.NetCore.Common.Entities;
using D3SK.NetCore.Domain.Entities;

namespace ExampleBookstore.Services.BookService.Domain.Entities
{
    public class Book : DomainConcurrencyEntityBase, IRootEntity, IReferenceCodeEntity
    {
        public string ReferenceCode { get; set; }

        public string Title { get; set; }

        public DateTimeOffset? PublishDate { get; set; }

        public ICollection<BookAuthor> Authors { get; set; } = new HashSet<BookAuthor>();
    }
}
