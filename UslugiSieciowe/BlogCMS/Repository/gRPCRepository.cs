using BlogCMS.Models;
using static BlogCMS.BlogPosts;

namespace BlogCMS.Repository
{

    public class gRPCRepository
    {
        private readonly BlogPostsClient _blogServiceClient;

        public gRPCRepository(BlogPostsClient blogServiceClient)
        {
            _blogServiceClient = blogServiceClient;
        }

        public async Task<Post> GetByIdAsync(int id)
        {
            var request = new GetPostRequest
            {
                Id = id
            };
            var res = await _blogServiceClient.GetPostAsync(request);

            var post = new Post()
            {
                Id = res.Id,
                Title = res.Title,
                Content = res.Content,
                ImageUrl = res.ImageUrl,
                Published = DateTime.Parse(res.Published)
            };
            return post;
        }
    }
}
