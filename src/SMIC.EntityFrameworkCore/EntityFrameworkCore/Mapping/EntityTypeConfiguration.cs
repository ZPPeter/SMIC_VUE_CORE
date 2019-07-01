using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SMIC.EntityFrameworkCore.Mapping
{
    public abstract class EntityTypeConfiguration<T> : IEntityTypeConfiguration<T> where T : class
    {
        //remove ctor with parameters. see https://github.com/aspnet/EntityFrameworkCore/blob/master/src/EFCore/ModelBuilder.cs#L359
        public abstract void Configure(EntityTypeBuilder<T> builder);
    }
}
