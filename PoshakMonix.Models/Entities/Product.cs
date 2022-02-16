using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoshakMonix.Models.Base;
namespace PoshakMonix.Models.Entities
{
    public class Product : Entity
    {
        [Display(Name = "نام محصول")]
        [MaxLength(150,ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        [MinLength(1,ErrorMessage = "لطفا یچیزی وارد کنید")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
        public string ProductName { get; set; }
        [Display(Name = "سایز گلدان")]
        [MaxLength(50, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        [MinLength(1, ErrorMessage = "لطفا یچیزی وارد کنید")]
        public string Size { get; set; }
        [Display(Name = "جنس گلدان")]
        [MaxLength(100, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        [MinLength(1, ErrorMessage = "لطفا یچیزی وارد کنید")]
        public string Material{ get; set; }
        [Display(Name = "رنگ گلدان")]
        [MaxLength(100, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        [MinLength(1, ErrorMessage = "لطفا یچیزی وارد کنید")]
        public string Color { get; set; }

        [Display(Name = "ابلق هست؟")]
        public bool IsAblagh { get; set; }
        [Display(Name = "زیره دارد؟")]
        public bool HaveUnderPot { get; set; }

        [Display(Name = "خاک گل")]
        [MaxLength(100, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        [MinLength(1, ErrorMessage = "لطفا یچیزی وارد کنید")]
        public string FlowerSoil { get; set; }

        [Display(Name = "قیمت نهایی")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
        public int Price { get; set; }
        [Display(Name = "قیمت بدون تخفیف")]
        public int WholePrice { get; set; }
        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
        [MaxLength(500, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        [MinLength(1, ErrorMessage = "لطفا یچیزی وارد کنید")]
        public string Description { get; set; }
        [Display(Name = "عکس")]
        public string Image { get; set; }
        public DateTime UpdateDate { get; set; }


        [Display(Name = "تعداد")]
        public int Quantity { get; set; }


        //Navigation Properties
        public ICollection<SubGroup> Groups { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
