using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using RandomQuoteGrpc;
using RandomQuoteGrpc.Data;

namespace RandomQuoteGrpc.Services
{
    public class QuoteService : Quote.QuoteBase
    {
        private readonly AppDbContext _context;

        public QuoteService(AppDbContext context)
        {
            _context = context;
        }

        public override async Task<QuoteReply> GetRandomQuote(QuoteRequest request, ServerCallContext context)
        {
            var count = await _context.Quotes.CountAsync();
            var index = new Random().Next(count);
            var randomQuote = await _context.Quotes.Skip(index).FirstOrDefaultAsync();

            if (randomQuote == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "No quotes found."));
            }
            return new QuoteReply
            {
                Id = randomQuote.Id,
                Author = randomQuote.Author,
                Message = randomQuote.Message
            };
        }
    }
}
