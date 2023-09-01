using ElBasset.DataBase;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ElBasset.DTO.DTO;
using Microsoft.AspNetCore.Authorization;

namespace ElBasset.Controllers.Roles
{
    [Authorize(Roles = "الادمن")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var Roles = await _roleManager.Roles.ToListAsync();
            List<RoleVM> AllRoles = new List<RoleVM>();
            foreach (var role in Roles)
            {
                AllRoles.Add(new RoleVM() { RoleName = role.Name, RoleId = role.Id });
            }
            return View(AllRoles);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RoleVM role)
        {
            var Role = new IdentityRole() { Name = role.RoleName };
            var Result = await _roleManager.CreateAsync(Role);
            if (!Result.Succeeded)
            {
                ViewBag.Result = "حدث خطأ اثناء انشاء الدور ";
            }
            return RedirectToAction("Index");
            return View(role);

        }
        [HttpGet]
        public async Task<IActionResult> Edit(string Id)
        {
            if (string.IsNullOrEmpty(Id))
                return View("~/Views/Shared/Error.cshtml");
            IdentityRole role = await _roleManager.FindByIdAsync(Id);
            var Role = new RoleVM() { RoleName = role.Name, RoleId = role.Id };
            return View(Role);

        }
        [HttpPost]
        public async Task<IActionResult> Edit(RoleVM model)
        {
            var CurrentRole = await _roleManager.FindByIdAsync(model.RoleId);
            var RoleId = CurrentRole.Id;
            if (ModelState.IsValid)
            {
                IdentityRole roleToEdit = await _roleManager.FindByIdAsync(RoleId);

                if (roleToEdit == null)
                {
                    return NotFound();
                }

                if (roleToEdit.Name != model.RoleName)
                    roleToEdit.Name = model.RoleName;

                IdentityResult result = await _roleManager.UpdateAsync(roleToEdit);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.ToString());
                    }
                }

            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(string Id)
        {
            if (string.IsNullOrEmpty(Id))
                return View("~/Views/Shared/Error.cshtml");
            IdentityRole role = await _roleManager.FindByIdAsync(Id);
            var Role = new RoleVM() { RoleName = role.Name, RoleId = role.Id };
            return View(Role);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmation(string RoleId)
        {
            var CurrenRole = await _roleManager.FindByIdAsync(RoleId);
            var roleId = CurrenRole.Id;
            if (ModelState.IsValid)
            {
                IdentityRole roleToDelete = await _roleManager.FindByIdAsync(roleId);
                var RoleName = roleToDelete.Name;
                if (roleToDelete == null)
                {
                    return NotFound();
                }
                var AllUserinRole = await _userManager.GetUsersInRoleAsync(roleId);
                if (AllUserinRole != null)
                {
                    foreach (var user in AllUserinRole)
                    {
                        await _userManager.RemoveFromRoleAsync(user, RoleName);
                    }
                }
                IdentityResult result = await _roleManager.DeleteAsync(roleToDelete);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.MessageResult = "حدث خطأ اثناء حذف الدور";
                    return View("Index");
                }
            }
            else
            {
                ViewBag.MessageResult = "حدث خطأ اثناء حذف الدور";
                return View("Index");
            }

        }
    }
}
