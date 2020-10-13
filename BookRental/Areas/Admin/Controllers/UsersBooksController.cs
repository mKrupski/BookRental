using System.Threading.Tasks;
using BookRental.App.Config;
using BookRental.App.ViewModels;
using BookRental.Domain.Models;
using BookRental.Infrastructure.Repositories.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookRental.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDependencies.Role_Admin)]
    public class UsersBooksController : Controller
    {
        public UsersBooksVm UsersBooksVm { get; set; }
        private readonly IRepository<BookCart> _bookCartRepository;
        private readonly IBookRepository _bookRepository;


        public UsersBooksController(IRepository<BookCart> bookCartRepository, IBookRepository bookRepository)
        {
            _bookCartRepository = bookCartRepository;
            _bookRepository = bookRepository;
        }

        public async Task<IActionResult> Index()
        {
            UsersBooksVm = new UsersBooksVm();
            {

                UsersBooksVm.BorrowedBookList =
                    await _bookCartRepository.GetAllWithProperties(includeProperties: "Book");
                UsersBooksVm.Users = await _bookCartRepository.GetAllWithProperties(includeProperties: "ApplicationUser");
            };
            return View(UsersBooksVm);
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
