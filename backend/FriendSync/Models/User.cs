using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;
namespace FriendSync.Models; 

public class User {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    //add max leng for username
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; } 
    //public string ProfilePictureURL { get; set; }
    public string Bio { get; set; }
}
