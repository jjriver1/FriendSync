using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FriendSync.Models;

public class Post
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = "";
    public string Content { get; set; } = "";
    public string AuthorUsername { get; set; } = "";
    public DateTime CreatedAt { get; set; }
    public List<string>? Likes { get; set; } //List of user IDs who liked the post
    public List<Comment>? Comments { get; set; } //List of comments
}
