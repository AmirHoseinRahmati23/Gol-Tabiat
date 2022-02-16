using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PoshakMonix.Models.Entities;
using PoshakMonix.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoshakMonix.Web.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UsersController(UserManager<User> userManager , SignInManager<User> signInManager , RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await userManager.Users.AsNoTracking().ToListAsync();
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string userId)
        {
            try
            {


                var user = await userManager.FindByIdAsync(userId);

                await userManager.DeleteAsync(user);


                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "مشکلی در حذف کاربر به وجود آمد");
            }
            return View();

        }

        [HttpGet]
        public async Task<IActionResult> AddUserToRole(string id)
        {
            var roles = await roleManager.Roles.AsNoTracking().Select(s => new RoleListViewModel() {
                RoleName = s.Name,
                RoleId = s.Id
            }).ToListAsync();

            ViewBag.User = await userManager.FindByIdAsync(id);

            return View(roles);
        }

        [HttpPost]
        public async Task<IActionResult> AddUserToRole(string userId ,List<string> roles)
        {
            var user = await userManager.FindByIdAsync(userId);
            bool succeeded = true;

            foreach(var item in await roleManager.Roles.ToListAsync())
            {
                var result = await userManager.RemoveFromRoleAsync(user, item.Name);
            };
            
            foreach(var item in roles)
            {
                var result = await userManager.AddToRoleAsync(user, item);
                succeeded = result.Succeeded;
                if (!succeeded)
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                }
            }

            if (succeeded)
            {
                return RedirectToAction(nameof(Index) , "Users");
            }



            return View();
        }

    }
}
