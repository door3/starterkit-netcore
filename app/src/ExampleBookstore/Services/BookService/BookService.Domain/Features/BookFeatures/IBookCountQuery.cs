using D3SK.NetCore.Domain.Features;
using ExampleBookstore.Services.BookService.Domain.Entities;

namespace ExampleBookstore.Services.BookService.Domain.Features.BookFeatures
{
    public interface IBookCountQuery : IEntityCountQuery<IBookDomain, Book>
    {
    }
}
