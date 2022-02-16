using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class CommentsController : Controller
    {
        private readonly PoshakMonixContext db;

        public CommentsController(PoshakMonixContext db)
        {
            this.db = db;
        }
        public async Task<IActionResult> Index()
        {
            var comments = await db.Comments.AsNoTracking().ToListAsync();
            return View(comments);
        }


        [HttpPost]
        public async Task<IActionResult> Confirm(int id)
        {
            try
            {
                var comment = await db.Comments.FindAsync(id);
                comment.Confirmed = true;
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "مشکلی به وجود آمد");
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var comment = await db.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            return View(comment);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Comment comment)
        {
            try
            {
                comment = await db.Comments.FindAsync(comment.Id);


                await Task.Run(() =>
                {
                    db.Remove(comment);
                });
                await db.SaveChangesAsync();


                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "مشکلی به وجود آمد");
            }
            return View(comment);

        }
    }
}
