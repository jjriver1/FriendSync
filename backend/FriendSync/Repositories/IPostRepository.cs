using FriendSync.Models;

namespace FriendSync.Repositories;

public interface IPostRepository
{
    //add notes for each method
    Task<Post> GetPostByIdAsync(string id);
    Task CreatePostAsync(Post post);
    Task DeletePostAsync(string id);
}

//void DeletePost(string id);