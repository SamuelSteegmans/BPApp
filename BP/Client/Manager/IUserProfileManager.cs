namespace BP.Client.Manager
{
    public interface IUserProfileManager
    {
        public Task<byte[]> GetUserProfilePictureAsync(string? path);
        public Task LikeUser(string userId, string targetUserId);
    }
}
