using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using D3SK.NetCore.Common.Extensions;
using D3SK.NetCore.Common.Specifications;
using D3SK.NetCore.Domain;
using D3SK.NetCore.Domain.Events;
using D3SK.NetCore.Domain.Models;
using ExampleBookstore.Services.BookService.Domain;
using ExampleBookstore.Services.BookService.Domain.Entities;
using ExampleBookstore.Services.BookService.Domain.Stores;

namespace ExampleBookstore.Services.BookService.Infrastructure.Validation
{
    public class BookValidator : AsyncValidatorBase<Book, PatchEntityValidationOptions, IBookDomain>
    {
        private readonly IBookQueryContainer _bookContainer;

        public BookValidator(IBookQueryContainer bookContainer)
        {
            _bookContainer = bookContainer;
        }

        public override async Task<bool> IsValidAsync(Book item, PatchEntityValidationOptions validationOptions, 
            IDomainInstance<IBookDomain> domainInstance)
        {
            var rules = new EmptySpecification<Book>()
                .WithExceptionManager(domainInstance.ExceptionManager)
                .AndIf(() => validationOptions.HasProperty(nameof(Book.Title)),
                    new HasTitle().WithError("Book must have a title."))
                .AndIf(() => validationOptions.HasProperty(nameof(Book.Isbn)),
                    new HasIsbn().WithError("Book must have ISBN."));

            var asyncRules = new AsyncEmptySpecification<Book>()
                .WithExceptionManager(domainInstance.ExceptionManager)
                .AndIfAsync(() => validationOptions.HasProperty(nameof(Book.Isbn)),
                    new IsbnIsUnique(_bookContainer).WithError("ISBN must be unique."));

            var isRulesSatisfied = rules.IsSatisfiedBy(item);
            var isAsyncRulesSatisfied = await asyncRules.IsSatisfiedByAsync(item);
            return isRulesSatisfied && isAsyncRulesSatisfied;
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
                if (item.Isbn.IsEmpty()) return true;

                var existingItems = await _bookContainer.GetAsync(x => x.Isbn == item.Isbn && x.Id != item.Id);
                return !existingItems.Any();
            }
        }
    }
}
