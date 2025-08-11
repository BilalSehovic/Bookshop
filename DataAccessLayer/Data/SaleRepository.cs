using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Data;

public class SaleRepository : ISaleRepository
{
    private readonly AppDbContext _context;

    public SaleRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Sale sale)
    {
        _context.Sales.Add(sale);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Sale>> GetAllAsync()
    {
        return await _context
            .Sales.Include(s => s.Book)
            .OrderByDescending(s => s.SaleDate)
            .ToListAsync();
    }

    public async Task<List<Sale>> GetSalesByDateAsync(DateTime date)
    {
        var startOfDay = date.Date;
        var endOfDay = startOfDay.AddDays(1);

        return await _context
            .Sales.Include(s => s.Book)
            .Where(s => s.SaleDate >= startOfDay && s.SaleDate < endOfDay)
            .OrderByDescending(s => s.SaleDate)
            .ToListAsync();
    }

    public async Task<List<Sale>> GetSalesForDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context
            .Sales.Include(s => s.Book)
            .Where(s => s.SaleDate >= startDate && s.SaleDate <= endDate)
            .OrderByDescending(s => s.SaleDate)
            .ToListAsync();
    }

    public async Task<decimal> GetTotalSalesForDateAsync(DateTime date)
    {
        var startOfDay = date.Date;
        var endOfDay = startOfDay.AddDays(1);

        return (decimal)(
            await _context
                .Sales.Where(s => s.SaleDate >= startOfDay && s.SaleDate < endOfDay)
                .SumAsync(s => s.TotalPrice)
        );
    }
}
