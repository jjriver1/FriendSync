using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using FriendSync.Services;
using FriendSync.Models;
using FriendSync.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace FriendSync.Controllers; 

[Controller]
[Route("api/[controller]")]
public class UserController(MongoDBService mongoDBService, IConfiguration config) : Controller
{
    
    private readonly UserService _userService = mongoDBService.UserService;
    private readonly IConfiguration _config = config;

    [AllowAnonymous]
    [HttpGet]
    public async Task<List<User>> Get() {
        return await _userService.GetAsync();
    }
    
    [AllowAnonymous]
    [HttpPost("/api/User/login/{email}/{password}")]
    public async Task<IActionResult> Login(string email, string password) {
        User? foundUser = await _userService.GetUserByEmailAsync(email);
        
        if (foundUser == null) {
            return BadRequest("User not found");
        }
        
        if (!BCrypt.Net.BCrypt.Verify(password, foundUser.Password)) {
            return BadRequest("Incorrect password");
        }
        
        return Ok(GenerateJwtToken(foundUser.Username, foundUser.Email));
    }
    
    /// <summary>
    /// Gets a specific User by unique id.
    /// </summary>
    /// <param name="id"> MongoDB unique id to search for user </param>
    /// <returns> The user with the specified id if found else null </returns>
    /// <response code="200"> Returns 200 if the user was successfully found </response>
    /// <response code="400"> Returns 400 if the id is not a valid MongoDB id </response>
    /// <response code="404"> Returns 404 if the user was not found </response>
    [Authorize]
    [HttpGet("/api/User/find-by-id/{id}")]
    public async Task<User?> GetUserByIDAsync(string id) {
        if (!MongoIDUtils.IsValidMongoId(id)) {
            Response.StatusCode = 400;
            return null;
        }
        
        User? user = await _userService.GetUserByIDAsync(id);
        
        if (user == null) {
            Response.StatusCode = 404;
            return null;
        }
        // steven: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6InN0ZXZlbiIsImVtYWlsIjoic3RldmVuIiwibmJmIjoxNzExNDgzNjcwLCJleHAiOjE3MTE0OTA4NzAsImlzcyI6ImZyaWVuZC1zeW5jLmNvbSIsImF1ZCI6ImZyaWVuZC1zeW5jLmNvbSJ9.oXBkoyZSMcG83Ou_LduOuvaUqUG3aZoihMO1unXx9hE
        // steve: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6InN0ZXZlIiwiZW1haWwiOiJzdGV2ZSIsIm5iZiI6MTcxMTQ4NDMzNCwiZXhwIjoxNzExNDkxNTM0LCJpc3MiOiJmcmllbmQtc3luYy5jb20iLCJhdWQiOiJmcmllbmQtc3luYy5jb20ifQ.NdwhOaGjihCRBW0RcUQLieCGO5fQOxb-MfkYXJ1mabY
    
        
        return user;
    }
    
    [Authorize]
    [HttpGet("/api/User/find-by-username/{username}")]
    public async Task<User?> GetUserByUsernameAsync(string username) {
        return await _userService.GetUserByUserNameAsync(username);
    }
    
    [AllowAnonymous]
    [HttpGet("/api/User/find-all-by-username/{username}")]
    public async Task<List<User>> GetAllUsersByUsernameAsync(string username) {
        return await _userService.GetAllUsersByUserNameAsync(username);
    }

    /// <summary>
    /// Creates a new User.
    /// </summary>
    /// <param name="user"> User object to be created </param>
    /// <returns> The created user </returns>
    /// <response code="201"> Returns 201 if the user was successfully created </response>
    /// <response code="400"> Returns 400 if the user is missing required fields or the username is taken </response>
    [AllowAnonymous]
    [HttpPost]
    [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract")]
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
        
        if (await _userService.GetUserByEmailAsync(user.Email) != null) {
            return BadRequest("Email is taken");
        }

        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password, 12);
        
        await _userService.CreateAsync(user);
        return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
    }
    
    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> AddToUser(string id, [FromBody] string movieId) {
        await _userService.AddToUserAsync(id, movieId);
        return NoContent();
    }

    /// <summary>
    /// Deletes a specific User by unique id.
    /// </summary>
    /// <param name="username"> unique username to search for user </param>
    /// <returns>  </returns>
    /// <response code="200"> Returns 200 if the user was successfully deleted </response>
    /// <response code="404"> Returns 404 if the user was not found </response>
    /// <response code="500"> Returns 500 if an internal service error occurred </response>
    [Authorize]
    [HttpDelete("{username}")]
    public async Task<IActionResult> Delete(string username) {
        var usernameFromToken = HttpContext.User.FindFirst(ClaimTypes.Name);
        
        if (usernameFromToken == null) {
            return BadRequest("Invalid JWT token");
        }
        
        if (usernameFromToken.Value != username) {
            return Unauthorized();
        }
        
        try {
            DeleteResult deleteResult = await _userService.DeleteAsync(username);
            
            if (!deleteResult.IsAcknowledged || deleteResult.DeletedCount == 0) {
                return NotFound();
            }

            return Ok(deleteResult);
        } catch (Exception e) {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Error deleting data\n" + e.Message);
        }
    }

    private string GenerateJwtToken(string username, string email)
    {
        var key = _config["Jwt:Key"];
        if (key == null)
        {
            throw new Exception("Jwt:Key and Jwt:Issuer must be set in appsettings.json");
        }
        
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var secureToken = new JwtSecurityToken(_config["Jwt:Issuer"],
            _config["Jwt:Issuer"],
            new[] {
                new Claim(JwtRegisteredClaimNames.UniqueName, username),
                new Claim(JwtRegisteredClaimNames.Email, email),
            },
            notBefore: DateTime.Now,
            expires: DateTime.Now.AddMinutes(120),
            signingCredentials: credentials);

        var token =  new JwtSecurityTokenHandler().WriteToken(secureToken);

        return token;
    }
    
}
