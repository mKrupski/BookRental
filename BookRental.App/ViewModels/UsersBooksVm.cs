using System.Collections.Generic;
using BookRental.Domain.Models;

namespace BookRental.App.ViewModels
{
    public class UsersBooksVm
    {
        public IEnumerable<BookCart> BorrowedBookList { get; set; }
        public Book Book { get; set; }
        public IEnumerable<BookCart> Users { get; set; }
        public AppUser AppUser { get; set; }
    }
}
