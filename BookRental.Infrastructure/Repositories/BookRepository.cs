using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BookRental.Domain.Models;
using BookRental.Infrastructure.Context;
using BookRental.Infrastructure.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BookRental.Infrastructure.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly BookRentalDbContext _context;

        public BookRepository(BookRentalDbContext context)
        {
            _context = context;
        }

        public IQueryable<Book> GetAll()
        {
            return _context.Books;
        }

        public async Task<IEnumerable<Book>> GetAllWithProperties(Expression<Func<Book, bool>> filter = null,
            Func<IQueryable<Book>, IOrderedQueryable<Book>> orderBy = null, string includeProperties = null)
        {
            IQueryable<Book> query = _context.Books;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            return await query.ToListAsync();
        }

        public async Task<Book> GetById(int id)
        {
            return await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Book> GetFirstOrDefaultWithProperties(Expression<Func<Book, bool>> filter = null,
            string includeProperties = null)
        {
            IQueryable<Book> query = _context.Books;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' },
                    StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task Insert(Book entity)
        {
            await _context.Books.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Book book)
        {
            var dbBook = _context.Books.FirstOrDefaultAsync(b => b.Id == book.Id).Result;
            if (dbBook != null)
            {
                if (book.ImageUrl != null)
                {
                    dbBook.ImageUrl = book.ImageUrl;
                }
                dbBook.Title = book.Title;
                dbBook.CategoryId = book.CategoryId;
                dbBook.Author = book.Author;
                dbBook.Status = book.Status;
                dbBook.Publisher = book.Publisher;
                dbBook.Description = book.Description;
                await _context.SaveChangesAsync();
            }
        }

        public async Task ChangeBookStatus(int bookCartBookId)
        {
            var dbBook = _context.Books.FirstOrDefaultAsync(b => b.Id == bookCartBookId).Result;
            if (dbBook.Status == BookStatus.ToRent)
                dbBook.Status = BookStatus.OnLoan;
            else
                dbBook.Status = BookStatus.ToRent;

            await _context.SaveChangesAsync();
        }

        public async Task Delete(Book entity)
        {
            _context.Books.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteById(int id)
        {
            var book = await _context.Books.SingleOrDefaultAsync(b => b.Id == id);
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksToRent()
        {
            return await GetAll().Where(b => b.Status == BookStatus.ToRent).ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksOnLoan()
        {
            return await GetAll().Where(b => b.Status == BookStatus.OnLoan).ToListAsync();
        }
    }
}