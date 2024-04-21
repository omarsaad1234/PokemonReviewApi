using PokemonReviewApi.Models;

namespace PokemonReviewApi.Interfaces
{
    public interface ICategoryRepository
    {
        public Task<ICollection<Category>> GetAll();
        public Task<Category> GetById(int id);
        public Task<Category> GetByName(string name);
        public bool Create(Category category);
        public bool Update(Category category);
        public bool Delete(Category category);
        public bool CategoryExists(int id);
        public bool CategoryExistsWithName(string name);
        public bool Save();
    }
}
