using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Data
{
    public class PersonRepository : IPersonRepository
    {
        private readonly AppDbContext _context;

        public PersonRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Person> GetAll() => _context.Persons.ToList();

        public async Task<List<Person>> GetAllAsync() => await _context.Persons.ToListAsync();

        public async Task<Person> GetByIdAsync(int id) => await _context.Persons.FindAsync(id);

        public async Task AddAsync(Person Person)
        {
            _context.Persons.Add(Person);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Person Person)
        {
            _context.Persons.Update(Person);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Person Person)
        {
            _context.Persons.Remove(Person);
            await _context.SaveChangesAsync();
        }
    }
}
