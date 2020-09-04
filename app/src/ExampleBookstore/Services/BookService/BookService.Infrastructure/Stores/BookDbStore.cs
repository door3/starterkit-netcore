using D3SK.NetCore.Common;
using D3SK.NetCore.Common.Utilities;
using D3SK.NetCore.Domain;
using D3SK.NetCore.Infrastructure.Stores;
using ExampleBookstore.Services.BookService.Domain;
using ExampleBookstore.Services.BookService.Domain.Entities;
using ExampleBookstore.Services.BookService.Domain.Stores;
using Microsoft.EntityFrameworkCore;

namespace ExampleBookstore.Services.BookService.Infrastructure.Stores
{
    public class BookDbStore : TenantDbStoreBase, IBookStore, IBookQueryStore
    {
        public BookDbStore(DbContextOptions options,
            IDomainInstance<IBookDomain> domainInstance,
            ITenantManager tenantManager,
            IClock currentClock)
            : base(options, domainInstance, tenantManager, currentClock)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("book");

            modelBuilder.Entity<Author>(entity =>
            {
                entity.ToTable("Authors").HasKey(x => x.Id);
                entity.Property(x => x.RowVersion).IsRowVersion();
                ApplyDeletedAndTenantIdFilter(entity);
                entity.OwnsOne(x => x.Name);
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.ToTable("Books").HasKey(x => x.Id);
                entity.Property(x => x.RowVersion).IsRowVersion();
                ApplyDeletedAndTenantIdFilter(entity);
            });

            modelBuilder.Entity<BookAuthor>(entity =>
            {
                entity.ToTable("BookAuthors").HasKey(x => new {x.BookId, x.AuthorId});
                entity.HasOne(x => x.Book)
                    .WithMany(x => x.Authors)
                    .HasForeignKey(x => x.BookId);
                entity.HasOne(x => x.Author)
                    .WithMany(x => x.Books)
                    .HasForeignKey(x => x.AuthorId);
            });
        }
    }
}