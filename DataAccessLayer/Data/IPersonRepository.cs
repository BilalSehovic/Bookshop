using DataAccessLayer.Models;

namespace DataAccessLayer.Data
{
    public interface IPersonRepository
    {
        List<Person> GetAll();
        Task AddAsync(Person Person);
        Task DeleteAsync(Person Person);
        Task<List<Person>> GetAllAsync();
        Task<Person> GetByIdAsync(int id);
        Task UpdateAsync(Person Person);
    }
}
