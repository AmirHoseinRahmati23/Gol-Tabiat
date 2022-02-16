using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PoshakMonix.Core.Utilities;
using PoshakMonix.Data;
using PoshakMonix.Models.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PoshakMonix.Web.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    [Authorize(Roles = "Admin")]
    public class SliderImagesController : Controller
    {
        private readonly PoshakMonixContext db;

        public SliderImagesController(PoshakMonixContext db)
        {
            this.db = db;
        }
        public async Task<IActionResult> Index()
        {
            var images = await db.SliderImages.AsNoTracking().ToListAsync();
            return View(images);
        }




        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(IFormFile imageFile)
        {
            if (imageFile != null)
            {

                try
                {
                string fileName = Generator.GenerateUniqueName() + Path.GetExtension(imageFile.FileName);
                string folderPath = Directory.GetCurrentDirectory() + Path.Combine("/wwwroot/", "SliderImages/");
                string path = folderPath + fileName;

                using (FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    await imageFile.CopyToAsync(stream);
                }

                    SliderImage image = new SliderImage()
                    {
                        Image = fileName
                    };
                    await db.AddAsync(image);
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
                ModelState.AddModelError("", "فایل خالیست :/");
            }
            return View();
        }




        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var image = await db.SliderImages.FindAsync(id);


                await Task.Run(() =>
                {
                    db.Remove(image);
                });
                await db.SaveChangesAsync();


                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "مشکلی به وجود آمد");
            }

            return View();

        }
    }
}
