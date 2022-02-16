using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoshakMonix.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Display(Name = "نام کاربری")]
        [Required( ErrorMessage = "لظفا {0} را وارد کنید")]
        [MaxLength(200,ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string UserName { get; set; }


        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لظفا {0} را وارد کنید")]
        [EmailAddress]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string Email { get; set; }


        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "لظفا {0} را وارد کنید")]
        [DataType(DataType.Password)]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string Password { get; set; }
        [Display(Name = "تکرار رمز عبور")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password),ErrorMessage = "رمز عبور و تکرارش باید برابر باشند")]
        public string ConfirmPassword { get; set; }

    }

    public class LoginViewModel
    {
        [Display(Name = "نام کاربری یا ایمیل")]
        [Required(ErrorMessage = "لظفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string UserNameOrEmail { get; set; }


        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "لظفا {0} را وارد کنید")]
        [DataType(DataType.Password)]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string Password { get; set; }

        [Display(Name = "مرا به خاطر بسپار")]
        public bool RememberMe { get; set; }


        public AuthenticationScheme ExternalLogin { get; set; }

    }

    public class ForgotPasswordViewModel
    {
        [MaxLength(150, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        [EmailAddress]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }
    }
    public class ResetPasswordViewModel
    {
        [DataType(DataType.Password)]
        [Display(Name = "رمز عبور")]
        public string Password { get; set; }

        [Display(Name = "تکرار رمز عبور")]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

        public string Email { get; set; }
        public string Token { get; set; }

    }
}
