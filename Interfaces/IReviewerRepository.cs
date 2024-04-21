using PokemonReviewApi.Models;

namespace PokemonReviewApi.Interfaces
{
    public interface IReviewerRepository
    {
        public Task<ICollection<Reviewer>> GetAll();
        public Task<Reviewer> GetById(int id);
        public bool Create(Reviewer reviewer);
        public bool Update(Reviewer reviewer);
        public bool Delete(Reviewer reviewer);
        public bool Save();
        public bool ReviewerExists(int id);
    }
}
