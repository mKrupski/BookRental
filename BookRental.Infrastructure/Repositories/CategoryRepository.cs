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
    public class CategoryRepository : IRepository<Category>
    {
        private readonly BookRentalDbContext _context;

        public CategoryRepository(BookRentalDbContext context)
        {
            _context = context;
        }

        public async Task<Category> GetById(int id)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Category>> GetAllWithProperties(Expression<Func<Category, bool>> filter = null, Func<IQueryable<Category>, IOrderedQueryable<Category>> orderBy = null, string includeProperties = null)
        {
            IQueryable<Category> query = _context.Categories;

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

        public async Task<Category> GetFirstOrDefaultWithProperties(Expression<Func<Category, bool>> filter = null, string includeProperties = null)
        {
            IQueryable<Category> query = _context.Categories;

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

        public IQueryable<Category> GetAll()
        {
            return _context.Categories;
        }

        public async Task Insert(Category entity)
        {
            await _context.Categories.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Category entity)
        {
            _context.Categories.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Category entity)
        {
            _context.Categories.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteById(int id)
        {
            var category = await _context.Categories.SingleOrDefaultAsync(c => c.Id == id);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}
