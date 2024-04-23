using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.EFCore.Config
{
    public class BookConfig : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasData(
                new Book { Id = 1, CategoryId = 2, Title = "Satranc", Price = 25 },
                new Book { Id = 2, CategoryId = 1, Title = "Nutuk", Price = 225 },
                new Book { Id = 3, CategoryId = 3, Title = "Alacakaranlik", Price = 125 }
                );
        }
    }
}
