using System.Linq;
using System.Threading.Tasks;
using D3SK.NetCore.Common.Extensions;
using D3SK.NetCore.Common.Specifications;
using D3SK.NetCore.Domain;
using D3SK.NetCore.Domain.Events;
using ExampleBookstore.Services.BookService.Domain;
using ExampleBookstore.Services.BookService.Domain.Entities;
using ExampleBookstore.Services.BookService.Domain.Stores;

namespace ExampleBookstore.Services.BookService.Infrastructure.Validation
{
    public class BookValidator : AsyncValidatorBase<Book, IBookDomain>
    {
        private readonly IBookQueryContainer _bookContainer;

        public BookValidator(IBookQueryContainer bookContainer)
        {
            _bookContainer = bookContainer;
        }

        public override async Task<bool> IsValidAsync(Book item, IDomainInstance<IBookDomain> domainInstance)
        {
            var rules = new HasTitle()
                .WithExceptionManager(domainInstance.ExceptionManager)
                .WithError("Book must have a title.")
                .And(new HasIsbn().WithError("Book must have ISBN."));

            var asyncRules = new IsbnIsUnique(_bookContainer)
                .WithExceptionManager(domainInstance.ExceptionManager)
                .WithError("ISBN must be unique.");

            return rules.IsSatisfiedBy(item) && await asyncRules.IsSatisfiedByAsync(item);
        }

        private class HasTitle : Specification<Book>
        {
            protected override bool IsSatisfiedByCondition(Book item)
            {
                return item.Title.IsNotEmpty();
            }
        }

        private class HasIsbn : Specification<Book>
        {
            protected override bool IsSatisfiedByCondition(Book item)
            {
                return item.Isbn.IsNotEmpty();
            }
        }

        private class IsbnIsUnique : AsyncSpecification<Book>
        {
            private readonly IBookQueryContainer _bookContainer;

            public IsbnIsUnique(IBookQueryContainer bookContainer)
            {
                _bookContainer = bookContainer;
            }

            protected override async Task<bool> IsSatisfiedByConditionAsync(Book item)
            {
                var existingItems = await _bookContainer.GetAsync(x => x.Isbn == item.Isbn && x.Id != item.Id);
                return !existingItems.Any();
            }
        }
    }
}
