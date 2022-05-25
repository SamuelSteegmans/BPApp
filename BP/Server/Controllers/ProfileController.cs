using BP.Server.Data;
using BP.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Drawing.Imaging;
using System.Security.Claims;

namespace BP.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        public ProfileController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet("profilepicture/{path}")]
        public async Task<IActionResult> GetUserProfilePicture(string path)
        {
            string filePath = "./wwwroot/images/" + path;
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        fileStream.CopyTo(memoryStream);
                        Bitmap image = new Bitmap(1, 1);
                        image.Save(memoryStream, ImageFormat.Jpeg);

                        byte[] byteImage = memoryStream.ToArray();
                        return Ok(byteImage);
                    }
                }
        }

        [HttpPost]
        public async Task<IActionResult> AddProfileLike(Like like)
        {
            ApplicationUser originUser = _context.Users.Where(u => u.Id.Equals(like.originUserId)).FirstOrDefault();
            ApplicationUser targetUser = _context.Users.Where(u => u.Id.Equals(like.targetUserId)).FirstOrDefault();
            like.originUser = originUser;
            like.targetUser = targetUser;
            _context.Likes.Add(like);
            _context.SaveChanges();
            return Ok();
        }

        [HttpGet("likedusers")]
        public async Task<IActionResult> GetLikedUsers()
        {
            var userId = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value).FirstOrDefault();
            var allUsers = await _context.Users.Where(user => user.Id != userId).ToListAsync();
            var currentUser =  allUsers.Where(user => user.Id == userId).FirstOrDefault();
            //List<Like> likes = _context.Likes.Where(l => l.originUserId == userId).ToList();
            //List<Like> liked = _context.Likes.Where(l => l.targetUserId == userId).ToList();
            //List<Like> bothLiked = likes.Where(l => liked.Select(l => l.targetUserId).Contains(l.originUserId)).ToList();
            //var users = allUsers.Where(u => bothLiked.Select(l => l.targetUserId).Contains(u.Id)).ToList();

            var users = allUsers.Where(user => _context.Likes.Select(l => l.originUserId + " " + l.targetUserId).Contains(userId + " " + user.Id)
            && _context.Likes.Select(l => l.originUserId + " " + l.targetUserId).Contains(user.Id + " " + userId)).ToList();
            return Ok(users);
        }
    }
}
