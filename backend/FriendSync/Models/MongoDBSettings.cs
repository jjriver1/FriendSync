namespace FriendSync.Models;

public class MongoDBSettings {
    public string ConnectionURI { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
    public string UserCollection { get; set; } = null!;
    public string PostCollection { get; set; } = null!;
    public string CommentCollection { get; set; } = null!;
}
