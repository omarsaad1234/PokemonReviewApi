using Microsoft.EntityFrameworkCore;
using PokemonReviewApi.Data;
using PokemonReviewApi.Interfaces;
using PokemonReviewApi.Models;

namespace PokemonReviewApi.Repository
{
    public class CategoryRepository:ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        
        public async Task<ICollection<Category>> GetAll()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetById(int id)
        {
            return await _context.Categories.Where(c=>c.Id==id).FirstOrDefaultAsync();
        }

        public async Task<Category> GetByName(string name)
        {
            return await _context.Categories.Where(c => c.Name == name).FirstOrDefaultAsync();
        }
        public bool CategoryExists(int id)
        {
            return _context.Categories.Any(c => c.Id == id);
        }

        public bool CategoryExistsWithName(string name)
        {
            return _context.Categories.Any(c => c.Name == name);
        }

        public bool Create(Category category)
        {
            _context.Categories.Add(category);
            return Save();
        }
        public bool Update(Category category)
        {
            _context.Categories.Update(category);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Delete(Category category)
        {
            _context.Categories.Remove(category);
            return Save();
        }
    }
}
