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
    public class FactorsController : Controller
    {
        private readonly PoshakMonixContext db;

        public FactorsController(PoshakMonixContext db)
        {
            this.db = db;
        }
        public async Task<IActionResult> Index()
        {
            var factors = await db.Factors.AsNoTracking().ToListAsync();
            return View(factors);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Postman")]
        public async Task<IActionResult> Confirm(int id)
        {
            try
            {
                var factor = await db.Factors.FindAsync(id);
                factor.Sended = true;
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "مشکلی به وجود آمد");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Payed(int id)
        {
            try
            {
                var factor = await db.Factors.FindAsync(id);
                factor.IsPayed = true;
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
            var factor = await db.Factors.FindAsync(id);
            if (factor == null)
            {
                return NotFound();
            }
            return View(factor);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Factor factor)
        {
            try
            {
                factor = await db.Factors.FindAsync(factor.Id);


                await Task.Run(() =>
                {
                    db.Remove(factor);
                });
                await db.SaveChangesAsync();


                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "مشکلی به وجود آمد");
            }
            return View(factor);

        }
    }
}
