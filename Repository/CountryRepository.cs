using Microsoft.EntityFrameworkCore;
using PokemonReviewApi.Data;
using PokemonReviewApi.Interfaces;
using PokemonReviewApi.Models;

namespace PokemonReviewApi.Repository
{
    public class CountryRepository:ICountryRepository
    {
        private readonly AppDbContext _context;

        public CountryRepository(AppDbContext context)
        {
            _context = context;
        }

        

        public async Task<ICollection<Country>> GetAll()
        {
            return await _context.Countries.ToListAsync();
        }

        public async Task<Country> GetById(int id)
        {
            return await _context.Countries.Where(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Country> GetByOwnerId(int ownerId)
        {
            return await _context.Owners.Where(o => o.Id == ownerId).Select(o => o.Country).FirstOrDefaultAsync();
        }
        public bool CountryExists(int id)
        {
            return _context.Countries.Any(c => c.Id == id);
        }

        public bool Create(Country country)
        {
            _context.Countries.Add(country);
            return Save();
        }
        public bool Update(Country country)
        {
            _context.Countries.Update(country);
            return Save();
        }
        public bool Save()
        {
            var saves = _context.SaveChanges();
            return saves > 0 ? true : false;
        }

        public bool Delete(Country country)
        {
            _context.Countries.Remove(country);
            return Save();
        }
    }
}
