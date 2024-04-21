using PokemonReviewApi.Models;

namespace PokemonReviewApi.Interfaces
{
    public interface IOwnerRepository
    {
        public Task<ICollection<Owner>> GetAll();
        public Task<ICollection<Owner>> GetByPokemonId(int pokeId);
        public Task<ICollection<Owner>> GetByCountryId(int countryId);
        public Task<Owner> GetById(int id);
        public bool Create(Owner owner);
        public bool Update(Owner owner);
        public bool Delete(Owner owner);
        public bool Save();
        public bool OwnerExists(int id);

    }
}
