using DataAccessLayer;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace WpfApp.Services;

public interface IBookService
{
    Task<List<Book>> GetAllBooksAsync();
    Task<Book?> GetBookByIdAsync(Guid id);
    Task<Book> AddBookAsync(Book book);
    Task<Book> UpdateBookAsync(Book book);
    Task<bool> DeleteBookAsync(Guid id);
    Task<bool> SellBookAsync(Guid bookId, double unitPrice, int quantity = 1);
    Task<List<Sale>> GetSalesByDateAsync(DateTime date);
}

public class BookService : IBookService
{
    private readonly AppDbContext _context;

    public BookService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Book>> GetAllBooksAsync()
    {
        return await _context
            .Books.OrderByDescending(b => b.CreatedAt)
            .ThenBy(b => b.Title)
            .ToListAsync();
    }

    public async Task<Book?> GetBookByIdAsync(Guid id)
    {
        return await _context.Books.FindAsync(id);
    }

    public async Task<Book> AddBookAsync(Book book)
    {
        _context.Books.Add(book);
        await _context.SaveChangesAsync();
        return book;
    }

    public async Task<Book> UpdateBookAsync(Book book)
    {
        _context.Books.Update(book);
        await _context.SaveChangesAsync();
        return book;
    }

    public async Task<bool> DeleteBookAsync(Guid id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null)
            return false;

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> SellBookAsync(Guid bookId, double unitPrice, int quantity = 1)
    {
        var book = await _context.Books.FindAsync(bookId);
        if (book == null || book.StockQuantity < quantity || quantity <= 0)
            return false;

        book.StockQuantity -= quantity;

        var sale = new Sale
        {
            BookId = bookId,
            UnitPrice = unitPrice,
            SaleDate = DateTime.UtcNow,
            Quantity = quantity,
            BookDescription = $"{book.Title} by {book.Author} ({book.Isbn})",
        };

        _context.Sales.Add(sale);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Sale>> GetSalesByDateAsync(DateTime date)
    {
        var startDate = DateTime.SpecifyKind(date.Date, DateTimeKind.Utc);
        var endDate = startDate.AddDays(1);

        return await _context
            .Sales.Include(s => s.Book)
            .Where(s => s.SaleDate >= startDate && s.SaleDate < endDate)
            .OrderBy(s => s.SaleDate)
            .ToListAsync();
    }
}
