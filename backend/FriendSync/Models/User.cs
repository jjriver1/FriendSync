using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FriendSync.Models; 

public class User {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    //add max length for username
    [BsonRequired]
    public string? Username { get; set; }
    [BsonRequired]
    public string? Email { get; set; }
    [BsonRequired]
    public string? Password { get; set; } 
    //public string ProfilePictureURL { get; set; }
    public string? Bio { get; set; }
}
