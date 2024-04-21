using PokemonReviewApi.Models;

namespace PokemonReviewApi.Interfaces
{
    public interface IReviewRepository
    {
        public Task<ICollection<Review>> GetAll();
        public Task<ICollection<Review>> GetByReviewerId(int reviewerId);
        public Task<ICollection<Review>> GetByPokemonId(int pokeId);
        public Review GetById(int id);
        public bool Create(Review review);
        public bool Update(Review review);
        public bool Delete(Review review);
        public bool Save();
        public bool ReviewExists(int id);
        public int GetReviewerId(int id);
        public int GetPokemonId(int id);
    }
}
