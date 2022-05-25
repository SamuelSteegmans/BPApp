using BP.Shared.Models;

namespace BP.Client.Manager
{
    public interface IChatManager
    {
        Task<List<ApplicationUser>> GetUsersAsync();
        Task<List<ApplicationUser>> GetLikedUsersAsync();
        Task<List<ApplicationUser>> GetUserContactsAsync();
        Task SaveMessageAsync(ChatMessage message);
        Task<List<ChatMessage>> GetConversationAsync(string contactId);
        Task<ApplicationUser> GetUserDetailsAsync(string userId);
    }
}
