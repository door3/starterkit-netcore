using System;
using System.Collections.Generic;
using System.Text;
using D3SK.NetCore.Common.Entities;
using D3SK.NetCore.Domain.Entities;

namespace ExampleBookstore.Services.BookService.Domain.Entities
{
    public class Author : DomainConcurrencyEntityBase, IRootEntity, IReferenceCodeEntity
    {
        public string ReferenceCode { get; set; }

        public DomainName Name { get; set; } = new DomainName();

        public DateTimeOffset? DateOfBirth { get; set; }

        public ICollection<BookAuthor> Books = new HashSet<BookAuthor>();
    }
}
