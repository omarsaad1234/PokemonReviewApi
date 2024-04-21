using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApi.Data;
using PokemonReviewApi.Dtos;
using PokemonReviewApi.Interfaces;
using PokemonReviewApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PokemonReviewApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IOwnerRepository _ownerRepository;
        private readonly IMapper _mapper;

        public CountriesController(ICountryRepository countryRepository,IOwnerRepository ownerRepository,IMapper mapper)
        {
            _countryRepository = countryRepository;
            _ownerRepository = ownerRepository;
            _mapper = mapper;
        }
        // GET: api/<CountriesController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var countries = await _countryRepository.GetAll();

            if (!ModelState.IsValid)
                return BadRequest();

            var dataToView = _mapper.Map<ICollection<CountryDto>>(countries);

            return Ok(dataToView);
        }

        // GET api/<CountriesController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (!_countryRepository.CountryExists(id))
                return NotFound();
            var country = await _countryRepository.GetById(id);
            if (!ModelState.IsValid)
                return BadRequest();
            var dataToView = _mapper.Map<CountryDto>(country);
            return Ok(dataToView);
        }
        [HttpGet("GetByOwnerId")]
        public async Task<IActionResult> GetByOwnerId(int ownerId)
        {
            var country = await _countryRepository.GetByOwnerId(ownerId);
            if (!ModelState.IsValid)
                return BadRequest();
            var dataToView = _mapper.Map<CountryDto>(country);
            return Ok(dataToView);
        }

        // POST api/<CountriesController>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CountryDto countryCreate)
        {
            if (countryCreate is null)
                return BadRequest(ModelState);

            var countries = await _countryRepository.GetAll();

            var country = countries.Where(c => c.Name.Trim().ToUpper() == countryCreate.Name.TrimEnd().ToUpper()).FirstOrDefault();

            if(country != null)
            {
                ModelState.AddModelError("", "Country Already Exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var countryMap = _mapper.Map<Country>(countryCreate);

            if (!_countryRepository.Create(countryMap))
            {
                ModelState.AddModelError("", "Something Went Wrong While Saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Created Successfully");
        }

        // PUT api/<CountriesController>/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] CountryDto countryUpdate)
        {
            if (countryUpdate is null)
                return BadRequest(ModelState);

            if (id != countryUpdate.Id)
                return BadRequest(ModelState);

            if (!_countryRepository.CountryExists(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoryMap = _mapper.Map<Country>(countryUpdate);
            if (!_countryRepository.Update(categoryMap))
            {
                ModelState.AddModelError("", "Something Went Wrong");
                return StatusCode(500, ModelState);
            }
            return Ok("Updated Successfully");

        }

        // DELETE api/<CountriesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!_countryRepository.CountryExists(id))
                return NotFound();

            var depenedents = await _ownerRepository.GetByCountryId(id);

            if (depenedents.Count > 0)
                return BadRequest("This Country Related To Another Owners Please Delete These Owners Or Assign Them To Another Country");

            var country = await _countryRepository.GetById(id);

            if (!_countryRepository.Delete(country))
            {
                ModelState.AddModelError("", "Something Went Wrong");
                return StatusCode(500, ModelState);
            }
            return Ok("Deleted Successfully");

        }
    }
}
