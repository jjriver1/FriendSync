// using FriendSync.Models;
// using FriendSync.Services;
// using Microsoft.AspNetCore.Mvc;
//
// namespace FriendSync.Controllers;
//
// [ApiController]
// [Route("api/[controller]")]
// public class CommentController : ControllerBase {
//     
//     private readonly CommentService _commentService;
//
//     public CommentController(MongoDBService mongoDBService) {
//         _commentService = mongoDBService.CommentService;
//     }
//
//     [HttpGet]
//     public async Task<List<Comment>> Get() {
//         return await _commentService.GetAsync();
//     }
//
//     [HttpPost]
//     public async Task<IActionResult> CreateComment(Comment comment) {
//         await _commentService.CreateAsync(comment);
//         return CreatedAtAction(nameof(Get), new { id = comment.Id }, comment);
//     }
//
//     [HttpDelete("{id}")]
//     public async Task<IActionResult> DeleteComment(string id) {
//         await _commentService.DeleteAsync(id);
//         return NoContent();
//     }
// }
