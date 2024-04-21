using Microsoft.EntityFrameworkCore;
using PokemonReviewApi.Data;
using PokemonReviewApi.Interfaces;
using PokemonReviewApi.Models;

namespace PokemonReviewApi.Repository
{
    public class ReviewerRepository : IReviewerRepository
    {
        private readonly AppDbContext _context;

        public ReviewerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Reviewer>> GetAll()
        {
            return await _context.Reviewers.ToListAsync();
        }

        public async Task<Reviewer> GetById(int id)
        {
            return await _context.Reviewers.Where(r => r.Id == id).FirstOrDefaultAsync();
        }
        public bool Create(Reviewer reviewer)
        {
            _context.Reviewers.Add(reviewer);
            return Save();
        }
        public bool Update(Reviewer reviewer)
        {
            _context.Reviewers.Update(reviewer);
            return Save();
        }
        public bool ReviewerExists(int id)
        {
            return _context.Reviewers.Any(r => r.Id == id);
        }

        public bool Save()
        {
            var saves = _context.SaveChanges();
            return saves > 0 ? true : false;
        }

        public bool Delete(Reviewer reviewer)
        {
            _context.Reviewers.Remove(reviewer);
            return Save();
        }
    }
}
