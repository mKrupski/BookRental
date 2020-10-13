using BookRental.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookRental.Infrastructure.Context
{
    public class BookRentalDbContext : IdentityDbContext
    {
        public BookRentalDbContext(DbContextOptions<BookRentalDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<BookCart> BookCarts { get; set; }
    }
}
