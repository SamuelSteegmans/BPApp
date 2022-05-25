using BP.Client.Manager;
using BP.Shared.Models;
using BP.Shared.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;

namespace BP.Client.Pages
{
    public partial class Profiles
    {
        public List<ApplicationUser> users = new List<ApplicationUser>();

        private List<string> profilePictures = new List<string>();

        protected override async Task OnInitializedAsync()
        {
            users = await _chatManager.GetUsersAsync();
            foreach (var user in users)
            {
                byte[] data = await _userProfileManager.GetUserProfilePictureAsync(user.ProfilePicturePath);
                var imageSrc = Convert.ToBase64String(data);
                string imageJpgDataURL = string.Format("data:image/jpeg;base64,{0}", imageSrc);
                profilePictures.Add(imageJpgDataURL);
            }
        }

        async Task LoadUserChat(string userId)
        {
            var contact = await _chatManager.GetUserDetailsAsync(userId);
            var ContactId = contact.Id;
            _navigationManager.NavigateTo($"chat/{ContactId}");
        }

        async Task Like(string userId)
        {
            var state = await _stateProvider.GetAuthenticationStateAsync();
            var currentUserId = state.User.Claims.Where(a => a.Type == "sub").Select(a => a.Value).FirstOrDefault();
            await _userProfileManager.LikeUser(currentUserId, userId);
        }
    }
}
