namespace Postcard.WebAPI_SignalR.Repository
{

    public class gRPCRepository
    {
        private readonly Postcard.PostcardClient _blogServiceClient;

        public gRPCRepository(Postcard.PostcardClient blogServiceClient)
        {
            _blogServiceClient = blogServiceClient;
        }

        public async Task<bool> AddAsync(string user, string email, string prompt)
        {
            MailRequest mailRequest = new MailRequest() 
            { 
                User = user,
                Email  = email,
                Prompt = prompt
            };
            var res = await _blogServiceClient.SendMailAsync(mailRequest);
            return res.Success;
        }
    }
}
