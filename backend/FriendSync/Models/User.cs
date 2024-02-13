using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;
namespace FriendSync.Models; 

public class User {
    [BsonId]
    public ObjectId Id { get; set; }
    //add max leng for username
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; } 
    //public string ProfilePictureURL { get; set; }
    public string Bio { get; set; }
}
