using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PoshakMonix.Core.Utilities;
using PoshakMonix.Data;
using PoshakMonix.Web.Areas.Admin.Models;
using PoshakMonix.Models.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace PoshakMonix.Web.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly PoshakMonixContext db;

        public ProductController(PoshakMonixContext db)
        {
            this.db = db;
        }
        public async Task<IActionResult> Index()
        {
            var products = await db.Products.AsNoTracking().ToListAsync();
            return View(products);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Product product, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    #region Add Photo
                    if (file != null)
                    {
                        product.Image = Generator.GenerateUniqueName() + Path.GetExtension(file.FileName);
                        string folderPath = Directory.GetCurrentDirectory() + Path.Combine("/wwwroot/", "ProductImages/");
                        string path = folderPath + product.Image;

                        using (FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write))
                        {
                            await file.CopyToAsync(stream);
                        }
                    }
                    #endregion

                    product.UpdateDate = DateTime.Now;
                    await db.AddAsync(product);
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
            return View(product);
        }





        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    #region Edit Photo
                    if (imageFile != null)
                    {
                        string filePath = Directory.GetCurrentDirectory() + Path.Combine("/wwwroot/", "ProductImages/");
                        string path;


                        if (!string.IsNullOrWhiteSpace(product.Image))
                        {
                            path = filePath + product.Image;
                            System.IO.File.Delete(path);
                        }
                        product.Image = Generator.GenerateUniqueName() + Path.GetExtension(imageFile.FileName);
                        path = filePath + product.Image;


                        using (FileStream stream = new FileStream(path, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }
                    }
                    #endregion
                    product.UpdateDate = DateTime.Now;
                    await Task.Run(() =>
                    {
                        db.Update(product);
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
            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Product product)
        {
            try
            {
                product = await db.Products.FindAsync(product.Id);

                if(product.Image != null)
                {
                    string path = Directory.GetCurrentDirectory() + Path.Combine("/wwwroot/", "ProductImages/", product.Image);

                    await Task.Run(() =>
                    {
                        System.IO.File.Delete(path);
                    });
                }

                var cartDetails = await db.CartDetails.Where(c => c.ProductId == product.Id).ToListAsync();
                await Task.Run(() =>
                {
                    db.Products.Remove(product);
                    db.RemoveRange(cartDetails);
                });

                await db.SaveChangesAsync();



                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "مشکلی در حذف محصول به وجود آمد");
            }
            return View(product);

        }

        [HttpGet]
        public async Task<IActionResult> AddProductToGroup(int id)
        {
            var product = await db.Products.FindAsync(id);
            var groups = await db.SubGroups.AsNoTracking().ToListAsync();
            var model = new AddProductToGroupViewModel()
            {
                Product = product,
                Groups = groups
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> AddProductToGroup(AddProductToGroupViewModel model , int groupId)
        {
            try
            {

                var product = await db.Products.Include( p => p.Groups).FirstOrDefaultAsync( p => p.Id == model.Product.Id);
                var group = await db.SubGroups.FindAsync(groupId);
                if(product != null && group != null)
                {
                    product.Groups.Add(group);
                    await db.SaveChangesAsync();

                    return RedirectToAction(nameof(Index), "Product");
                }
                else
                {
                    throw new NullReferenceException();
                }
            }
            catch
            {
                ModelState.AddModelError("", "مشکلی به وجود آمد");
            }

            return View();
        }
    }
}
