using BlogGrpcService.Data;
using BlogGrpcService.Models;
using Grpc.Core;

namespace BlogGrpcService.Services
{
    public class BlogPostsService : BlogPosts.BlogPostsBase
    {
        private readonly BlogDbContext _context;

        public BlogPostsService(BlogDbContext context)
        {
            _context = context;
        }

        public override async Task<PostReply> CreatePost(CreatePostRequest request, ServerCallContext context)
        {
            var post = new Post
            {
                Title = request.Title,
                Content = request.Content,
                ImageUrl = request.ImageUrl,
                Published = DateTime.UtcNow
            };
            try
            {
                _context.Posts.Add(post);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }


            return new PostReply
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                ImageUrl = post.ImageUrl,
                Published = post.Published.ToString()
            };
        }

        public override async Task<PostReply> GetPost(GetPostRequest request, ServerCallContext context)
        {
            var post = await _context.Posts.FindAsync(request.Id);

            if (post == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Post with ID={request.Id} is not found."));
            }

            return new PostReply
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                ImageUrl = post.ImageUrl,
                Published = post.Published.ToString()
            };
        }

        public override async Task<PostReply> UpdatePost(UpdatePostRequest request, ServerCallContext context)
        {
            var post = await _context.Posts.FindAsync(request.Id);

            if (post == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Post with ID={request.Id} is not found."));
            }

            post.Title = request.Title;
            post.Content = request.Content;
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();

            return new PostReply
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                ImageUrl = post.ImageUrl,
                Published = post.Published.ToString()
            };
        }

        public override async Task<DeletePostReply> DeletePost(DeletePostRequest request, ServerCallContext context)
        {
            var post = await _context.Posts.FindAsync(request.Id);

            if (post == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Post with ID={request.Id} is not found."));
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return new DeletePostReply
            {
                Success = true
            };
        }
    }
}
