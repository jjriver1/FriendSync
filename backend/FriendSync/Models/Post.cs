using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FriendSync.Models;

public class Post
{
    public ObjectId Id { get; set; }
    public string Content { get; set; }
    public ObjectId AuthorId { get; set; } //Reference to the author's user ID
    public DateTime CreatedAt { get; set; }
    public List<ObjectId> Likes { get; set; } //List of user IDs who liked the post
    public List<Comment> Comments { get; set; } //List of comments
}