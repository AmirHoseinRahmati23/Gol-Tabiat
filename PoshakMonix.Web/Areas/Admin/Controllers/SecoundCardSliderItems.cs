using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PoshakMonix.Data;
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
    public class SecoundCardSliderItemsController : Controller
    {
        private readonly PoshakMonixContext db;

        public SecoundCardSliderItemsController(PoshakMonixContext db)
        {
            this.db = db;
        }
        public async Task<IActionResult> Index()
        {
            var cards = await db.SecooundCardSliderItems.Include(s => s.Product).AsNoTracking().ToListAsync();
            return View(cards);
        }



        public async Task<IActionResult> Add()
        {
            var products = await db.Products.AsNoTracking().Select(p => new ShowProductsInCardSliderManagementViewModel {
                Id = p.Id,
                Name = p.ProductName
            }).ToListAsync();

            return View(products);
        }

        [HttpPost]
        public async Task<IActionResult> Add(List<int> products)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var items = await db.SecooundCardSliderItems.ToListAsync();
                    await Task.Run(() =>
                    {
                        db.RemoveRange(items);
                    });
                    foreach (var id in products)
                    {
                        var card = new SecooundCardSliderItem()
                        {
                            ProductId = db.Products.AsNoTracking().Single(p => p.Id == id).Id
                        };


                        await db.AddAsync(card);
                        await db.SaveChangesAsync();
                    }

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
            return View();
        }






        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            SecooundCardSliderItem card;
            try
            {
                card = await db.SecooundCardSliderItems.FindAsync(id);


                await Task.Run(() =>
                {
                    db.Remove(card);
                });
                await db.SaveChangesAsync();


                return RedirectToAction(nameof(Index));
            }
            catch
            {
                card = null;
                ModelState.AddModelError("", "مشکلی به وجود آمد");
            }
            return View(card);

        }

    }
}
