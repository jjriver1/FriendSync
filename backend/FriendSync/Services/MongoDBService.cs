using FriendSync.Models;
using FriendSync.Repositories;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;

namespace FriendSync.Services;

public class MongoDBService {
    
    private readonly IMongoCollection<User> _playlistCollection;
    
    private readonly IPostRepository _postRepository;
    
    public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings) {
        MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
        IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
        _playlistCollection = database.GetCollection<User>(mongoDBSettings.Value.CollectionName);
    }
    

    public async Task<List<User>> GetAsync() {
        return await _playlistCollection.Find(new BsonDocument()).ToListAsync();
    }
    
    public async Task CreateAsync(User playlist) {
        await _playlistCollection.InsertOneAsync(playlist);
        return;
    }

    public async Task AddToUserAsync(string id, string movieId) {
        FilterDefinition<User> filter = Builders<User>.Filter.Eq("Id", id);
        UpdateDefinition<User> update = Builders<User>.Update.AddToSet<string>("movieIds", movieId);
        await _playlistCollection.UpdateOneAsync(filter, update);
        return;
    }

    public async Task DeleteAsync(string id) {
        FilterDefinition<User> filter = Builders<User>.Filter.Eq("Id", id);
        await _playlistCollection.DeleteOneAsync(filter);
        return;
    }  
    
    public async Task<Post> GetPostByIdAsync(string id)
    {
        return await _postRepository.GetPostByIdAsync(id);
    }

    public async Task CreatePostAsync(Post post)
    {
        await _postRepository.CreatePostAsync(post);
    }
    
    public async Task DeletePostAsync(string id)
    {
        await _postRepository.DeletePostAsync(id);
    }
    
}