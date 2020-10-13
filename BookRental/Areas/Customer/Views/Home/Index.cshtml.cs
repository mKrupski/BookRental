using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookRental.Domain.Models;
using BookRental.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BookRental.Pages.BookList
{
    public class IndexModel : PageModel, IEnumerable
    {
        private readonly BookRentalDbContext _context;
        public IEnumerable<Book> Books { get; set; }

        public IndexModel(BookRentalDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task OnGet()
        {
            Books = await _context.Books.ToListAsync();
        }

        public IEnumerator GetEnumerator()
        {
            throw new System.NotImplementedException();
        }
    }
}