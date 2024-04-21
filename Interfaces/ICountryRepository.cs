using PokemonReviewApi.Models;
using System.Diagnostics.Metrics;

namespace PokemonReviewApi.Interfaces
{
    public interface ICountryRepository
    {
        public Task<ICollection<Country>> GetAll();
        public Task<Country> GetById(int id);
        public Task<Country> GetByOwnerId(int ownerId);

        public bool Create(Country country);
        public bool Update(Country country);
        public bool Delete(Country country);
        public bool Save();
        public bool CountryExists(int id);
    }
}
