﻿using FriendSync.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FriendSync.Services; 

public class UserService(IMongoCollection<User> userCollection) {
    public async Task<List<User>> GetAsync() {
        return await userCollection.Find(new BsonDocument()).ToListAsync();
    }
    
    public async Task<User?> GetUserByIDAsync(string id) {
        FilterDefinition<User> filter = Builders<User>.Filter.Eq("Id", id);
        return await userCollection.FindAsync(filter).Result.FirstOrDefaultAsync();
    }
    
    public async Task<User?> GetUserByUserNameAsync(string username) {
        FilterDefinition<User> filter = Builders<User>.Filter.Eq("Username", username);
        return await userCollection
            .FindAsync(filter)
            .Result
            .FirstOrDefaultAsync();
    }
    
    public async Task<User?> GetUserByEmailAsync(string email) {
        FilterDefinition<User> filter = Builders<User>.Filter.Eq("Email", email);
        return await userCollection
            .FindAsync(filter)
            .Result
            .FirstOrDefaultAsync();
    }
    
    public async Task<List<User>> GetAllUsersByUserNameAsync(string userNameSubstring) {
        return await userCollection
            .FindAsync(user => 
                user.Username!.Contains(userNameSubstring))
            .Result
            .ToListAsync();
    }
    
    public async Task CreateAsync(User playlist) {
        await userCollection.InsertOneAsync(playlist);
    }

    // TODO: Update
    public async Task AddToUserAsync(string id, string movieId) {
        FilterDefinition<User> filter = Builders<User>.Filter.Eq("Id", id);
        UpdateDefinition<User> update = Builders<User>.Update.AddToSet("movieIds", movieId);
        await userCollection.UpdateOneAsync(filter, update);
    }

    public async Task<DeleteResult> DeleteAsync(string username) {
        FilterDefinition<User> filter = Builders<User>.Filter.Eq("Username", username);
        return await userCollection.DeleteOneAsync(filter);
    }

}
