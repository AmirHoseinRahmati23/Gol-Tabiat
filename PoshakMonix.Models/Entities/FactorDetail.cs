using PoshakMonix.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PoshakMonix.Models.Entities
{
    public class FactorDetail : Entity
    {
        [Display(Name = "نام محصول")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
        [MaxLength(150, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string ProductName { get; set; }

        public int Price { get; set; }
        //Navigation Properties
        public int FactorId { get; set; }
        [ForeignKey(nameof(FactorId))]
        public Factor Factor { get; set; }

    }
}