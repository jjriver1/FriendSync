using FriendSync.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FriendSync.Services;

public class CommentService
{
    private readonly IMongoCollection<Comment> _commentCollection;

    public CommentService(IMongoCollection<Comment> CommentCollection) {
        _commentCollection = CommentCollection;
    }
    
    public async Task<List<Comment>> GetAsync()
    {
        return await _commentCollection.Find(new BsonDocument()).ToListAsync();
    }
    
    public async Task CreateAsync(Comment comment)
    {
        await _commentCollection.InsertOneAsync(comment);
    }
    
    public async Task DeleteAsync(string id)
    {
        FilterDefinition<Comment> filter = Builders<Comment>.Filter.Eq("Id", id);
        await _commentCollection.DeleteOneAsync(filter);
        return;
    }
}