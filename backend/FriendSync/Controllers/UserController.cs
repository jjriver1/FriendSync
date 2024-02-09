using System;
using Microsoft.AspNetCore.Mvc;
using FriendSync.Services;
using FriendSync.Models;

namespace FriendSync.Controllers; 

[Controller]
[Route("api/[controller]")]
public class UserController : Controller {
    
    private readonly MongoDBService _mongoDBService;

    public UserController(MongoDBService mongoDBService) {
        _mongoDBService = mongoDBService;
    }

    [HttpGet]
    public async Task<List<User>> Get() {
        return await _mongoDBService.GetAsync();
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] User user) {
        await _mongoDBService.CreateAsync(user);
        return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
    }

    /// <summary>
    /// put stuff
    /// </summary>
    /// <param name="id">stuff</param>
    /// <param name="movieId">stuff234</param>
    /// <returns>asdfasdf</returns>
    /// /// <response code="200">Returns 200 and the share price</response>
    /// <response code="400">Returns 400 if the query is invalid</response>
    [HttpPut("{id}")]
    public async Task<IActionResult> AddToUser(string id, [FromBody] string movieId) {
        await _mongoDBService.AddToUserAsync(id, movieId);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id) {
        await _mongoDBService.DeleteAsync(id);
        return NoContent();
    }
    
}
