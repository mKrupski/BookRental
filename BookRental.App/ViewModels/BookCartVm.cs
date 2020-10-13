using System.Collections.Generic;
using BookRental.Domain.Models;

namespace BookRental.App.ViewModels
{
    public class BookCartVm
    {
        public IEnumerable<BookCart> BorrowedBookList { get; set; }
        public Book Book { get; set; }
    }
}
