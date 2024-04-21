using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApi.Dtos;
using PokemonReviewApi.Interfaces;
using PokemonReviewApi.Models;
using System.Collections;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PokemonReviewApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewersController : ControllerBase
    {
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;

        public ReviewersController(IReviewerRepository reviewerRepository
            ,IReviewRepository reviewRepository
            , IMapper mapper)
        {
            _reviewerRepository = reviewerRepository;
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }
        // GET: api/<ReviewersController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var reviewers = await _reviewerRepository.GetAll();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var dataToView = _mapper.Map<ICollection<ReviewerDto>>(reviewers);

            return Ok(dataToView);
        }

        // GET api/<ReviewersController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (!_reviewerRepository.ReviewerExists(id))
                return NotFound();

            var reviewer = await _reviewerRepository.GetById(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var dataToView = _mapper.Map<ReviewerDto>(reviewer);

            return Ok(dataToView);
        }

        // POST api/<ReviewersController>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ReviewerDto reviewerCreate)
        {
            if (reviewerCreate is null)
                return BadRequest(ModelState);
            var reviewers = await _reviewerRepository.GetAll();
            var reviewer = reviewers.Where(r => r.LastName.Trim().ToUpper() == reviewerCreate.LastName.TrimEnd().ToUpper()).FirstOrDefault();
            if(reviewer != null)
            {
                ModelState.AddModelError("", "Reviewer Already Exists");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var reviewerMap = _mapper.Map<Reviewer>(reviewerCreate);
            if (!_reviewerRepository.Create(reviewerMap))
            {
                ModelState.AddModelError("", "Something Went Wrong While Saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Created Successfully");
        }

        // PUT api/<ReviewersController>/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] ReviewerDto reviewerUpdate)
        {
            if (reviewerUpdate is null)
                return BadRequest(ModelState);
            if (id != reviewerUpdate.Id)
                return BadRequest(ModelState);
            if (!_reviewerRepository.ReviewerExists(id))
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest();
            var reviewerMap = _mapper.Map<Reviewer>(reviewerUpdate);
            if (!_reviewerRepository.Update(reviewerMap))
            {
                ModelState.AddModelError("", "Something Went Wrong ");
                return StatusCode(500, ModelState);
            }
            return Ok("Updated Successfully");
        }

        // DELETE api/<ReviewersController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!_reviewerRepository.ReviewerExists(id))
                return NotFound();

            var dependents = await _reviewRepository.GetByReviewerId(id);
            if (dependents.Count > 0)
                return BadRequest("This Reviewer is related to another reviews please delete these reviews or assign them to another reviewer");
            var reviewer = await _reviewerRepository.GetById(id);
            if (!_reviewerRepository.Delete(reviewer))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }
            return Ok("Deleted Successfully");
        }
    }
}
