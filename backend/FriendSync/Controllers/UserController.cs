using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using FriendSync.Services;
using FriendSync.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using MongoDB.Driver;

namespace FriendSync.Controllers; 

[Controller]
[Route("api/[controller]")]
public class UserController : Controller {
    
    private readonly UserService _userService;

    public UserController(MongoDBService mongoDBService) {
        _userService = mongoDBService.UserService;
    }

    [HttpGet]
    public async Task<List<User>> Get() {
        return await _userService.GetAsync();
    }
    
    /// <summary>
    /// Gets a specific User by unique id.
    /// </summary>
    /// <param name="id"> MongoDB unique id to search for user </param>
    /// <returns> The user with the specified id if found else null </returns>
    /// <response code="200"> Returns 200 if the user was successfully found </response>
    /// <response code="400"> Returns 400 if the id is not a valid MongoDB id </response>
    /// <response code="404"> Returns 404 if the user was not found </response>
    [HttpGet("/api/User/find-by-id/{id}")]
    public async Task<User?> GetUserByIDAsync(string id) {
        if (id.Length != 24) {
            Response.StatusCode = 400;
            return null;
        }
        
        User? user = await _userService.GetUserByIDAsync(id);
        
        if (user == null) {
            Response.StatusCode = 404;
            return null;
        }
        
        return user;
    }
    
    [HttpGet("/api/User/find-by-username/{username}")]
    public async Task<User?> GetUserByUsernameAsync(string username) {
        return await _userService.GetUserByUserNameAsync(username);
    }
    
    [HttpGet("/api/User/find-all-by-username/{username}")]
    public async Task<List<User>> GetAllUsersByUsernameAsync(string username) {
        return await _userService.GetAllUsersByUserNameAsync(username);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] User user) {
        // Prevent post request from having setting the ID to prevent MongoDB from throwing an error
        user.Id = "";
        
        if (user.Username == null || user.Password == null || user.Email == null) {
            return BadRequest("Username, email, and password are required");
        }
        
        // Verify username and email are unique
        if (await _userService.GetUserByUserNameAsync(user.Username) != null) {
            return BadRequest("Username is taken");
        }
        
        /*if (await _userService.GetUserByEmailAsync(user.Email) != null) {
            return BadRequest("Email is taken");
        }*/
        
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
    /// <returns>  </returns>
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
                "Error deleting data\n" + e.Message);
        }
    }
    
}
