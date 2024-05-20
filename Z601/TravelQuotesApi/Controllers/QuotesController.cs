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
        private IRepository<Quote> _repository;

        public QuotesController(IRepository<Quote> repository)
        {
            _repository = repository;
        }

        // GET: api/Quotes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Quote>>> GetQuotes()
        {   var quotes = await _repository.GetAllAsync();
            return Ok(quotes);
        }

        // POST: api/Quotes
        [HttpPost]
        public async Task<ActionResult<Quote>> PostQuote(Quote quote)
        {
            await _repository.CreateAsync(quote);
            return CreatedAtAction("GetByIdAsync", new { id = quote.Id }, quote);
        }

        [HttpGet("{id}")]
        public async Task<Quote> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        [HttpDelete("{id}")]
        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        [HttpPut("{id}")]
        public async Task UpdateAsync(Quote entity)
        {
            await _repository.UpdateAsync(entity);
        }
    }
}
