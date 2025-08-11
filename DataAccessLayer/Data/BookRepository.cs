using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Data
{
    public class BookRepository : IBookRepository
    {
        private readonly AppDbContext _context;

        public BookRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Book> GetAll() => _context.Books.ToList();

        public async Task<List<Book>> GetAllAsync() => await _context.Books.ToListAsync();

        public async Task<Book?> GetByIdAsync(Guid id) => await _context.Books.FindAsync(id);

        public async Task AddAsync(Book Book)
        {
            _context.Books.Add(Book);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Book Book)
        {
            _context.Books.Update(Book);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Book Book)
        {
            _context.Books.Remove(Book);
            await _context.SaveChangesAsync();
        }
    }
}
