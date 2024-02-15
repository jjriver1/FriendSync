using FriendSync.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FriendSync.Services; 

public class UserService {
    
    private readonly IMongoCollection<User> _userCollection;

    public UserService(IMongoCollection<User> UserCollection) {
        _userCollection = UserCollection;
    }
    
    public async Task<List<User>> GetAsync() {
        return await _userCollection.Find(new BsonDocument()).ToListAsync();
    }
    
    public async Task<User> GetUserByIDAsync(string id) {
        FilterDefinition<User> filter = Builders<User>.Filter.Eq("Id", id);
        return await _userCollection.FindAsync(filter).Result.FirstOrDefaultAsync();
    }
    
    public async Task<User> GetUserByUserNameAsync(string username) {
        FilterDefinition<User> filter = Builders<User>.Filter.Eq("Username", username);
        return await _userCollection
            .FindAsync(filter)
            .Result
            .FirstOrDefaultAsync();
    }
    
    public async Task<List<User>> GetAllUsersByUserNameAsync(string userNameSubstring) {
        return await _userCollection
            .FindAsync(user => 
                user.Username.Contains(userNameSubstring))
            .Result
            .ToListAsync();
    }
    
    public async Task CreateAsync(User playlist) {
        await _userCollection.InsertOneAsync(playlist);
    }

    // TODO: Update
    public async Task AddToUserAsync(string id, string movieId) {
        FilterDefinition<User> filter = Builders<User>.Filter.Eq("Id", id);
        UpdateDefinition<User> update = Builders<User>.Update.AddToSet<string>("movieIds", movieId);
        await _userCollection.UpdateOneAsync(filter, update);
        return;
    }

    public async Task<DeleteResult> DeleteAsync(string id) {
        FilterDefinition<User> filter = Builders<User>.Filter.Eq("Id", id);
        return await _userCollection.DeleteOneAsync(filter);
    }  

}
