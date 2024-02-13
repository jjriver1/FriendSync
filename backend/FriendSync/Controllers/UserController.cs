using System;
using Microsoft.AspNetCore.Mvc;
using FriendSync.Services;
using FriendSync.Models;
using MongoDB.Driver;

namespace FriendSync.Controllers; 

[Controller]
[Route("api/[controller]")]
public class UserController : Controller {
    
    private readonly UserService _userService;

    public UserController(MongoDBService mongoDBService) {
        _userService = mongoDBService.UserService;
    }

    [HttpGet("/api/[controller]/find-all-users")]
    public async Task<List<User>> Get() {
        return await _userService.GetAsync();
    }
    
    [HttpGet("{id}")]
    public async Task<User> GetUserByIDAsync(string id) {
        return await _userService.GetUserByIDAsync(id);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] User user) {
        // Prevent post request from having setting the ID to prevent MongoDB from throwing an error
        user.Id = "";
        
        await _userService.CreateAsync(user);
        return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> AddToUser(string id, [FromBody] string movieId) {
        await _userService.AddToUserAsync(id, movieId);
        return NoContent();
    }

    /// <summary>
    /// Deletes a specific User by unique id.
    /// </summary>
    /// <param name="id"> MongoDB unique id to search for user </param>
    /// <returns> A </returns>
    /// <response code="200"> Returns 200 if the user was successfully deleted </response>
    /// <response code="404"> Returns 404 if the user was not found </response>
    /// <response code="500"> Returns 500 if an internal service error occurred </response>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id) {
        try {
            DeleteResult deleteResult = await _userService.DeleteAsync(id);
        
            if (!deleteResult.IsAcknowledged || deleteResult.DeletedCount == 0) {
                return NotFound();
            }

            return Ok(deleteResult);
        } catch (Exception e) {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Error deleting data");
        }
    }
    
}
