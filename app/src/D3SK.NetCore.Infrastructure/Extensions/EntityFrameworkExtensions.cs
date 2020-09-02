using System;
using System.Collections.Generic;
using System.Linq;
using D3SK.NetCore.Common.Entities;
using D3SK.NetCore.Common.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace D3SK.NetCore.Infrastructure.Extensions
{
    public static class EntityFrameworkExtensions
    {
        public const int DefaultPrecision = 18;

        public const int DefaultScale = 2;

        public static PropertyBuilder<decimal?> HasPrecision(this PropertyBuilder<decimal?> builder,
            int precision = DefaultPrecision, int scale = DefaultScale)
        {
            return builder.HasColumnType($"decimal({precision},{scale})");
        }

        public static PropertyBuilder<decimal> HasPrecision(this PropertyBuilder<decimal> builder,
            int precision = DefaultPrecision, int scale = DefaultScale)
        {
            return builder.HasColumnType($"decimal({precision},{scale})");
        }

        public static PropertyBuilder<decimal?> IsDecimal(this PropertyBuilder<decimal?> builder, int decimals = 2)
        {
            return builder.HasPrecision(scale: decimals);
        }

        public static PropertyBuilder<decimal> IsDecimal(this PropertyBuilder<decimal> builder, int decimals = 2)
        {
            return builder.HasPrecision(scale: decimals);
        }

        public static PropertyBuilder<decimal?> IsPercent(this PropertyBuilder<decimal?> builder, int decimals = 4)
        {
            return builder.HasPrecision(scale: decimals);
        }

        public static PropertyBuilder<decimal> IsPercent(this PropertyBuilder<decimal> builder, int decimals = 4)
        {
            return builder.HasPrecision(scale: decimals);
        }

        public static PropertyBuilder<decimal?> IsMoney(this PropertyBuilder<decimal?> builder)
        {
            return builder.HasPrecision(scale: 2);
        }

        public static PropertyBuilder<decimal> IsMoney(this PropertyBuilder<decimal> builder)
        {
            return builder.HasPrecision(scale: 2);
        }

        public static EntityTypeBuilder<T> CreateLookupEntity<T>(this ModelBuilder modelBuilder, string tableName,
            IList<T> seedData = null) where T : class, ILookupEntity<int> =>
            CreateLookupEntity<T, int>(modelBuilder, tableName, seedData);

        public static EntityTypeBuilder<T> CreateLookupEntity<T, TKey>(this ModelBuilder modelBuilder, string tableName,
            IList<T> seedData = null)
            where T : class, ILookupEntity<TKey>
            where TKey : IComparable
        {
            var entity = modelBuilder.Entity<T>();
            entity.ToTable(tableName);
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).IsRequired();
            
            seedData ??= LookupHelper.GetAll<T, TKey>().ToList();

            if (seedData.Any())
            {
                entity.HasData(seedData.ToArray());
            }

            return entity;
        }
    }
}