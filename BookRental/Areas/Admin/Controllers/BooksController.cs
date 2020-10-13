using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BookRental.App.Config;
using BookRental.App.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookRental.Domain.Models;
using BookRental.Infrastructure.Repositories.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;

namespace BookRental.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDependencies.Role_Admin)]
    public class BooksController : Controller
    {
        private readonly IRepository<Book> _bookRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IWebHostEnvironment _hostEnvironment;

        public BooksController(IRepository<Book> bookRepository, IWebHostEnvironment hostEnvironment, IRepository<Category> categoryRepository)
        {
            _bookRepository = bookRepository;
            _hostEnvironment = hostEnvironment;
            _categoryRepository = categoryRepository;
        }

        // GET: Admin/Books
        public async Task<IActionResult> Index()
        {
            return View(await _bookRepository.GetAllWithProperties(includeProperties: "Category"));
        }


        // GET: Admin/Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _bookRepository.GetById(id.Value);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        public async Task<IActionResult> Create(int? id)
        {
            IEnumerable<Category> catList = await _categoryRepository.GetAll().ToListAsync();
            BookVm bookVm = new BookVm()
            {
                Book = new Book(),
                CategoryList = catList.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };
            if (id == null)
            {
                //CREATE
                return View(bookVm);
            }
            //EDIT
            bookVm.Book = await _bookRepository.GetById(id.GetValueOrDefault());
            if (bookVm.Book == null)
            {
                return NotFound();
            }
            return View(bookVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookVm bookVm)
        {
            if (ModelState.IsValid)
            {
                string webRootPath = _hostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"images\books");
                    var extenstion = Path.GetExtension(files[0].FileName);

                    if (bookVm.Book.ImageUrl != null)
                    {
                        //EDIT IMAGE
                        var imagePath = Path.Combine(webRootPath, bookVm.Book.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }
                    using (var filesStreams = new FileStream(Path.Combine(uploads, fileName + extenstion), FileMode.Create))
                    {
                        files[0].CopyTo(filesStreams);
                    }
                    bookVm.Book.ImageUrl = @"\images\books\" + fileName + extenstion;
                }
                else
                {
                    //UPDATE WHEN WE DON'T CHANGE AN IMAGE
                    if (bookVm.Book.Id != 0)
                    {
                        var objFromDb = _bookRepository.GetById(bookVm.Book.Id).Result;
                        bookVm.Book.ImageUrl = objFromDb.ImageUrl;
                    }
                }
                if (bookVm.Book.Id == 0)
                {
                    await _bookRepository.Insert(bookVm.Book);
                }
                else
                {
                    await _bookRepository.Update(bookVm.Book);
                }
                return RedirectToAction(nameof(Index));
            }

            IEnumerable<Category> catList = await _categoryRepository.GetAll().ToListAsync();
            bookVm.CategoryList = catList.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            if (bookVm.Book.Id != 0)
            {
                bookVm.Book = await _bookRepository.GetById(bookVm.Book.Id);
            }
            return View(bookVm);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _bookRepository.GetById(id.Value);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Admin/Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _bookRepository.GetById(id);
            await _bookRepository.Delete(book);
            return RedirectToAction(nameof(Index));
        }
    }
}
