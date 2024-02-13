using FriendSync.Models;
using FriendSync.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FriendSync.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostController : ControllerBase
{
    private readonly IPostRepository _postRepository;

    public PostController(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPostById(string id)
    {
        var post = await _postRepository.GetPostByIdAsync(id);
        return Ok(post);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePost(Post post)
    {
        await _postRepository.CreatePostAsync(post);
        return CreatedAtAction(nameof(GetPostById), new { id = post.Id }, post);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePost(string id)
    {
        var existingPost = await _postRepository.GetPostByIdAsync(id);
        await _postRepository.DeletePostAsync(id);
        return NoContent();
    }

}

// [HttpDelete("{id}")]
// public IActionResult DeletePost(string id)
// {
//     var existingPost = _postRepository.GetPostById(id);
//     if (existingPost == null)
//     {
//         return NotFound();
//     }
//     _postRepository.DeletePost(id);
//     return NoContent();
// }