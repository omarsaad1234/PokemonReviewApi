using Microsoft.EntityFrameworkCore;
using PokemonReviewApi.Data;
using PokemonReviewApi.Interfaces;
using PokemonReviewApi.Models;

namespace PokemonReviewApi.Repository
{
    public class OwnerRepository:IOwnerRepository
    {
        private readonly AppDbContext _context;

        public OwnerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Owner>> GetAll()
        {
            return await _context.Owners.ToListAsync();
        }

        public async Task<ICollection<Owner>> GetByCountryId(int countryId)
        {
            return await _context.Owners.Where(o => o.CountryId == countryId).ToListAsync();
        }

        public async Task<Owner> GetById(int id)
        {
            return await _context.Owners.Where(o => o.Id == id).FirstOrDefaultAsync();
        }

        public async Task<ICollection<Owner>> GetByPokemonId(int pokeId)
        {
            return await _context.PokemonOwners.Where(po=>po.PokemonId==pokeId).Select(o=>o.Owner).ToListAsync();
        }
        public bool Create(Owner owner)
        {
            _context.Owners.Add(owner);
            return Save();
        }
        public bool Update(Owner owner)
        {
            _context.Owners.Update(owner);
            return Save();
        }

        public bool Save()
        {
            var saves = _context.SaveChanges();
            return saves > 0 ? true : false;
        }

        public bool OwnerExists(int id)
        {
            return _context.Owners.Any(o => o.Id == id);
        }

        public bool Delete(Owner owner)
        {
            _context.Owners.Remove(owner);
            return Save();
        }
    }
}
