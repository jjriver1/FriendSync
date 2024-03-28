using FriendSync.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace FriendSync.Services;

public class MongoDBService {

    public UserService UserService { get; private set; }
    public PostService PostService { get; private set; }
   // public CommentService CommentService { get; private set; }

    public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings) {
        MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
        IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
        
        var userCollection = database.GetCollection<User>(mongoDBSettings.Value.UserCollection);
        UserService = new UserService(userCollection);
        
        var postCollection = database.GetCollection<Post>(mongoDBSettings.Value.PostCollection);
        PostService = new PostService(postCollection);
        
        // var CommentCollection = database.GetCollection<Comment>(mongoDBSettings.Value.CommentCollection);
        // CommentService = new CommentService(CommentCollection);
    }

}
