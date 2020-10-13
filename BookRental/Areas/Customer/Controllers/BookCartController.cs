using System.Security.Claims;
using System.Threading.Tasks;
using BookRental.App.ViewModels;
using BookRental.Domain.Models;
using BookRental.Infrastructure.Repositories.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace BookRental.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class BookCartController : Controller
    {
        private readonly IRepository<BookCart> _bookCartRepository;
        private readonly IBookRepository _bookRepository;

        [BindProperty] public BookCartVm BookCartVm { get; set; }

        public BookCartController(IRepository<BookCart> bookCartRepository, IBookRepository bookRepository)
        {
            _bookCartRepository = bookCartRepository;
            _bookRepository = bookRepository;
        }

        public async Task<IActionResult> Index()
        {
            var claimsIdentity = (ClaimsIdentity) User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            BookCartVm = new BookCartVm()
            {
                BorrowedBookList =
                   await _bookCartRepository.GetAllWithProperties(u => u.AppUserId == claim.Value, includeProperties: "Book")
            };

            return View(BookCartVm);
        }

        [HttpGet]
        public async Task<IActionResult> ReturnBook(int bookCartId)
        {
            var bookCartToRemove = await _bookCartRepository.GetFirstOrDefaultWithProperties(b => b.Id == bookCartId,
                includeProperties: "Book");
            await _bookRepository.ChangeBookStatus(bookCartToRemove.BookId);
            await _bookCartRepository.Delete(bookCartToRemove);

            return RedirectToAction(nameof(Index));
        }
    }
}
