using D3SK.NetCore.Domain.Features;

namespace ExampleBookstore.Services.BookService.Domain.Features.Book
{
    public interface IBookUpdateCommand : IEntityUpdateCommand<IBookDomain, Entities.Book>
    {
    }
}
