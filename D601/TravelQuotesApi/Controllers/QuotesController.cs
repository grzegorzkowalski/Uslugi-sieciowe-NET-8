using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelQuotesApi.Data;
using TravelQuotesApi.Interfaces;
using TravelQuotesApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TravelQuotesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuotesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        private readonly IRepository<Quote> _repository;

        public QuotesController(ApplicationDbContext context, IRepository<Quote> repository)
        {
            _context = context;
            _repository = repository;
        }

        // GET: api/Quotes
        [HttpGet]
        public async Task<IEnumerable<Quote>> GetQuotes()
        {
            return await _repository.GetAllAsync(); 
        }

        // POST: api/Quotes
        [HttpPost]
        public async Task<ActionResult<Quote>> PostQuote([FromBody] Quote quote)
        {
            return await _repository.CreateAsync(quote);

            //return CreatedAtAction("GetQuote", new { id = quote.Id }, quote);
        }

        // Dodaj więcej metod API według potrzeb...
    }
}
