using Grpc.Core;
using Postcard.gRPC;

namespace Postcard.gRPC.Services
{
    public class PostcardService : Postcard.PostcardBase
    {
        private readonly ILogger<PostcardService> _logger;
        public PostcardService(ILogger<PostcardService> logger)
        {
            _logger = logger;
        }

        public override Task<MailReply> SendMail(MailRequest request, ServerCallContext context)
        {


            return Task.FromResult(new MailReply
            {
                Success = true
            });
        }
    }
}
