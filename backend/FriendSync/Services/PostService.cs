using FriendSync.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FriendSync.Services; 

public class PostService {
    
    private readonly IMongoCollection<Post> _postCollection;

    public PostService(IMongoCollection<Post> postCollection) {
        _postCollection = postCollection;
    }
    
    public async Task<List<Post>> GetAsync()
    {
        return await _postCollection.Find(new BsonDocument()).ToListAsync();
    }
    
    /*public async Task<List<Post>> GetAsync(string id)
    {
        FilterDefinition<Post> filter = Builders<Post>.Filter.Eq("Id", id);
        return await _postCollection.Find(filter).ToListAsync();
    }*/

    public async Task CreateAsync(Post post)
    {
        await _postCollection.InsertOneAsync(post);
    }
    
    public async Task DeleteAsync(string id)
    {
        FilterDefinition<Post> filter = Builders<Post>.Filter.Eq("Id", id);
        await _postCollection.DeleteOneAsync(filter);
    }
    
}
