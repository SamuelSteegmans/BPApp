// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using BlazorInputFile;
using BP.Server.Data;
using BP.Shared.Models;
using BP.Shared.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BP.Server.Areas.Identity.Pages.Account.Manage
{
    public class ProfileModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProfileModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context,
            IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "Voornaam")]
            [MaxLength(50)]
            public string FirstName { get; set; }
            [Display(Name = "Leeftijd")]
            public string Age { get; set; }
            [Display(Name = "Gender")]
            [MaxLength(20)]
            public string Gender { get; set; }
            [Display(Name = "Beschrijving")]
            [MaxLength(500)]
            public string Description { get; set; }
            [Display(Name = "Interesses (gescheiden met komma)")]
            [MaxLength(100)]
            public string Interests { get; set; }
            [Display(Name = "Profielfoto")]
            public IFormFile ProfileImage { get; set; }
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string FirstName { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public string Description { get; set; }
        public string Interests { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        public string ImagePath { get; set; }

        public string ProfileImagePath = "";
            
        private async Task LoadAsync(ApplicationUser user)
        {
            //FirstName = user.FirstName;
            //Age = user.Age.ToString();
            //Gender = user.Gender;
            //Description = user.Description;
            //string interests = "";
            //foreach (var interest in user.Interests)
            //{
            //    interests += interest + "; ";
            //}
            //Interests = interests;

            IFormFile file = null;
            byte[] data = null;
            if (!string.IsNullOrEmpty(user.ProfilePicturePath))
            {
                string path = "./wwwroot/images/" + user.ProfilePicturePath;
                
                using (var stream = System.IO.File.OpenRead(path))
                {
                    file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name));
                    using (var ms = new MemoryStream())
                    {
                        file.CopyTo(ms);
                        data = ms.ToArray();
                        ms.Close();
                    }
                }
            }

            var imageSrc = data != null ? Convert.ToBase64String(data) : null;
            string imageJpgDataURL = string.Format("data:image/jpeg;base64,{0}", imageSrc);
            ProfileImagePath = imageJpgDataURL;

            Input = new InputModel
            {
                FirstName = user.FirstName,
                Age = user.Age.ToString(),
                Gender = user.Gender,
                Description = user.Description,
                Interests = user.Interests,
                ProfileImage = file != null ? file : null
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            user.FirstName = Input.FirstName;
            user.Age = Input.Age != null ? int.Parse(Input.Age) : null;
            user.Gender = Input.Gender;
            user.Description = Input.Description;
            user.Interests = Input.Interests;

            //FileInfo file = new FileInfo("./wwwroot/images/" + user.ProfilePicturePath);
            //if (Input.ProfileImage != null)
            //{
            //    file.Delete();
            //}
          
            string uniqueFileName = null;

            if (Input.ProfileImage != null)
            {
                string uploadsFolder = "./wwwroot/images/";
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Input.ProfileImage.FileName;
                string filePath = uploadsFolder + uniqueFileName;
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    Input.ProfileImage.CopyTo(fileStream);
                }
            }
            user.ProfilePicturePath = uniqueFileName == null ? user.ProfilePicturePath : uniqueFileName;

            await _userManager.UpdateAsync(user);

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Uw profiel is aangepast!";
            return RedirectToPage();
        }
    }
}
