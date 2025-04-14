using BlogCMS.Data;
using BlogCMS.Models;
using BlogCMS.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogCMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuotesController : ControllerBase
    {
        private readonly QuoteRepository _repository;

        public QuotesController(QuoteRepository repository)
        {
            _repository = repository;
        }

        // POST: api/Quotes
        [HttpPost]
        public async Task<ActionResult<Quote>> PostQuote(Quote quote)
        {
            var newQuote = await _repository.CreateAsync(quote);

            return CreatedAtAction("GetQuote", new { id = quote.Id }, newQuote);
        }

        [HttpGet("{id}")]
        public async Task<Quote> GetQuote(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        [HttpGet]
        public async Task<IEnumerable<Quote>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        [HttpPatch]
        public async Task<ActionResult<Quote>> PatchQuote(Quote quote)
        {
            await _repository.UpdateAsync(quote);
            return CreatedAtAction("GetQuote", new { id = quote.Id }, quote);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Quote>> DeleteQuote(int id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
