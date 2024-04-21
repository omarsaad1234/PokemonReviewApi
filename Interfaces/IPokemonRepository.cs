using PokemonReviewApi.Models;

namespace PokemonReviewApi.Interfaces
{
    public interface IPokemonRepository
    {
        public Task<ICollection<Pokemon>> GetAll();
        public Task<ICollection<Pokemon>> GetByCategoryId(int catId);
        public Task<ICollection<Pokemon>> GetByOwnerId(int ownerId);
        public Task<Pokemon> GetById(int id);

        public Task<Pokemon> GetByName(string name);

        public decimal GetPokemonRating(int pokeId);
        public bool Create(int ownerId,int categoryId,Pokemon pokemon);
        public bool Update(int ownerId, int categoryId, Pokemon pokemon);
        public bool Delete( Pokemon pokemon);
        public bool Save();
        public bool PokemonExists(int id);
        public bool PokemonExistsWithName(string name);
    }
}
