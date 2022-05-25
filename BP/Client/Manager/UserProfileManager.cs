using BP.Shared.Models;
using System.Net.Http.Json;

namespace BP.Client.Manager
{
    public class UserProfileManager : IUserProfileManager
    {
        private readonly HttpClient _httpClient;
        public UserProfileManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<byte[]> GetUserProfilePictureAsync(string? path)
        {
            if (path != null)
            {
                return await _httpClient.GetFromJsonAsync<byte[]>($"api/profile/profilepicture/{path}");
                //byte[] bytePhoto = await photo.Content.ReadAsByteArrayAsync();
                //return Convert.ToBase64String(photo); 
            }
            else
            {
                path = "unknownpicture.png";
                return await _httpClient.GetFromJsonAsync<byte[]>($"api/profile/profilepicture/{path}");
            }
        }

        public async Task LikeUser(string userId, string targetUserId)
        {
            Like like = new Like() { originUserId = userId , targetUserId = targetUserId};
            await _httpClient.PostAsJsonAsync($"api/profile", like);
        }
    }
}
