using Microsoft.EntityFrameworkCore;
using PokemonReviewApi.Data;
using PokemonReviewApi.Interfaces;
using PokemonReviewApi.Models;

namespace PokemonReviewApi.Repository
{
    public class ReviewRepository:IReviewRepository
    {
        private readonly AppDbContext _context;

        public ReviewRepository(AppDbContext context)
        {
            _context = context;
        }

        

        public async Task<ICollection<Review>> GetAll()
        {
            return await _context.Reviews.Include(r => r.Reviewer).ToListAsync();
        }

        public Review GetById(int id)
        {
            return _context.Reviews.Where(r => r.Id == id).Include(r=>r.Reviewer).FirstOrDefault();
        }

        public async Task<ICollection<Review>> GetByPokemonId(int pokeId)
        {
            return await _context.Reviews.Where(r => r.PokemonId == pokeId).Include(r => r.Reviewer).ToListAsync();
        }

        public async Task<ICollection<Review>> GetByReviewerId(int reviewerId)
        {
            return await _context.Reviews.Where(r => r.ReviewrId == reviewerId).Include(r => r.Reviewer).ToListAsync();
        }
        public bool Create(Review review)
        {
            _context.Reviews.Add(review);
            return Save();
        }
        public bool Update(Review review)
        {
            _context.Reviews.Update(review);
            return Save();
        }
        public bool ReviewExists(int id)
        {
            return _context.Reviews.Any(r => r.Id == id);
        }

        public bool Save()
        {
            var saves = _context.SaveChanges();
            return saves > 0 ? true : false;
        }

        public int GetReviewerId(int id)
        {
            return _context.Reviews.Where(r => r.Id == id).FirstOrDefault().ReviewrId;
        }

        public int GetPokemonId(int id)
        {
            return _context.Reviews.Where(r => r.Id == id).FirstOrDefault().PokemonId;
        }

        public bool Delete(Review review)
        {
            _context.Reviews.Remove(review);
            return Save();
        }
    }
}
