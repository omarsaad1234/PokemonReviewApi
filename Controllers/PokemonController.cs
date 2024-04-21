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
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IOwnerRepository _ownerRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;

        public PokemonController(IPokemonRepository pokemonRepository
            ,ICategoryRepository categoryRepository
            ,IOwnerRepository ownerRepository
            ,IReviewRepository reviewRepository
            ,IMapper mapper)
        {
            _pokemonRepository = pokemonRepository;
            _categoryRepository = categoryRepository;
            _ownerRepository = ownerRepository;
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }
        // GET: api/<PokemonController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var pokemons = await _pokemonRepository.GetAll();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var dataToView = _mapper.Map<ICollection<PokemonDto>>(pokemons);
            return Ok(dataToView);
        }

        // GET api/<PokemonController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (!_pokemonRepository.PokemonExists(id))
                return NotFound();

            var pokemon = await _pokemonRepository.GetById(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var dataToView = _mapper.Map<PokemonDto>(pokemon);

            return Ok(dataToView);

        }
        [HttpGet("GetByName")]
        public async Task<IActionResult> GetByName(string name)
        {
            if (!_pokemonRepository.PokemonExistsWithName(name))
                return NotFound();

            var pokemon = await _pokemonRepository.GetByName(name);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var dataToView = _mapper.Map<PokemonDto>(pokemon);

            return Ok(dataToView);

        }
        [HttpGet("{id}/rating")]
        public IActionResult GetPokemonRating(int id)
        {
            if (!_pokemonRepository.PokemonExists(id))
                return NotFound();

            var pokemonRating = _pokemonRepository.GetPokemonRating(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemonRating);

        }

        // POST api/<PokemonController>
        [HttpPost]
        public async Task<IActionResult> Create([FromQuery] int categoryId, [FromQuery] int ownerId ,[FromBody] PokemonDto pokemonCreate)
        {
            if (pokemonCreate is null)
                return BadRequest(ModelState);

            var pokemons = await _pokemonRepository.GetAll();

            var pokemon = pokemons.Where(c => c.Name.Trim().ToUpper() == pokemonCreate.Name.TrimEnd().ToUpper()).FirstOrDefault();

            if (pokemon != null)
            {
                ModelState.AddModelError("", "Pokemon Already Exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_categoryRepository.CategoryExists(categoryId)||!_ownerRepository.OwnerExists(ownerId))
                return BadRequest("Not Valid Category Or Owner Id");

            var pokemonMap = _mapper.Map<Pokemon>(pokemonCreate);

            if (!_pokemonRepository.Create(ownerId,categoryId, pokemonMap))
            {
                ModelState.AddModelError("", "Something Went Wrong While Saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Created Successfully");
        }

        // PUT api/<PokemonController>/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromQuery] int ownerId,
            [FromQuery] int catId, [FromBody] PokemonDto pokemonUpdate)
        {
            if (pokemonUpdate is null)
                return BadRequest(ModelState);
            
            if (id != pokemonUpdate.Id)
                return BadRequest(ModelState);
            
            if (!_pokemonRepository.PokemonExists(id))
                return NotFound();
            if (!_ownerRepository.OwnerExists(ownerId)||!_categoryRepository.CategoryExists(catId))
                return BadRequest("Invalid Category Or Owner Id");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pokemonMap = _mapper.Map<Pokemon>(pokemonUpdate);
            if (!_pokemonRepository.Update(ownerId, catId, pokemonMap))
            {
                ModelState.AddModelError("", "Something Went Wrong");
                return StatusCode(500, ModelState);
            }
            return Ok("Updated Successfully");

        }

        // DELETE api/<PokemonController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!_pokemonRepository.PokemonExists(id))
                return NotFound();

            var dependents = await _reviewRepository.GetByPokemonId(id);
            if (dependents.Count > 0)
                return BadRequest("This Pokemon Related To Another Reviews Please Remove These Reviews Or Assign Them To Another Pokemon ");
            var pokemon = await _pokemonRepository.GetById(id);
            if (!_pokemonRepository.Delete(pokemon))
            {
                ModelState.AddModelError("", "Something Went Wrong");
                return StatusCode(500, ModelState);
            }
            return Ok("Deleted Successfully");
        }
    }
}
