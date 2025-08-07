using DataAccessLayer.Models;

namespace DataAccessLayer.Data
{
    public interface IBookRepository
    {
        Task AddAsync(Book Book);
        Task DeleteAsync(Book Book);
        List<Book> GetAll();
        Task<List<Book>> GetAllAsync();
        Task<Book> GetByIdAsync(int id);
        Task UpdateAsync(Book Book);
    }
}