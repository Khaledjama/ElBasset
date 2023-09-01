using ElBasset.DataBase;
using ElBasset.DTO.DTO;
using ElBasset.Repo.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElBasset.Controllers.Assistent
{
    [Authorize(Roles = "الادمن")]
    public class AssitentController : Controller
    {
        private readonly IUnitOfWork _unit;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AssitentController(IUnitOfWork unit, SignInManager<ApplicationUser> sign, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> RoleManager)
        {
            this._unit = unit;
            this._userManager = userManager;
            this._roleManager = RoleManager;
            this._signInManager = sign;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var AllUser = await _userManager.Users.Where(s => s.Department == null).ToListAsync();
            return View(AllUser);
        }
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            await Data();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO model)
        {
            if (!ModelState.IsValid)
            {
                await Data();
                return View(model);

            }
            // Check Emial or User name already Exist
            var checkEmail = await _userManager.FindByEmailAsync(model.Email);
            var checkUserName = await _userManager.FindByNameAsync(model.UserName);
            if (checkEmail != null && checkUserName != null)
            {
                ViewBag.Message = "الايميل او اسم المستخدم موجود مسبقا";
                await Data();
                return View(model);
            }
            // Create Users 
            ApplicationUser AppUser = new ApplicationUser()
            {
                Email = model.Email,
                UserName = model.UserName,
                Comment = "New User"
            };

            var Result = await _userManager.CreateAsync(AppUser, model.Password);
            if (!Result.Succeeded)
            {
                ViewBag.Message = "حدث خطأ";
                await Data();
                return View(model);
            }
            // Insert into Student Role
            var Role = await _roleManager.FindByIdAsync(model.RoleId);
            var inserUserInRole = await _userManager.AddToRoleAsync(AppUser, Role.Name);
            if (!inserUserInRole.Succeeded)
            {
                ViewBag.Message = "حدث خطأ";
                await Data();
                return View(model);
            }
            return RedirectToAction("Index");
        }
        private async Task Data()
        {
            var AllDepartment = await _unit.DepartmentRepo.GetAllAsync();
            ViewBag.AllDepartment = AllDepartment;
            var AllRole = await _roleManager.Roles.ToListAsync();
            ViewBag.AllRole = AllRole;

        }
        [HttpGet]
        public async Task<IActionResult> ResetPassword(string Id)
        {
            var CurrentUser = await _userManager.FindByIdAsync(Id);
            var token = await _userManager.GeneratePasswordResetTokenAsync(CurrentUser);
            var ResetPasswordDTO = new ResetPasswordDTO() { UserId = Id, Token = token };
            return View(ResetPasswordDTO);
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO model)
        {
            var CurrentUser = await _userManager.FindByIdAsync(model.UserId);
            var ResuetPassword = await _userManager.ResetPasswordAsync(CurrentUser, model.Token, model.Password);
            if (!ResuetPassword.Succeeded)
            {
                ViewBag.Message = "حدث خطأ";
                return View(model);
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(string Id)
        {
            try
            {
                var AppUser = await _userManager.FindByIdAsync(Id);
                await _userManager.DeleteAsync(AppUser);
                return RedirectToAction("Index");
            }
            catch 
            {

                return View("~/Views/Shared/Error.cshtml");
            }
        }
    }
}
