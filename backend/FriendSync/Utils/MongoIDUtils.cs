namespace FriendSync.Utils;

public static class MongoIDUtils
{
    public static bool IsValidMongoId(string id)
    {
        return id.Length == 24;
    }
}
