using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PoshakMonix.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using PoshakMonix.Data;
using Microsoft.EntityFrameworkCore;
using PoshakMonix.Models.Entities;

namespace PoshakMonix.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PoshakMonixContext db;

        public HomeController(ILogger<HomeController> logger, PoshakMonixContext db)
        {
            _logger = logger;
            this.db = db;
        }

        public async Task<IActionResult> Index()
        {
            var model = new HomePageViewModel() {
                SliderImages = await db.SliderImages.AsNoTracking().ToListAsync(),
                NewestProducts = await db.Products.AsNoTracking().Include(p => p.Groups).OrderBy(p => p.UpdateDate).Take(10).ToListAsync(),
                SecoundCardSliderItems = await db.SecooundCardSliderItems.Include(s => s.Product).ThenInclude(s => s.Groups).Select(s => s.Product)
                .AsNoTracking().ToListAsync()
            };

            return View(model);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var product = await db.Products.Include(p => p.Comments.Where(c => c.Confirmed))
                .AsNoTracking()
                .Include(p => p.Groups).ThenInclude(g => g.Group)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);


            if(product == null)
            {
                return NotFound();
            }


            return View(product);
        }



        [HttpGet]
        public async Task<IActionResult> Search()
        {
            var model = new SearchViewModel();
            model.Groups = await db.SubGroups.AsNoTracking().ToListAsync();
            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> Search(SearchViewModel model, List<int> groupIds, int id = 1)
        {
            if(groupIds.Count != 0)
            {
                foreach (var item in groupIds)
                {
                    var group = await db.SubGroups.Include(s => s.Products).SingleAsync(s => s.Id == item);
                    var items = group.Products.Where(p => p.ProductName.Contains(model.SearchText)).ToList();
                    model.Products.Items.AddRange(items);
                }
            }
            else
            {
                model.Products.Items = await db.Products.Where(p => p.ProductName.Contains(model.SearchText)).AsNoTracking().ToListAsync();
            }
            
            model.Groups = await db.SubGroups.AsNoTracking().ToListAsync();

            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> AddComment(string userName, string email, string commentText , int productId)
        {
            Comment comment= new Comment()
            {
                ProductId = productId,
                UserName = userName,
                Email = email,
                Confirmed = false,
                Text = commentText
            };

            await db.Comments.AddAsync(comment);
            await db.SaveChangesAsync();

            return Redirect($"/Home/Detail/{productId}");
        }


        [HttpGet]
        public IActionResult Support()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SupportMessage(SupportMessage message)
        {
            await db.SupportMessages.AddAsync(message);
            await db.SaveChangesAsync();

            return RedirectToAction("Support","Home");
        }


    }
}
