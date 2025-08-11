using DataAccessLayer.Models;

namespace DataAccessLayer.Interfaces
{
    public interface IBookRepository
    {
        Task AddAsync(Book Book);
        Task DeleteAsync(Book Book);
        List<Book> GetAll();
        Task<List<Book>> GetAllAsync();
        Task<Book?> GetByIdAsync(Guid id);
        Task UpdateAsync(Book Book);
    }
}
