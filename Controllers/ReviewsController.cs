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
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IMapper _mapper;

        public ReviewsController(IReviewRepository reviewRepository,IPokemonRepository pokemonRepository,IReviewerRepository reviewerRepository,IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _pokemonRepository = pokemonRepository;
            _reviewerRepository = reviewerRepository;
            _mapper = mapper;
        }
        // GET: api/<ReviewsController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var reviews = await _reviewRepository.GetAll();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var dataToView = _mapper.Map<ICollection<ReviewDto>>(reviews);

            return Ok(dataToView);
        }

        // GET api/<ReviewsController>/5
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            if (!_reviewRepository.ReviewExists(id))
                return NotFound();

            var review = _reviewRepository.GetById(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var dataToView = _mapper.Map<ReviewDto>(review);

            return Ok(dataToView);
        }
        [HttpGet("GetByReviewerId")]
        public async Task<IActionResult> GetByReviewerId(int reviewerId)
        {
            if (!_reviewerRepository.ReviewerExists(reviewerId))
                return NotFound();

            var reviews = await _reviewRepository.GetByReviewerId(reviewerId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var dataToView = _mapper.Map<ICollection<ReviewDto>>(reviews);

            return Ok(dataToView);
        }
        [HttpGet("GetByPokemonId")]
        public async Task<IActionResult> GetByPokemonId(int pokeId)
        {
            if (!_pokemonRepository.PokemonExists(pokeId))
                return NotFound();

            var reviews = await _reviewRepository.GetByPokemonId(pokeId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var dataToView = _mapper.Map<ICollection<ReviewDto>>(reviews);

            return Ok(dataToView);
        }

        // POST api/<ReviewsController>
        [HttpPost]
        public IActionResult Create([FromQuery] int pokeId, [FromQuery] int reviewerId,[FromBody] ReviewDto reviewCreate)
        {
            if (reviewCreate is null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if(!_pokemonRepository.PokemonExists(pokeId)||!_reviewerRepository.ReviewerExists(reviewerId))
                return BadRequest("Not Valid Pokemon Or Reviewer Id");

            var reviewMap = _mapper.Map<Review>(reviewCreate);
            reviewMap.ReviewrId = reviewerId;
            reviewMap.PokemonId = pokeId;
            if (!_reviewRepository.Create(reviewMap))
            {
                ModelState.AddModelError("", "Something Went Wrong While Saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Created Successfully");


        }

        // PUT api/<ReviewsController>/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromQuery] int reviewerId
            , [FromQuery] int pokeId, [FromBody] ReviewDto reviewUpdate)
        {
            if (reviewUpdate is null)
                return BadRequest(ModelState);

            if (id != reviewUpdate.Id)
                return BadRequest(ModelState);

            if (!_reviewRepository.ReviewExists(id))
                return NotFound();
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_pokemonRepository.PokemonExists(pokeId) || !_reviewerRepository.ReviewerExists(reviewerId))
                return BadRequest("Invalid Pokemon Or Reviewer Id");

            var reviewMap = _mapper.Map<Review>(reviewUpdate);
            reviewMap.PokemonId = pokeId;
            reviewMap.ReviewrId = reviewerId;
            if (!_reviewRepository.Update(reviewMap))
            {
                ModelState.AddModelError("", "Something Went Wrong ");
                return StatusCode(500, ModelState);
            }
            return Ok("Updated Successfully");
        }

        // DELETE api/<ReviewsController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!_reviewRepository.ReviewExists(id))
                return NotFound();
            var review = _reviewRepository.GetById(id);
            if (!_reviewRepository.Delete(review))
            {
                ModelState.AddModelError("", "Something Went Wrong");
                return StatusCode(500, ModelState);

            }
            return Ok("Deleted Successfully");
        }
    }
}
