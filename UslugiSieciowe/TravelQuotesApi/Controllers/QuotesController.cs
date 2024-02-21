using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelQuotesApi.Data;
using TravelQuotesApi.Interfaces;
using TravelQuotesApi.Models;

namespace TravelQuotesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuotesController : ControllerBase
    {
        private readonly IRepository<Quote> _repository;
        private readonly ILogger<QuotesController> _logger;

        public QuotesController(IRepository<Quote> repository, ILogger<QuotesController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        // GET: api/Quotes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Quote>>> GetQuotes()
        {
            var all = await _repository.GetAllAsync();
            return Ok(all);
        }

        // GET: api/Quotes/id
        [HttpGet("{id}")]
        public async Task<ActionResult<Quote>> GetQuote(int id)
        {
            var quote = await _repository.GetByIdAsync(id);
            if (quote != null)
            {
                return Ok(quote);
            }
            else
            {
                return NotFound();
            }
        }

        // POST: api/Quotes
        [HttpPost]
        public async Task<ActionResult<Quote>> PostQuote(Quote quote)
        {
            try
            {
                await _repository.CreateAsync(quote);
                _logger.LogInformation($"Dodano cytat {quote.Message}");
                return Created();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Błąd {ex.Message}, przy próbie dodania cytatu {quote.Message}");
                return BadRequest($"Błąd {ex.Message}, przy próbie dodania cytatu {quote.Message}");
            }
            //return CreatedAtAction("GetQuote", new { id = quote.Id }, quote);
        }

        // Update: api/Quotes/id
        [HttpPut]
        public async Task<ActionResult<Quote>> UpdateQuote(Quote quote)
        {
            await _repository.UpdateAsync(quote);
            return Created();
        }

        // Delete: api/Quotes/id
        [HttpDelete]
        public async Task<ActionResult<Quote>> DeleteQuote(int id)
        {
            try
            {
                await _repository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
