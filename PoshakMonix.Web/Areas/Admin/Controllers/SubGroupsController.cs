using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PoshakMonix.Core.Utilities;
using PoshakMonix.Data;
using PoshakMonix.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoshakMonix.Web.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    [Authorize(Roles = "Admin")]
    public class SubGroupsController : Controller
    {
        private readonly PoshakMonixContext db;

        public SubGroupsController(PoshakMonixContext db)
        {
            this.db = db;
        }
        public async Task<IActionResult> Index()
        {
            var subGroups = await db.SubGroups.AsNoTracking().ToListAsync();
            return View(subGroups);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var list = await db.Groups.AsNoTracking().Select(g => g.GroupName).ToListAsync();
            ViewBag.Groups = new SelectList(list , list.First());
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(SubGroup subGroup , string groupName)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var group = await db.Groups.FirstAsync(g => g.GroupName == groupName);
                    subGroup.GroupId = group.Id;

                    await db.AddAsync(subGroup);
                    await db.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError("", "مشکلی به وجود آمد");
                }
            }
            else
            {
                ModelState.AddModelError("", "مشکلی به وجود آمد لطفا دوباره تلاش کنید");
            }
            return View(subGroup);
        }





        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var subGroup = await db.SubGroups.FindAsync(id);
            if (subGroup == null)
            {
                return NotFound();
            }

            var list = await db.Groups.AsNoTracking().Select(g => g.GroupName).ToListAsync();
            var group = await db.Groups.FindAsync(subGroup.GroupId);
            ViewBag.Groups = new SelectList(list, selectedValue: group.GroupName);
            
            return View(subGroup);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SubGroup subGroup, string groupName)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    var group = await db.Groups.FirstAsync(g => g.GroupName == groupName);
                    subGroup.GroupId = group.Id;

                    await Task.Run(() =>
                    {
                        db.Update(subGroup);
                    });
                    await db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError("", "مشکلی به وجود آمد");
                }
            }
            else
            {
                ModelState.AddModelError("", "مشکلی به وجود آمد");
            }
            return View(subGroup);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var subGroup = await db.SubGroups.FindAsync(id);
            if (subGroup == null)
            {
                return NotFound();
            }
            return View(subGroup);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(SubGroup subGroup)
        {
            try
            {
                subGroup = await db.SubGroups.FindAsync(subGroup.Id);


                await Task.Run(() =>
                {
                    db.Remove(subGroup);
                });
                await db.SaveChangesAsync();


                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "مشکلی در حذف محصول به وجود آمد");
            }
            return View(subGroup);

        }
    }
}
