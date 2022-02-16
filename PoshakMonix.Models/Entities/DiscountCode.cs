using PoshakMonix.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoshakMonix.Models.Entities
{
    public class DiscountCode: Entity
    {
        [Display(Name = "درصد")]
        [Required( ErrorMessage = "لظفا {0} را وارد کنید")]
        public int Percent { get; set; }
        [Display(Name = "کد")]
        [Required(ErrorMessage = "لظفا {0} را وارد کنید")]
        [MaxLength(10,ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string Code { get; set; }
    }
}
