using FriendSync.Controllers;
using FriendSync.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;

namespace FriendSync.Services;

public class MongoDBService {

    public UserService UserService { get; private set; }
    public PostService PostService { get; private set; }
    public CommentService CommentService { get; private set; }

    public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings) {
        MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
        IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
        
        var UserCollection = database.GetCollection<User>(mongoDBSettings.Value.UserCollection);
        UserService = new UserService(UserCollection);
        
        var PostCollection = database.GetCollection<Post>(mongoDBSettings.Value.PostCollection);
        PostService = new PostService(PostCollection);
        
        var CommentCollection = database.GetCollection<Comment>(mongoDBSettings.Value.CommentCollection);
        CommentService = new CommentService(CommentCollection);
    }

}
