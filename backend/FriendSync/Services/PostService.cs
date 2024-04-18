using FriendSync.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FriendSync.Services; 

public class PostService(IMongoCollection<Post> postCollection) {
    public async Task<List<Post>> GetAsync()
    {
        return await postCollection.Find(new BsonDocument()).ToListAsync();
    }
    
    /*public async Task<List<Post>> GetAsync(string id)
    {
        FilterDefinition<Post> filter = Builders<Post>.Filter.Eq("Id", id);
        return await _postCollection.Find(filter).ToListAsync();
    }*/
    
    public async Task<List<Post>> GetPostByAuthorAsync(string author) {
        FilterDefinition<Post> filter = Builders<Post>.Filter.Eq("AuthorUsername", author);
        return await postCollection.FindAsync(filter).Result.ToListAsync();
    }

    public async Task CreateAsync(Post post)
    {
        await postCollection.InsertOneAsync(post);
    }
    
    public async Task DeleteAsync(string id)
    {
        FilterDefinition<Post> filter = Builders<Post>.Filter.Eq("Id", id);
        await postCollection.DeleteOneAsync(filter);
    }
    
}
