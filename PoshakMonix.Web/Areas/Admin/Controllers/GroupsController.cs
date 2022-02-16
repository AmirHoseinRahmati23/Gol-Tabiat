using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class GroupsController : Controller
    {
        private readonly PoshakMonixContext db;

        public GroupsController(PoshakMonixContext db)
        {
            this.db = db;
        }
        public async Task<IActionResult> Index()
        {
            var groups = await db.Groups.AsNoTracking().ToListAsync();
            return View(groups);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Group group)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    await db.AddAsync(group);
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
            return View(group);
        }





        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var group = await db.Groups.FindAsync(id);
            if (group == null)
            {
                return NotFound();
            }
            return View(group);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Group group)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await Task.Run(() =>
                    {
                        db.Update(group);
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
            return View(group);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var group = await db.Groups.FindAsync(id);
            if (group == null)
            {
                return NotFound();
            }
            return View(group);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Group group)
        {
            try
            {
                group = await db.Groups.FindAsync(group.Id);


                await Task.Run(() =>
                {
                    db.Groups.Remove(group);
                });
                await db.SaveChangesAsync();


                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "مشکلی در حذف محصول به وجود آمد");
            }
            return View(group);

        }
    }
}
