using ElBasset.DataBase;
using ElBasset.DTO.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ElBasset.Controllers.Authentication
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountController(
            UserManager<ApplicationUser> userManeger,
            SignInManager<ApplicationUser> SignManager,
            RoleManager<IdentityRole> RoleManager)
        {
            this._userManager = userManeger;
            this._signManager = SignManager;
            this._roleManager = RoleManager;
        }
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var AppUser = await _userManager.FindByEmailAsync(model.Email);
            if (AppUser == null)
            {
                ViewBag.Message = "الايميل او كلمة السر خطأ";
                return View(model);
            }
            var LoginResult = await _signManager.PasswordSignInAsync(AppUser, model.Password, false,lockoutOnFailure:false);
            if (!LoginResult.Succeeded)
            {
                ViewBag.Message = "الايميل او كلمة السر خطأ";
                return View(model);
            }
            var CheckUserIsAdminRole = await _userManager.IsInRoleAsync(AppUser, "الادمن");
            if (CheckUserIsAdminRole)
                return RedirectToAction("Index", "Home");
            return RedirectToAction("Index","Home");

                
        }
        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
          await _signManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
