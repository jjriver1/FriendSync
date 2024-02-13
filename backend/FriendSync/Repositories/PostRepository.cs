using FriendSync.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FriendSync.Repositories;

public class PostRepository : IPostRepository
{
    private readonly IMongoCollection<Post> _postsCollection;

    public PostRepository(IMongoDatabase database)
    {
        _postsCollection = database.GetCollection<Post>("posts");
    }

    public async Task<Post> GetPostByIdAsync(string id)
    {
        ObjectId objectId = ObjectId.Parse(id);
        return await _postsCollection.Find(post => post.Id == objectId).FirstOrDefaultAsync();
    }

    public async Task CreatePostAsync(Post post)
    {
        await _postsCollection.InsertOneAsync(post);
    }

    public async Task DeletePostAsync(string id)
    {
        ObjectId objectId = ObjectId.Parse(id);
        await _postsCollection.DeleteOneAsync(p => p.Id == objectId);
    }
}

// public void DeletePost(string id)
// {
//     ObjectId objectId = ObjectId.Parse(id);
//     _postsCollection.DeleteOne(p => p.Id == objectId);
// }