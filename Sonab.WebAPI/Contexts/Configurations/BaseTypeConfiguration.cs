using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sonab.WebAPI.Models.DB;

namespace Sonab.WebAPI.Contexts.Configurations;

public class BaseTypeConfiguration<IKey> : IEntityTypeConfiguration<IKey> where IKey : Key
{
    public virtual void Configure(EntityTypeBuilder<IKey> builder)
    {
        builder.HasKey(k => k.Id);

        builder.Property(k => k.Id)
            .ValueGeneratedOnAdd();
    }
}
