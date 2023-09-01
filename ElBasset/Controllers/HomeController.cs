using AutoMapper;
using ElBasset.DataBase;
using ElBasset.DTO.DTO;
using ElBasset.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using System.Xml.Linq;

namespace ElBasset.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signManager;
        private readonly IMapper _mapper;
        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> UserManager, SignInManager<ApplicationUser> signManager, IMapper Mapper)
        {
            _logger = logger;
            this._userManager = UserManager;
            this._signManager = signManager;
            this._mapper = Mapper;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        public async Task<IActionResult> Privacy()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value; 
            var CurrentUser =  _userManager.Users.FirstOrDefault(s => s.Id == userId);
            var ApplicationUser = _mapper.Map<ApplicationUserDTO>(CurrentUser);
            return View(ApplicationUser);
        }
        [Authorize]
        public async Task<IActionResult> ChangeMyProfile(ApplicationUserDTO model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var CurrentUser = _userManager.Users.FirstOrDefault(s => s.Id == userId);
            CurrentUser.FirstName = model.FirstName;
            CurrentUser.MiddleName = model.MiddleName;
            CurrentUser.LastName = model.LastName;
            CurrentUser.PhoneNumber = model.PhoneNumber;
            CurrentUser.Adress = model.Adress;
            await _userManager.UpdateAsync(CurrentUser);
            return RedirectToAction(nameof(Privacy));
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> RessetPassword()
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var CurrentUser = _userManager.Users.FirstOrDefault(s => s.Id == userId);
            var token = await _userManager.GeneratePasswordResetTokenAsync(CurrentUser);
            var ApplicationUser = _mapper.Map<ApplicationUserDTO>(CurrentUser);
            TempData["RessetPasswod"] = token;
            return View("Privacy",ApplicationUser);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Resset(string CurrentPassword,string newPassword,string confirmNewPassword)
        {
            string name;
            if (TempData.ContainsKey("token"))
                name = TempData["token"].ToString(); // returns "Bill" 
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var CurrentUser = _userManager.Users.FirstOrDefault(s => s.Id == userId);
            var Data = _mapper.Map<ApplicationUserDTO>(CurrentUser);
            var CheckPasswordAsync = await _userManager.CheckPasswordAsync(CurrentUser, CurrentPassword);
            if (!CheckPasswordAsync)
            {
                ViewBag.Result = "لقد حث خطأ اثناء تحديث كلمة المرور ";
                return View(viewName: "Privacy", Data);
            }
            var ChangePasswordAsync = await _userManager.ChangePasswordAsync(CurrentUser, CurrentPassword, newPassword);
            if (!ChangePasswordAsync.Succeeded)
            {
                ViewBag.Result = "لقد حث خطأ اثناء تحديث كلمة المرور ";
                return View(viewName: "Privacy", Data);
            }
            ViewBag.Result = "تم تحديث كلمة المرور بنجاح";
            return View(viewName: "Privacy", Data);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}