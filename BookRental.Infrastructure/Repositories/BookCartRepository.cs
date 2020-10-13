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
    public class BookCartRepository : IRepository<BookCart>
    {
        private readonly BookRentalDbContext _context;
        public BookCartRepository(BookRentalDbContext context)
        {
            _context = context;
        }
        public Task<BookCart> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<BookCart> GetAll()
        {
            return _context.BookCarts;
        }

        public async Task<IEnumerable<BookCart>> GetAllWithProperties(Expression<Func<BookCart, bool>> filter = null, Func<IQueryable<BookCart>, IOrderedQueryable<BookCart>> orderBy = null, string includeProperties = null)
        {
            IQueryable<BookCart> query = _context.BookCarts;

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

        public async Task<BookCart> GetFirstOrDefaultWithProperties(Expression<Func<BookCart, bool>> filter = null, string includeProperties = null)
        {
            IQueryable<BookCart> query = _context.BookCarts;

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

        public async Task Insert(BookCart entity)
        {
            await _context.BookCarts.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Update(BookCart entity)
        {
            _context.BookCarts.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(BookCart entity)
        {
            _context.BookCarts.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteById(int id)
        {
            var bookCart = await _context.BookCarts.SingleOrDefaultAsync(b => b.Id == id);
            _context.BookCarts.Remove(bookCart);
            await _context.SaveChangesAsync();
        }
    }
}
