using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BookRental.Domain.Models;
using BookRental.Infrastructure.Repositories.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookRental.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IBookRepository _bookRepository;
        private readonly IRepository<BookCart> _bookCartRepository;

        public HomeController(ILogger<HomeController> logger, IBookRepository bookRepository, IRepository<BookCart> bookCartRepository)
        {
            _logger = logger;
            _bookRepository = bookRepository;
            _bookCartRepository = bookCartRepository;
        }

        public async Task<IActionResult> Index()
        {
            var bookList = await _bookRepository.GetAll().ToListAsync();
            return View(bookList);
        }


        public async Task<IActionResult> BooksToRent()
        {
            var bookList = await _bookRepository.GetBooksToRent();
            return View("Index", bookList);
        }


        public async Task<IActionResult> BooksOnLoan()
        {
            var bookList = await _bookRepository.GetBooksOnLoan();
            return View("Index", bookList);
        }

        public async Task<IActionResult> Details(int id)
        {
            var book = await _bookRepository.GetFirstOrDefaultWithProperties(u => u.Id == id, includeProperties: "Category");
            var bookCart = new BookCart()
            {
                Book = book,
                BookId = book.Id
            };
            return View(bookCart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Details(BookCart bookCart)
        {
            bookCart.Id = 0;
            if (ModelState.IsValid)
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                bookCart.AppUserId = claim.Value;

                // u cant borrow the same book many times
                var borrowedBook =  await _bookCartRepository.GetAll().Where(b=>b.BookId==bookCart.BookId).ToListAsync();
                if (borrowedBook.Count==0)
                {
                    // borrow book
                    await _bookCartRepository.Insert(bookCart);
                    await _bookRepository.ChangeBookStatus(bookCart.BookId);
                }
                else
                {
                    await _bookRepository.ChangeBookStatus(bookCart.BookId);
                    //TODO: Notification this book has already been rented by someone
                }
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
