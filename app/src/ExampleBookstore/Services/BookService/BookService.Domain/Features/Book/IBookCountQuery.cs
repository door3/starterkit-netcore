using D3SK.NetCore.Domain.Features;

namespace ExampleBookstore.Services.BookService.Domain.Features.Book
{
    public interface IBookCountQuery : IEntityCountQuery<IBookDomain, Entities.Book>
    {
    }
}
