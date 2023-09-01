using ElBasset.DataBase;
using ElBasset.Service.UploadFiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ElBasset.Controllers.Account
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public UsersController(UserManager<ApplicationUser> UserManager)
        {
            this._userManager = UserManager;
        }
        public async Task<IActionResult> ChangeProtofile()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var CurrentUser = _userManager.Users.FirstOrDefault(s => s.Id == userId);
            IFormFile postedFile1 = Request.Form.Files[0];

            //check User has Photo 
            if (CurrentUser.ImagePath != null)
            {
               var ResultDelete = Upload.DeleteImage(CurrentUser.ImagePath);
                if(ResultDelete == true)
                {
                  var ImagePath=  Upload.UploadImage(postedFile1);
                    CurrentUser.ImagePath = ImagePath;
                   var ResultUpdate = await _userManager.UpdateAsync(CurrentUser);
                    if (ResultUpdate.Succeeded)
                    {
                        return Ok();
                    }
                }
            }
            var ImagePath2 = Upload.UploadImage(postedFile1);
            CurrentUser.ImagePath = ImagePath2;
            var ResultUpdate2 = await _userManager.UpdateAsync(CurrentUser);
            if (ResultUpdate2.Succeeded)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
