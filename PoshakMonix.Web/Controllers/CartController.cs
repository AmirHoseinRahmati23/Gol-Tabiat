using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PoshakMonix.Data;
using PoshakMonix.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using PoshakMonix.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using ZarinPal;

namespace PoshakMonix.Web.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly PoshakMonixContext db;
        private readonly UserManager<User> userManager;

        public CartController(PoshakMonixContext db, UserManager<User> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await userManager.GetUserAsync(HttpContext.User);

            var cart = db.Carts.Include(c => c.CartDetails).SingleOrDefault(c => !c.IsFinaly && c.UserId == user.Id);
            
            if(cart == null) ViewBag.Error = "سبد خریدی وجود ندارد";
            else ViewBag.Error = "";

            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> FinilizeCart(int id , string address)
        {
            var cart = await db.Carts.Include(c => c.CartDetails).FirstAsync(c => c.Id == id);

            if(cart.CartDetails == null)
            {
                return RedirectToAction(nameof(Index), "Home");
            }

            var currentUser = await userManager.GetUserAsync(User);

            var factor = new Factor()
            {
                Address = address,
                IsPayed = false,
                Sended = false,
                UserId = currentUser.Id,
                CartId = cart.Id
            };

            await db.Factors.AddAsync(factor);
            await db.SaveChangesAsync();


            foreach (var item in cart.CartDetails)
            {
                await db.FactorDetails.AddAsync(new FactorDetail()
                {
                    FactorId = factor.Id,
                    ProductName = item.Name,
                    Price = item.Price
                });
            }
            await db.SaveChangesAsync();


            return RedirectToAction(nameof(Payment), "Cart");
        }

        [HttpGet]
        public async Task<IActionResult> Add(int id)
        {
            try
            {
                var product = await db.Products.FindAsync(id);
                if (product != null)
                {
                    var user = await userManager.GetUserAsync(User);

                    #region Get Or Create Cart

                    var cart = db.Carts.Include(c => c.CartDetails).SingleOrDefault(c => !c.IsFinaly && c.UserId == user.Id);


                    if(cart == null)
                    {
                        cart = new Cart() {
                            UserId = user.Id,
                            IsFinaly = false,
                            CartDetails = new List<CartDetail>()
                        };


                        await db.Carts.AddAsync(cart);
                        await db.SaveChangesAsync();
                    }

                    #endregion

                    if(!cart.CartDetails.Any(c => c.ProductId == product.Id))
                    {
                        var cartDetail = new CartDetail()
                        {
                            Name = product.ProductName,
                            Price = product.Price,
                            Quantity = 1,
                            CartId = cart.Id,
                            ProductId = product.Id
                        };

                        cart.CartDetails.Add(cartDetail);
                    }
                    else
                    {
                        var cartDetail = cart.CartDetails.Single(c => c.ProductId == product.Id);
                        cartDetail.Quantity++;
                    }
                    await db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index), "Cart");

                }
                else
                {
                    throw new NullReferenceException();
                }
            }
            catch
            {
                ViewBag.Error = "مشکلی به وجود آمد";
            }

            return RedirectToAction(nameof(Index) , "Cart");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var cartDetail = await db.CartDetails.FindAsync(id);
                db.Remove(cartDetail);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index), "Cart");
            }
            catch
            {
                return NotFound();
            }

        }
        [HttpGet]
        public async Task<IActionResult> Payment()
        {
            var user = await userManager.GetUserAsync(User);
            var cart = db.Carts.Include(c => c.CartDetails).SingleOrDefault(o => !o.IsFinaly && o.UserId == user.Id);
            if(cart == null)
            {
                return NotFound();
            }

            string merchantId = "71c705f8-bd37-11e6-aa0c-000c295eb8fc";
            string callbackUrl = Url.ActionLink(nameof(VerifyPayment), "Cart") + "/" + cart.Id;
            long amount = cart.CartDetails.Sum(c => c.Price) * 1000;
            var description = $"خرید به مبلغ {cart.CartDetails.Sum(c => c.Price)}";


            var url = "https://sandbox.zarinpal.com/pg/rest/WebGate/PaymentRequest.json";
            var values = new Dictionary<string, string>
                {
                    { "MerchantID", merchantId },
                    { "Amount", amount.ToString() }, //Toman 
                    { "CallbackURL", callbackUrl },
                    { "Mobile", "09148001392" }, 
                    { "Description", description }
                };

            var paymentRequestJsonValue = JsonConvert.SerializeObject(values);
            var content = new StringContent(paymentRequestJsonValue, Encoding.UTF8, "application/json");


            var response = await new HttpClient().PostAsync(url, content);
            var responseString = await response.Content.ReadAsStringAsync();

            ViewBag.StatusCode = response.StatusCode;
            ViewBag.responseString = responseString;

            ZarinPalRequestResponseModel zarinPalResponseModel =
             JsonConvert.DeserializeObject<ZarinPalRequestResponseModel>(responseString);

            ViewBag.IsSuccess = false;
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                ViewBag.Error = true; // Post Error
                return View("ResultOfPayment");
            }

            if (zarinPalResponseModel.Status != 100 && zarinPalResponseModel.Status != 101) //Zarinpal Did not Accepted the payment
            {
                ViewBag.Error = true; // Post Error
                return View("ResultOfPayment");
            }

            // [/ُSad] will redirect to the sadad gateway if you already have zarin gate enabled, let's read here
            // https://www.zarinpal.com/blog/زرین-گیت،-درگاهی-اختصاصی-به-نام-وبسایت/

            return Redirect("https://sandbox.zarinpal.com/pg/StartPay/" + zarinPalResponseModel.Authority +"/Sad");
        }

        public async Task<IActionResult> VerifyPayment(string authority, string status , int cartId)
        {
            var url = "https://sandbox.zarinpal.com/pg/rest/WebGate/PaymentVerification.json";
            string merchantId = "71c705f8-bd37-11e6-aa0c-000c295eb8fc";

            var user = await userManager.GetUserAsync(User);
            var cart = db.Carts.Include(c => c.CartDetails).SingleOrDefault(o => !o.IsFinaly && o.UserId == user.Id);
            var amount = (cart.CartDetails.Sum(c => c.Price) * 1000).ToString();
            var values = new Dictionary<string, string>
                {
                    { "MerchantID", merchantId }, //Change This To work, something like this : xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
                    { "Authority", authority },
                    { "Amount", amount } //Toman
                };

            var paymenResponsetJsonValue = JsonConvert.SerializeObject(values);
            var content = new StringContent(paymenResponsetJsonValue, Encoding.UTF8, "application/json");

            var response = await new HttpClient().PostAsync(url, content);
            var responseString = await response.Content.ReadAsStringAsync();

            ViewBag.responseString = responseString;

            ZarinPalVerifyResponseModel _zarinPalResponseModel =
             JsonConvert.DeserializeObject<ZarinPalVerifyResponseModel>(responseString);
            
            
            if(_zarinPalResponseModel.Status == 101 || _zarinPalResponseModel.Status == 100)
            {
                ViewBag.RefId = _zarinPalResponseModel.RefID;
                ViewBag.IsSuccess = true;
                cart.IsFinaly = true;

                var factor = await db.Factors.FirstOrDefaultAsync(f => f.CartId == cart.Id);
                factor.IsPayed = true;

                await db.SaveChangesAsync();
            }
            else
            {
                ViewBag.IsSuccess = false;
            }
            
            return View("ResultOfPayment");
        }

        public async Task<IActionResult> ResultOfPayment()
        {
            if (HttpContext.Request.Query["Status"] != "" &&
                HttpContext.Request.Query["Status"].ToString().ToLower() == "ok" &&
                HttpContext.Request.Query["Authority"] != "")
            {
                var authority = HttpContext.Request.Query["Authority"];
                var user = await userManager.GetUserAsync(User);
                var cart = db.Carts.Include(c => c.CartDetails).SingleOrDefault(o => !o.IsFinaly && o.UserId == user.Id);
                //    var payment = new Payment(cart.CartDetails.Sum(c => c.Price));
                //    var result = await payment.Verification(authority);

                //    if (result.Status == 100)
                //    {
                //        ViewBag.RefId = result.RefId;
                //        ViewBag.IsSuccess = true;
                //        cart.IsFinaly = true;

                //        var factor = await db.Factors.SingleOrDefaultAsync(f => f.CartId == cart.Id);
                //        factor.IsPayed = true;

                //        await db.SaveChangesAsync();
                //    }
                //    else
                //    {
                //        ViewBag.IsSuccess = false;
                //    }
            }

                return View();
        }
    }

}
