using System.Collections.Generic;
using System.Threading.Tasks;
using BookRental.Domain.Models;

namespace BookRental.Infrastructure.Repositories.IRepository
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<IEnumerable<Book>> GetBooksToRent();
        Task<IEnumerable<Book>> GetBooksOnLoan();
        Task ChangeBookStatus(int bookCartBookId);
    }
}
