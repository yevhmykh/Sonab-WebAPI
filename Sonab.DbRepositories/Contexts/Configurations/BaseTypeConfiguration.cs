using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sonab.Core.Entities;

namespace Sonab.DbRepositories.Contexts.Configurations;

public class BaseTypeConfiguration<TKey> : IEntityTypeConfiguration<TKey> where TKey : Entity
{
    public virtual void Configure(EntityTypeBuilder<TKey> builder)
    {
        builder.HasKey(k => k.Id);

        builder.Property(k => k.Id)
            .ValueGeneratedOnAdd();
    }
}
