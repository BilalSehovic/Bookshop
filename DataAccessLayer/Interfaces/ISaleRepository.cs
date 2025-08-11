using DataAccessLayer.Models;

namespace DataAccessLayer.Interfaces;

public interface ISaleRepository
{
    Task AddAsync(Sale sale);
    Task<List<Sale>> GetAllAsync();
    Task<List<Sale>> GetSalesByDateAsync(DateTime date);
    Task<List<Sale>> GetSalesForDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<decimal> GetTotalSalesForDateAsync(DateTime date);
}
