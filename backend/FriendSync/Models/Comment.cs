using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FriendSync.Models;

public class Comment
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public ObjectId AuthorId { get; set; } //Reference to the author's user ID
    public string? Content { get; set; }
    public DateTime CreatedAt { get; set; } 
}
