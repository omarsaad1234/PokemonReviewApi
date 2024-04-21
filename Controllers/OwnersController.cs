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
    public class OwnersController : ControllerBase
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly IMapper _mapper;
        private readonly IPokemonRepository _pokemonRepository;
        private readonly ICountryRepository _countryRepository;

        public OwnersController(IOwnerRepository ownerRepository,IMapper mapper,IPokemonRepository pokemonRepository,ICountryRepository countryRepository)
        {
            _ownerRepository = ownerRepository;
            _mapper = mapper;
            _pokemonRepository = pokemonRepository;
            _countryRepository = countryRepository;
        }
        // GET: api/<OwnersController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var owners = await _ownerRepository.GetAll();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var dataToView = _mapper.Map<ICollection<OwnerDto>>(owners);
            return Ok(dataToView);
        }
        [HttpGet("GetByPokemonId")]
        public async Task<IActionResult> GetByPokemonId(int pokeId)
        {
            if (!_pokemonRepository.PokemonExists(pokeId))
                return NotFound();
            var owners = await _ownerRepository.GetByPokemonId(pokeId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var dataToView = _mapper.Map<ICollection<OwnerDto>>(owners);
            return Ok(dataToView);
        }
        [HttpGet("GetByCountryId")]
        public async Task<IActionResult> GetByCountryId(int countryId)
        {
            if (!_countryRepository.CountryExists(countryId))
                return NotFound();
            var owners = await _ownerRepository.GetByCountryId(countryId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var dataToView = _mapper.Map<ICollection<OwnerDto>>(owners);
            return Ok(dataToView);
        }

        // GET api/<OwnersController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (!_ownerRepository.OwnerExists(id))
                return NotFound();
            var owner = await _ownerRepository.GetById(id);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var dataToView = _mapper.Map<OwnerDto>(owner);
            return Ok(dataToView);
        }

        // POST api/<OwnersController>
        [HttpPost]
        public async Task<IActionResult> Create([FromQuery] int countryId,[FromBody] OwnerDto ownerCreate)
        {
            if (ownerCreate is null)
                return BadRequest(ModelState);

            var owners = await _ownerRepository.GetAll();

            var owner = owners.Where(c => c.LastName.Trim().ToUpper() == ownerCreate.LastName.TrimEnd().ToUpper()).FirstOrDefault();

            if (owner != null)
            {
                ModelState.AddModelError("", "Owner Already Exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if(!_countryRepository.CountryExists(countryId))
                return NotFound();

            var ownerMap = _mapper.Map<Owner>(ownerCreate);
            ownerMap.CountryId = countryId;

            if (!_ownerRepository.Create(ownerMap))
            {
                ModelState.AddModelError("", "Something Went Wrong While Saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Created Successfully");
        }

        // PUT api/<OwnersController>/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromQuery] int countryId, [FromBody] OwnerDto ownerUpdate)
        {
            if (ownerUpdate is null)
                return BadRequest(ModelState);
            if (id != ownerUpdate.Id)
                return BadRequest(ModelState);
            if (!_ownerRepository.OwnerExists(id))
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_countryRepository.CountryExists(countryId))
                return BadRequest("Not Valid Country Id");
            var ownerMap = _mapper.Map<Owner>(ownerUpdate);
            ownerMap.CountryId = countryId;
            if (!_ownerRepository.Update(ownerMap))
            {
                ModelState.AddModelError("", "Something Went Wrong");
                return StatusCode(500, ModelState);
            }
            return Ok("Updated Successfully");
        }

        // DELETE api/<OwnersController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!_ownerRepository.OwnerExists(id))
                return NotFound();
            var dependents = await _pokemonRepository.GetByOwnerId(id);
            if (dependents.Count > 0)
                return BadRequest("This Owner Related To Another Pokemons Please Delete These Pokemons Or Assign Them To Another Owner");
            var owner = await _ownerRepository.GetById(id);
            if (!_ownerRepository.Delete(owner))
            {
                ModelState.AddModelError("", "Something Went Wrong");
                return StatusCode(500, ModelState);

            }
            return Ok("Deleted Successfully");

        }
    }
}
