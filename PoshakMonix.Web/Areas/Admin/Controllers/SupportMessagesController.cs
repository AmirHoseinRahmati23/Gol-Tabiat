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
using UnitOfWork.Repositories;

namespace PoshakMonix.Web.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    [Authorize(Roles = "Admin")]
    public class SupportMessagesController : Controller
    {
        private readonly PoshakMonixContext _db;
        private readonly IEmailSender _emailSender;

        public SupportMessagesController(PoshakMonixContext db , IEmailSender emailSender)
        {
            _db = db;
            _emailSender = emailSender;
        }
        public async Task<IActionResult> Index()
        {
            var messages = await _db.SupportMessages.AsNoTracking().ToListAsync();
            return View(messages);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var message = await _db.SupportMessages.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }
            return View(message);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(SupportMessage message)
        {
            try
            {
                var supportMessage = await _db.Products.FindAsync(message.Id);

                

                await Task.Run(() =>
                {
                    _db.Products.Remove(supportMessage);
                });
                await _db.SaveChangesAsync();


                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "مشکلی در حذف محصول به وجود آمد");
            }
            return View(message);

        }


        [HttpGet]
        public async Task<IActionResult> SendMessage(int id)
        {
            var message = await _db.SupportMessages.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }
            message.Text = default;
            return View(message);
        }
        [HttpPost]
        public async Task<IActionResult> SendMessage(SupportMessage message)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "عملیات با شکست مواجه شد");
                return View();
            }
            await _emailSender.SendEmailAsync(message.Email, "جواب پشتیبانی گل طبیعت", message.Text);

            
            return RedirectToAction(nameof(Index), (nameof(SupportMessage) + "s") );
        }
    }
}
