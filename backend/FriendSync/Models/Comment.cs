using MongoDB.Bson;

namespace FriendSync.Models;

public class Comment
{
    public ObjectId Id { get; set; }
    public ObjectId AuthorId { get; set; } //Reference to the author's user ID
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; } 
}