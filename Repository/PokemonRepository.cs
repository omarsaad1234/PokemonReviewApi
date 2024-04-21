using Microsoft.EntityFrameworkCore;
using PokemonReviewApi.Data;
using PokemonReviewApi.Interfaces;
using PokemonReviewApi.Models;

namespace PokemonReviewApi.Repository
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly AppDbContext _context;

        public PokemonRepository(AppDbContext context)
        {
            _context = context;
        }

        public bool Create(int ownerId, int categoryId, Pokemon pokemon)
        {
            var owner = _context.Owners.Where(o => o.Id == ownerId).FirstOrDefault();

            var category = _context.Categories.Where(c => c.Id == categoryId).FirstOrDefault();

            var pokemonCategory = new PokemonCategory
            {
                Category = category,
                Pokemon = pokemon
            };

            _context.PokemonCategories.Add(pokemonCategory);

            var pokemonOwner = new PokemonOwner
            {
                Owner = owner,
                Pokemon = pokemon
            };

            _context.PokemonOwners.Add(pokemonOwner);

            _context.Pokemon.Add(pokemon);

            return Save();
        }
        public bool Update(int ownerId, int categoryId, Pokemon pokemon)
        {
            var owner = _context.Owners.Where(o => o.Id == ownerId).FirstOrDefault();

            var category = _context.Categories.Where(c => c.Id == categoryId).FirstOrDefault();

            var pokemonCategory = _context.PokemonCategories
                .Where(p=>p.PokemonId == pokemon.Id).FirstOrDefault();

            var pokemonOwner = _context.PokemonOwners
                .Where(p => p.PokemonId == pokemon.Id).FirstOrDefault();

            _context.PokemonCategories.Remove(pokemonCategory);
            _context.PokemonOwners.Remove(pokemonOwner);
            _context.SaveChanges();

            var pokemonNewCategory = new PokemonCategory
            {
                Category = category,
                Pokemon = pokemon
            };
            var pokemonNewOwner = new PokemonOwner
            {
                Owner = owner,
                Pokemon = pokemon
            };

            _context.PokemonCategories.Add(pokemonNewCategory);
            _context.PokemonOwners.Add(pokemonNewOwner);

            _context.Pokemon.Update(pokemon);

            return Save();
        }
        public async Task<ICollection<Pokemon>> GetAll()
        {
            return await _context.Pokemon.ToListAsync();
        }

        public async Task<Pokemon> GetById(int id)
        {
            return await _context.Pokemon.Where(p=>p.Id==id).FirstOrDefaultAsync();
        }

        public async Task<Pokemon> GetByName(string name)
        {
            return await _context.Pokemon.Where(p => p.Name == name).FirstOrDefaultAsync();
        }

        public decimal GetPokemonRating(int pokeId)
        {
            var review = _context.Reviews.Where(r => r.PokemonId == pokeId);
            if (review.Count() <= 0)
                return 0;
            return ((decimal)review.Sum(r => r.Rating) / review.Count());
        }

        public bool PokemonExists(int id)
        {
            return _context.Pokemon.Any(p => p.Id == id);
        }

        public bool PokemonExistsWithName(string name)
        {
            return _context.Pokemon.Any(p => p.Name == name);
        }

        public bool Save()
        {
            var saves = _context.SaveChanges();
            return saves > 0 ? true : false;
        }

        public async Task<ICollection<Pokemon>> GetByCategoryId(int catId)
        {
            return await _context.PokemonCategories.Where(p => p.CategoryId == catId).Select(x => x.Pokemon).ToListAsync();
        }

        public async Task<ICollection<Pokemon>> GetByOwnerId(int ownerId)
        {
            return await _context.PokemonOwners.Where(p => p.OwnerId == ownerId).Select(x => x.Pokemon).ToListAsync();
        }

        public bool Delete(Pokemon pokemon)
        {
            _context.Pokemon.Remove(pokemon);
            _context.PokemonCategories.RemoveRange(_context.PokemonCategories.Where(p => p.PokemonId == pokemon.Id).ToList());
            _context.PokemonOwners.RemoveRange(_context.PokemonOwners.Where(p => p.PokemonId == pokemon.Id).ToList());
            return Save();
        }
    }
}
