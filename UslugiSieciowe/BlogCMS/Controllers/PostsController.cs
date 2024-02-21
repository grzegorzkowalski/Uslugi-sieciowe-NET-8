using Microsoft.AspNetCore.Mvc;
using BlogCMS.Models; 
using BlogCMS.Interfaces; 

[Route("api/[controller]")]
[ApiController]
public class PostsController : ControllerBase
{
    private readonly IRepository<Post> _postRepository;

    public PostsController(IRepository<Post> postRepository)
    {
        _postRepository = postRepository;
    }

    // GET: api/posts
    [HttpGet]
    public async Task<IActionResult> GetAllPosts()
    {
        var posts = await _postRepository.GetAllAsync();
        return Ok(posts);
    }

    // GET: api/posts/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPostById(int id)
    {
        var post = await _postRepository.GetByIdAsync(id);
        if (post == null)
        {
            return NotFound();
        }
        return Ok(post);
    }

    // POST: api/posts
    [HttpPost]
    public async Task<IActionResult> CreatePost(Post post)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _postRepository.AddAsync(post);
        return Created();
    }

    // PUT: api/posts/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePost(Post post)
    {
        var state = await _postRepository.UpdateAsync(post);
        if (!state)
        {
            return NotFound();
        }

        return NoContent();
    }

    // DELETE: api/posts/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePost(int id)
    {
        var state = await _postRepository.DeleteAsync(id);
        if (!state)
        {
            return NotFound();
        }

        return NoContent();
    }
}
