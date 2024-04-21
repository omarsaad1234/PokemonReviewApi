using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApi.Dtos;
using PokemonReviewApi.Interfaces;
using PokemonReviewApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PokemonReviewApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryRepository categoryRepository,IPokemonRepository pokemonRepository,IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _pokemonRepository = pokemonRepository;
            _mapper = mapper;
        }
        // GET: api/<CategoriesController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryRepository.GetAll();
            if (!ModelState.IsValid)
                return BadRequest();
            var dataToView = _mapper.Map<ICollection<CategoryDto>>(categories);
            return Ok(dataToView);
        }

        // GET api/<CategoriesController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (!_categoryRepository.CategoryExists(id))
                return NotFound();

            var category = await _categoryRepository.GetById(id);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var dataToView = _mapper.Map<CategoryDto>(category);
            return Ok(dataToView);
        }
        [HttpGet("GetByName")]
        public async Task<IActionResult> GetByName(string name)
        {
            if (!_categoryRepository.CategoryExistsWithName(name))
                return NotFound();

            var category = await _categoryRepository.GetByName(name);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var dataToView = _mapper.Map<CategoryDto>(category);
            return Ok(dataToView);
        }

        // POST api/<CategoriesController>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryDto categoryCreate)
        {
            if (categoryCreate is null)
                return BadRequest(ModelState);

            var categories = await _categoryRepository.GetAll();

            var category = categories.Where(c => c.Name.Trim().ToUpper() == categoryCreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault();

            if(category != null)
            {
                ModelState.AddModelError("", "Category Already Exists");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoryMap = _mapper.Map<Category>(categoryCreate);

            if (!_categoryRepository.Create(categoryMap))
            {
                ModelState.AddModelError("", "Something Went Wrong While Saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Created Successfully");
        }

        // PUT api/<CategoriesController>/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] CategoryDto categoryUpdate)
        {
            if (categoryUpdate is null)
                return BadRequest(ModelState);

            if (id != categoryUpdate.Id)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_categoryRepository.CategoryExists(id))
                return NotFound();

            var categoryMap = _mapper.Map<Category>(categoryUpdate);
            if (!_categoryRepository.Update(categoryMap))
            {
                ModelState.AddModelError("", "Something Went Wrong While Updating !");
                return StatusCode(500, ModelState);
            }
            return Ok("Updated Successfully");

        }

        // DELETE api/<CategoriesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!_categoryRepository.CategoryExists(id))
                return NotFound();
            var category = await _categoryRepository.GetById(id);
            var dependents = await _pokemonRepository.GetByCategoryId(id);
            if (dependents.Count > 0)
                return BadRequest("This Category Is Assosiated To Pokemons , Please Remove These Pokemons First Or Assign Them To Another Category");
            if (!_categoryRepository.Delete(category))
            {
                ModelState.AddModelError("", "Something Went Wrong");
                return StatusCode(500, ModelState);
            }
            return Ok("Deleted Successfully");
        }
    }
}
