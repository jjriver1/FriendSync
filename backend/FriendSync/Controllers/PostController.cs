using FriendSync.Models;
using FriendSync.Services;
using Microsoft.AspNetCore.Mvc;

namespace FriendSync.Controllers;

[Controller]
[Route("api/[controller]")]
public class PostController(MongoDBService mongoDBService) : Controller {
    private readonly PostService _postService = mongoDBService.PostService;

    [HttpGet]
    public async Task<List<Post>> Get() {
        return await _postService.GetAsync();
    }

    /*[HttpGet("{id}")]
    public async Task<IActionResult> GetPostById(string id) {
        var post = await _postService.GetPostByIdAsync(id);
        return Ok(post);
    }*/
    
    [HttpGet("{author}")]
    public async Task<List<Post>> GetPostById(string author) {
        return await _postService.GetPostByAuthorAsync(author);;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePost(Post post) {
        post.Id = "";
        post.CreatedAt = DateTime.Now;
        
        await _postService.CreateAsync(post);
        return CreatedAtAction(nameof(Get), new { id = post.Id }, post);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePost(string id) {
        await _postService.DeleteAsync(id);
        return NoContent();
    }

}
